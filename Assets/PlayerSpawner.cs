using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Cambiamos de Awake a Start para asegurar que CharacterManager.Instance ya esté listo
    void Start()
    {
        Debug.Log("<color=orange>PLAYER SPAWNER:</color> Se ha iniciado en la escena de juego.");

        // Comprueba si el CharacterManager existe
        if (CharacterManager.Instance != null)
        {
            Debug.Log("<color=orange>PLAYER SPAWNER:</color> CharacterManager encontrado.");

            // Comprueba si tiene un personaje seleccionado
            if (CharacterManager.Instance.selectedCharacterPrefab != null)
            {
                Debug.Log("<color=green>PLAYER SPAWNER:</color> Personaje seleccionado encontrado: " + CharacterManager.Instance.selectedCharacterPrefab.name + ". Creando instancia...");
                Debug.Log("<color=cyan>PLAYER SPAWNER:</color> Posición de instanciación: " + transform.position);
                // Crea una instancia del prefab del personaje seleccionado
                GameObject spawnedBird = Instantiate(CharacterManager.Instance.selectedCharacterPrefab, transform.position, Quaternion.identity);
                Debug.Log("<color=cyan>PLAYER SPAWNER:</color> Pájaro instanciado en: " + spawnedBird.transform.position);
            }
            else
            {
                Debug.LogWarning("<color=orange>PLAYER SPAWNER:</color> No se encontró un personaje seleccionado. Se usará el personaje por defecto.");
                // Fallback: Si no hay personaje seleccionado, crea el primero de la lista.
                if (CharacterManager.Instance.characterPrefabs.Length > 0)
                {
                    Debug.Log("<color=green>PLAYER SPAWNER:</color> Creando personaje por defecto: " + CharacterManager.Instance.characterPrefabs[0].name);
                    Debug.Log("<color=cyan>PLAYER SPAWNER:</color> Posición de instanciación: " + transform.position);
                    GameObject spawnedBird = Instantiate(CharacterManager.Instance.characterPrefabs[0], transform.position, Quaternion.identity);
                    Debug.Log("<color=cyan>PLAYER SPAWNER:</color> Pájaro instanciado en: " + spawnedBird.transform.position);
                }
                else
                {
                    Debug.LogError("<color=red>PLAYER SPAWNER:</color> ¡No hay prefabs de personaje en la lista del CharacterManager!");
                }
            }
        }
        else
        {
            Debug.LogError("<color=red>PLAYER SPAWNER:</color> ¡NO se pudo encontrar el CharacterManager.Instance! Asegúrate de empezar desde la escena MainMenu.");
        }
    }
}
