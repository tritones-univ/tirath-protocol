using UnityEngine;
using UnityEngine.SceneManagement;

public class DestructibleObject : MonoBehaviour
{
  public string uniqueID;
  void Start()
  {
    string currentSceneName = SceneManager.GetActiveScene().name;

    if (GameStateManager.Instance != null && GameStateManager.Instance.HasVisited(currentSceneName))
    {
      SceneData sceneData = GameStateManager.Instance.GetSceneState(currentSceneName);

      if (sceneData == null)
      {
        return;
      }

      if (sceneData.destroyedObjects.Contains(uniqueID))
      {
        Destroy(gameObject);
      }
    }
  }

  // Llama a este método cuando el jugador destruya el objeto
  public void DestroyObjectByPlayer()
  {
    // 2. Registrar la destrucción en el GameStateManager antes de desaparecer
    if (GameStateManager.Instance != null)
    {
      string currentSceneName = SceneManager.GetActiveScene().name;
      GameStateManager.Instance.RegisterDestroyedObject(currentSceneName, uniqueID);
    }
  }
}