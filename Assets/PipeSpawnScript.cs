using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;
    public float spawnRate = 2;
    private float timer = 0;
    public float heightOffset = 5; // Define qué tan arriba o abajo pueden aparecer

    void Start()
    {
        // Llamamos a spawnPipe una vez al inicio
        spawnPipe();
    }

    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            // Cuando se cumple el tiempo, volvemos a llamar a spawnPipe
            spawnPipe();
            timer = 0;
        }
    }

    // Ahora la función hace todo el trabajo, por lo que es reutilizable
    void spawnPipe()
    {
        // 1. Calcula los límites de altura JUSTO ANTES de usarlos
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        // 2. Usa esas variables locales para crear la tubería
        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
    }
}