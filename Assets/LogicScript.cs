using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public static LogicScript Instance { get; private set; } // Singleton instance
    public Text scoreText;
    public GameObject gameOverScreen;
    
    [Header("Temporizador")]
    public float gameTimeLimit = 60f; // Límite de tiempo en segundos
    public Text timerText; // Referencia al texto del temporizador
    private float timeLeft;
    private bool timerIsRunning = false;
    
    [Header("Partículas")]
    public GameObject scoreParticlesPrefab; // Prefab de partículas de puntuación

    [Header("Audio")]
    private AudioSource[] audioSources; // Array de AudioSources
    private AudioSource scoreAudioSource; // Sonido de puntuar
    private AudioSource gameOverAudioSource; // Sonido de game over

    void Awake()
    {
        // Configurar el Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Obtener todos los AudioSources del GameObject
        audioSources = GetComponents<AudioSource>();
        
        // Asignar los AudioSources específicos
        if (audioSources.Length >= 2)
        {
            scoreAudioSource = audioSources[0]; // Primer AudioSource (point-sound)
            gameOverAudioSource = audioSources[1]; // Segundo AudioSource (gameover-sound)
        }

        // Iniciar el temporizador
        timeLeft = gameTimeLimit;
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                Debug.Log("¡Se acabó el tiempo!");
                timeLeft = 0;
                timerIsRunning = false;
                gameOver(); // Llama a la función de Game Over
            }
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            // Muestra el tiempo redondeado al entero más cercano
            timerText.text = "Tiempo: " + Mathf.CeilToInt(timeLeft).ToString();
        }
    }

    [ContextMenu("Increase Score")]
    public void addScore()
    {
        playerScore = playerScore + 1;
        scoreText.text = playerScore.ToString();
        // Este mensaje nos confirmará que la puntuación se está sumando
        Debug.Log("Puntuación Aumentada a: " + playerScore);
        
        // Reproducir sonido de puntuar
        if (scoreAudioSource != null)
        {
            scoreAudioSource.Play();
        }

        // Instanciar partículas de puntuación
        // Necesitamos una referencia al objeto que activa el trigger para saber dónde instanciarlas.
        // Esto lo añadiremos en pipeMiddleScript.
    }

    public void TriggerScoreParticles(Vector3 position)
    {
        if (scoreParticlesPrefab != null)
        {
            Instantiate(scoreParticlesPrefab, position, Quaternion.identity);
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
        
        // Reproducir sonido de game over
        if (gameOverAudioSource != null)
        {
            gameOverAudioSource.Play();
        }
    }
}