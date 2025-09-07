using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -45;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mueve el objeto en la dirección 'izquierda' a la velocidad 'moveSpeed'
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        if (transform.position.x < deadZone)
        {
            Debug.Log("Pipe Deleted");
            Destroy(gameObject);
        }
    }
}
