using UnityEngine;
using UnityEngine.SceneManagement;

public class SmartSceneLoader : MonoBehaviour
{
    public void LoadSceneAndSaveCurrent(string newSceneName)
    {
        // Verifica si la escena ya está activa
        if (SceneManager.GetActiveScene().name == newSceneName)
        {
            Debug.Log("Ya estás en la escena, no se recarga.");
            return;
        }

        // 1️⃣ Guardar el estado de la escena actual
        var currentScene = SceneManager.GetActiveScene().name;

        // ** CORRECCIÓN: Acceder directamente al Singleton **
        var memoryController = SceneMemoryController.Instance;

        if (memoryController != null)
        {
            memoryController.SaveState();
            Debug.Log($"Estado de la escena '{currentScene}' guardado antes de cambiar.");
        }
        else
        {
            // Este caso debería ser raro si SceneMemoryController usa DontDestroyOnLoad
            Debug.LogWarning("No se encontró SceneMemoryController (Instance) en la escena actual.");
        }

        // 2️⃣ Cambiar de escena
        SceneManager.LoadScene(newSceneName);
    }
}