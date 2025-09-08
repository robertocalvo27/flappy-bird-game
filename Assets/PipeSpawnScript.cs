using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;
    public float spawnRate = 2;
    private float timer = 0;
    public float heightOffset = 5; // Define qué tan arriba o abajo pueden aparecer

    [Header("Power-ups")]
    public GameObject starPowerUpPrefab; // Referencia al prefab de la estrella
    [Range(0, 1)]
    public float powerUpSpawnChance = 0.2f; // 20% de probabilidad de que aparezca un power-up

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
        // Define la altura del hueco entre las tuberías.
        // Un valor más alto hace el juego más fácil.
        float gapSize = 10f; // Puedes ajustar este valor desde el Inspector si lo haces público.

        // Calcula los límites verticales seguros para el CENTRO del hueco.
        float screenTop = Camera.main.orthographicSize - (gapSize / 2);
        float screenBottom = -Camera.main.orthographicSize + (gapSize / 2);

        // Elige una posición Y aleatoria para el centro del hueco.
        float gapCenterY = Random.Range(screenBottom, screenTop);

        // Crea el prefab de las tuberías en la posición del spawner pero con la altura del centro del hueco.
        GameObject newPipe = Instantiate(pipe, new Vector3(transform.position.x, gapCenterY, 0), transform.rotation);

        // --- LÓGICA DE SPAWN DE POWER-UP ---
        // Con una cierta probabilidad, instancia un power-up en el centro del hueco.
        if (Random.value < powerUpSpawnChance) // Random.value devuelve un número entre 0 y 1
        {
            if (starPowerUpPrefab != null)
            {
                Instantiate(starPowerUpPrefab, new Vector3(transform.position.x, gapCenterY, 0), Quaternion.identity);
            }
        }
        
        // --- AJUSTE PRECISO DEL HUECO ---
        // Ahora, posicionamos las tuberías superior e inferior RELATIVO al centro del hueco.

        Transform topPipe = newPipe.transform.Find("TopPipe");
        Transform bottomPipe = newPipe.transform.Find("BottomPipe");

        if (topPipe == null || bottomPipe == null)
        {
            Debug.LogError("El prefab 'Pipes' no tiene hijos llamados 'TopPipe' y 'BottomPipe'.");
            return;
        }

        // Coloca la tubería superior justo encima del centro del hueco.
        topPipe.localPosition = new Vector3(0, gapSize / 2, 0);

        // Coloca la tubería inferior justo debajo del centro del hueco.
        bottomPipe.localPosition = new Vector3(0, -gapSize / 2, 0);
    }
}