using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    [Tooltip("La velocidad base con la que se mueven las tuberías al inicio del juego.")]
    public float moveSpeed = 5;
    public float deadZone = -45;
    
    [Header("Dificultad Progresiva")]
    [Tooltip("Cuánto aumenta la velocidad por cada punto. Un valor de 0.3 es un buen comienzo.")]
    public float speedIncreasePerPoint = 0.3f; // Aumentado desde 0.1 para que sea más notable
    private float currentMoveSpeed;

    void Start()
    {
        // La velocidad inicial es la velocidad base
        currentMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Calcular la velocidad actual basada en la puntuación
        // Asegurarse de que LogicScript.Instance no sea nulo para evitar errores al inicio
        if (LogicScript.Instance != null)
        {
            currentMoveSpeed = moveSpeed + (LogicScript.Instance.playerScore * speedIncreasePerPoint);
        }

        // Mueve el objeto en la dirección 'izquierda' a la velocidad 'currentMoveSpeed'
        transform.Translate(Vector3.left * currentMoveSpeed * Time.deltaTime);
        
        if (transform.position.x < deadZone)
        {
            Debug.Log("Pipe Deleted");
            Destroy(gameObject);
        }
    }
}
