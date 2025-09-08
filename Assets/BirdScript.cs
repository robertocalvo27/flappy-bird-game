using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrenght;
    public LogicScript logic;
    public bool birdIsAlive = true;
    
    [Header("Animación de Rotación")]
    public float rotationSpeed = 3f; // Velocidad de rotación
    public float maxRotationAngle = 25f; // Ángulo máximo de rotación
    public float minRotationAngle = -90f; // Ángulo mínimo de rotación (caída)
    
    [Header("Audio")]
    private AudioSource flyingAudioSource; // Sonido de volar
    
    [Header("Animator")]
    private Animator birdAnimator; // Componente Animator del pájaro

    [Header("Partículas")]
    public GameObject deathParticlesPrefab; // Prefab de partículas de muerte

    [Header("Límites de Altura")]
    public float topBoundary = 15f; // Límite superior de la pantalla
    public float bottomBoundary = -15f; // Límite inferior (causa Game Over)

    [Header("Power-up de Invencibilidad")]
    private bool isInvincible = false;
    public float invincibilityDuration = 5f; // Duración de la invencibilidad en segundos
    private SpriteRenderer birdSpriteRenderer;

    // Awake se llama justo al iniciar el objeto, antes que Start.
    // Es el lugar ideal para buscar componentes.
    void Awake()
    {
        // Esta línea busca el componente Rigidbody2D en el mismo GameObject
        // y lo asigna a tu variable 'myRigidbody'.
        myRigidbody = GetComponent<Rigidbody2D>();
        
        // Buscar el componente AudioSource para el sonido de volar
        flyingAudioSource = GetComponent<AudioSource>();
        
        // Buscar el componente Animator
        birdAnimator = GetComponent<Animator>();

        // Buscar el SpriteRenderer para cambiar el color
        birdSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            // Solo reduce la velocidad hacia abajo, no la elimina completamente
            // Esto hace que el vuelo sea más realista
            Vector2 currentVelocity = myRigidbody.linearVelocity;
            
            // Si está cayendo rápido, reduce la velocidad de caída antes de aplicar impulso
            if (currentVelocity.y < 0)
            {
                currentVelocity.y *= 0.3f; // Reduce la velocidad de caída al 30%
            }
            
            // Aplica impulso hacia arriba (más realista que resetear completamente)
            myRigidbody.linearVelocity = new Vector2(currentVelocity.x, currentVelocity.y + flapStrenght);
            
            // Reproducir sonido de volar
            if (flyingAudioSource != null)
            {
                flyingAudioSource.Play();
            }
        }

        // Aplicar rotación basada en la velocidad vertical
        if (birdIsAlive)
        {
            RotateBird();
        }

        // Comprobar límites de altura
        CheckBoundaries();
    }

    /// <summary>
    /// Comprueba si el pájaro se ha salido de los límites de la pantalla.
    /// </summary>
    void CheckBoundaries()
    {
        // Si el pájaro toca el límite superior, no le dejamos subir más
        if (transform.position.y > topBoundary)
        {
            // Forzamos al pájaro a quedarse en la posición del límite y anulamos su velocidad hacia arriba
            transform.position = new Vector3(transform.position.x, topBoundary, transform.position.z);
            if (myRigidbody.linearVelocity.y > 0)
            {
                myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x, 0);
            }
        }

        // Si el pájaro toca el límite inferior, es Game Over
        if (transform.position.y < bottomBoundary && birdIsAlive)
        {
            logic.gameOver();
            birdIsAlive = false;
        }
    }

    /// <summary>
    /// Se activa cuando el pájaro entra en un Trigger Collider.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprueba si ha chocado con un power-up
        if (other.gameObject.CompareTag("PowerUp"))
        {
            // Diferencia qué power-up ha recogido basándose en el nombre del prefab
            switch (other.gameObject.name)
            {
                case "StarPowerUp(Clone)":
                    // Activa la invencibilidad y puntos dobles
                    StartCoroutine(ActivateInvincibility());
                    break;
                case "TimePowerUp(Clone)":
                    // Añade tiempo extra
                    LogicScript.Instance.AddTime(5f); // Añade 5 segundos
                    break;
                case "PointsPowerUp(Clone)":
                    // Añade puntos extra
                    LogicScript.Instance.AddExtraPoints(5); // Añade 5 puntos
                    break;
            }

            // Destruye el objeto del power-up
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// Activa la invencibilidad del pájaro por un tiempo limitado.
    /// </summary>
    private System.Collections.IEnumerator ActivateInvincibility()
    {
        isInvincible = true;
        LogicScript.Instance.doubleScoreActive = true; // Activa la puntuación doble
        Debug.Log("¡Pájaro es INVENCIBLE y con PUNTOS DOBLES!");

        // Cambia el color del pájaro para dar feedback visual
        if (birdSpriteRenderer != null)
        {
            birdSpriteRenderer.color = Color.yellow; // Cambia a amarillo brillante
        }

        // Espera por la duración de la invencibilidad
        yield return new WaitForSeconds(invincibilityDuration);

        // Desactiva la invencibilidad y restaura el color
        isInvincible = false;
        LogicScript.Instance.doubleScoreActive = false; // Desactiva la puntuación doble
        if (birdSpriteRenderer != null)
        {
            birdSpriteRenderer.color = Color.white; // Restaura el color original
        }
        Debug.Log("Invencibilidad TERMINADA.");
    }

    /// <summary>
    /// Rota el pájaro según su velocidad vertical para crear animación realista
    /// </summary>
    void RotateBird()
    {
        // Obtener la velocidad vertical actual
        float velocityY = myRigidbody.linearVelocity.y;
        
        // Calcular el ángulo de rotación basado en la velocidad
        // Velocidad positiva (subiendo) = rotación hacia arriba
        // Velocidad negativa (cayendo) = rotación hacia abajo
        float targetRotation;
        
        if (velocityY > 0)
        {
            // Pájaro subiendo - rotar hacia arriba
            targetRotation = Mathf.Lerp(0, maxRotationAngle, velocityY / flapStrenght);
        }
        else
        {
            // Pájaro cayendo - rotar hacia abajo gradualmente
            targetRotation = Mathf.Lerp(0, minRotationAngle, Mathf.Abs(velocityY) / 10f);
        }
        
        // Limitar el ángulo dentro de los rangos establecidos
        targetRotation = Mathf.Clamp(targetRotation, minRotationAngle, maxRotationAngle);
        
        // Aplicar rotación suave usando Lerp
        float currentRotation = transform.eulerAngles.z;
        // Convertir ángulos para manejar correctamente el rango 0-360
        if (currentRotation > 180) currentRotation -= 360;
        
        float newRotation = Mathf.LerpAngle(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el pájaro es invencible, ignora la colisión con las tuberías.
        if (isInvincible)
        {
            // Podríamos añadir aquí una comprobación de si la colisión es con una tubería,
            // pero por ahora, la invencibilidad lo protege de todo.
            return;
        }

        logic.gameOver();
        birdIsAlive = false;
        
        // Detener la animación de aleteo
        if (birdAnimator != null)
        {
            birdAnimator.enabled = false;
        }

        // Instanciar partículas de muerte
        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }
    }
}
