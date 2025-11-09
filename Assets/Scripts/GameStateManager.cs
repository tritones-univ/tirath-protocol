using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    // Diccionario: nombre de escena → estado guardado
    private Dictionary<string, SceneData> sceneStates = new Dictionary<string, SceneData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Guardar o actualizar el estado de una escena
    public void SaveSceneState(string sceneName, SceneData data)
    {
        sceneStates[sceneName] = data;
    }

    // Recuperar el estado de una escena si existe
    public SceneData GetSceneState(string sceneName)
    {
        if (sceneStates.TryGetValue(sceneName, out SceneData data))
            return data;
        return null; // No hay estado guardado
    }

    // Verificar si ya se visitó una escena
    public bool HasVisited(string sceneName)
    {
        return sceneStates.ContainsKey(sceneName);
    }
}

// Clase serializable con los datos de una escena
[System.Serializable]
public class SceneData
{
    public Vector3 playerPosition;
    public List<string> destroyedObjects = new List<string>();
    public List<string> openedChests = new List<string>();
    // Agrega aquí lo que quieras guardar del mapa
}
