using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    // Almacenará el prefab del personaje que el jugador ha seleccionado
    public GameObject selectedCharacterPrefab;
    
    // Lista de todos los prefabs de personajes disponibles
    public GameObject[] characterPrefabs;

    private void Awake()
    {
        // Configuración del Singleton para que no se destruya entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ¡La clave para recordar la elección!
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Este método será llamado por los botones en el menú principal.
    /// </summary>
    /// <param name="characterIndex">El índice del personaje en la lista (0 para el primero, 1 para el segundo, etc.)</param>
    public void SelectCharacter(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < characterPrefabs.Length)
        {
            selectedCharacterPrefab = characterPrefabs[characterIndex];
            Debug.Log("<color=green>CHARACTER MANAGER:</color> Personaje #" + characterIndex + " (" + selectedCharacterPrefab.name + ") seleccionado y guardado.");
            Debug.Log("<color=yellow>CHARACTER MANAGER:</color> Lista completa de personajes:");
            for (int i = 0; i < characterPrefabs.Length; i++)
            {
                Debug.Log("<color=yellow>CHARACTER MANAGER:</color> Índice " + i + ": " + characterPrefabs[i].name);
            }
        }
        else
        {
            Debug.LogError("Índice de personaje inválido: " + characterIndex);
        }
    }
}
