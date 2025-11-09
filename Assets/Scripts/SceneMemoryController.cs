using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMemoryController : MonoBehaviour
{
    private string sceneName;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;

        // Si ya se visitó antes, restaurar
        if (GameStateManager.Instance.HasVisited(sceneName))
        {
            LoadState();
        }
        else
        {
            // Primera vez que se visita
            SaveState(); 
        }
    }

    public void SaveState()
    {
        SceneData data = new SceneData();

        // Ejemplo: guardar posición del jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            data.playerPosition = player.transform.position;

        // Aquí podrías recorrer cofres, enemigos, etc.
        // y guardar cuáles fueron destruidos o abiertos.

        GameStateManager.Instance.SaveSceneState(sceneName, data);
    }

    public void LoadState()
    {
        SceneData data = GameStateManager.Instance.GetSceneState(sceneName);

        // Restaurar posición del jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            player.transform.position = data.playerPosition;

        // Restaurar cofres, enemigos, etc. según el estado guardado.
    }
}
