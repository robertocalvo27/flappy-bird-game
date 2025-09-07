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


    // Awake se llama justo al iniciar el objeto, antes que Start.
    // Es el lugar ideal para buscar componentes.
    void Awake()
    {
        // Esta línea busca el componente Rigidbody2D en el mismo GameObject
        // y lo asigna a tu variable 'myRigidbody'.
        myRigidbody = GetComponent<Rigidbody2D>();
        
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
        }

        // Aplicar rotación basada en la velocidad vertical
        if (birdIsAlive)
        {
            RotateBird();
        }
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
        logic.gameOver();
        birdIsAlive = false;
    }
}
