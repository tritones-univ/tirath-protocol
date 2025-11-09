using UnityEngine;
using UnityEngine.SceneManagement;

public class SmartSceneLoader : MonoBehaviour
{
    public void LoadSceneAndSaveCurrent(string newSceneName)
    {
        if (SceneManager.GetActiveScene().name == newSceneName)
        {
            Debug.Log("Ya estás en la escena, no se recarga.");
            return; // No recargar
        }
        // 1️⃣ Guardar el estado de la escena actual
        var currentScene = SceneManager.GetActiveScene().name;

        var memoryController = FindObjectOfType<SceneMemoryController>();
        if (memoryController != null)
        {
            memoryController.SaveState();
            Debug.Log($"Estado de la escena '{currentScene}' guardado antes de cambiar.");
        }
        else
        {
            Debug.LogWarning("No se encontró SceneMemoryController en la escena actual.");
        }

        // 2️⃣ Cambiar de escena
        SceneManager.LoadScene(newSceneName);
    }
}
