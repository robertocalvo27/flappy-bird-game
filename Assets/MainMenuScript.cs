using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [Header("Configuración del Menú")]
    public string gameSceneName = "SampleScene"; // Nombre de la escena del juego
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Mensaje de confirmación que el menú principal está funcionando
        Debug.Log("Menú Principal cargado correctamente");
    }

    /// <summary>
    /// Función que se ejecuta cuando se presiona el botón "Jugar"
    /// Carga la escena del juego principal
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Iniciando juego...");
        
        // Cargar la escena del juego (SampleScene)
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Función que se ejecuta cuando se presiona el botón "Salir"
    /// Cierra la aplicación
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Cerrando juego...");
        
        // En el editor de Unity, detiene el juego
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // En una build, cierra la aplicación
            Application.Quit();
        #endif
    }

    /// <summary>
    /// Función opcional para agregar efectos de sonido más adelante
    /// </summary>
    public void PlayButtonSound()
    {
        // Aquí se puede agregar sonido de botón más adelante
        Debug.Log("Sonido de botón (placeholder)");
    }
}
