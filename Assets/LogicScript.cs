using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public Text scoreText;
    public GameObject gameOverScreen;
    
    [Header("Audio")]
    private AudioSource[] audioSources; // Array de AudioSources
    private AudioSource scoreAudioSource; // Sonido de puntuar
    private AudioSource gameOverAudioSource; // Sonido de game over

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