using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadNewScene(string sceneName)
    {
        if (SceneMemoryController.Instance != null)
        {
            SceneMemoryController.Instance.SaveCurrentState();
            Debug.Log("Guardando estado de la scena saliente");
        }
        SceneManager.LoadScene(sceneName);
    }
}