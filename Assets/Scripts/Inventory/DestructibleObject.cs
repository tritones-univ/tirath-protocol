using UnityEngine;
using UnityEngine.SceneManagement;

public class DestructibleObject : MonoBehaviour
{
  public string uniqueID;
  void Start()
  {
    // 1. Verificar si ya debería estar destruido
    if (GameStateManager.Instance != null && GameStateManager.Instance.HasVisited(SceneManager.GetActiveScene().name))
    {
      SceneData sceneData = GameStateManager.Instance.GetSceneState(SceneManager.GetActiveScene().name);

      if (sceneData != null && sceneData.destroyedObjects.Contains(uniqueID))
      {
        // Si la memoria dice que fue destruido, lo destruimos al instante.
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

    // 3. Destruir el objeto de la escena
    Destroy(gameObject);
  }
}