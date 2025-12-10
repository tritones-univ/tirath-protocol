using System.Collections.Generic;
using UnityEngine;


public class GameStateManager : MonoBehaviour
{
  public static GameStateManager Instance;
  private Dictionary<string, SceneData> sceneStates = new Dictionary<string, SceneData>();
  public GameObject[] buildablePrefabs;

  void Awake()
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
  public void SaveSceneState(string sceneName, SceneData data)
  {
    sceneStates[sceneName] = data;
    Debug.Log("Estado de la scena guadado");
  }
  public SceneData GetSceneState(string sceneName)
  {
    if (sceneStates.ContainsKey(sceneName))
    {
      return sceneStates[sceneName];
    }
    return null;
  }
  public bool HasVisited(string sceneName)
  {
    return sceneStates.ContainsKey(sceneName);
  }
  public GameObject GetPrefabByID(string id)
  {
    foreach (var prefab in buildablePrefabs)
    {
      // Asumimos que el prefabID es el nombre del GameObject (prefab.name)
      if (prefab.name == id)
      {
        return prefab;
      }
    }
    return null;
  }
  public void RegisterDestroyedObject(string sceneName, string objectId)
  {
    if (!sceneStates.ContainsKey(sceneName))
    {
      sceneStates[sceneName] = new SceneData();
    }
    SceneData data = sceneStates[sceneName];

    if (!data.destroyedObjects.Contains(objectId))
    {
      data.destroyedObjects.Add(objectId);
    }
  }
}