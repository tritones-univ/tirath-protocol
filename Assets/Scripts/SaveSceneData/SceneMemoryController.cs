using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMemoryController : MonoBehaviour
{
    public static SceneMemoryController Instance;
    private string sceneName;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("GameStateManager no encotrado");
            return;
        }
        if (GameStateManager.Instance.HasVisited(sceneName))
        {
            Debug.Log("Restaurando scena");
            LoadState();
        }
        else
        {
            SaveState(new SceneData());
        }
    }
    public void LoadState()
    {
        SceneData data = GameStateManager.Instance.GetSceneState(sceneName);

        if (data == null) return;

        foreach (var structure in data.builtStructures)
        {
            GameObject prefabToInstantiate = GameStateManager.Instance.GetPrefabByID(structure.prefabID);
            if (prefabToInstantiate != null)
            {
                Instantiate(prefabToInstantiate, structure.position, structure.rotation);
            }
            else
            {
                Debug.LogError($"Error: Prefab con ID '{structure.prefabID}' no encontrado en GameStateManager.");
            }
        }

    }
    public void SaveCurrentState()
    {
        if (GameStateManager.Instance == null) return;

        // 1. Crea un nuevo objeto de datos para guardar el estado actual
        SceneData currentData = new SceneData();

        // 2. Encuentra todas las estructuras construidas actualmente en la escena
        // Necesitas que tus estructuras construidas tengan una etiqueta o un script
        // que las identifique fácilmente, por ejemplo, la etiqueta "BuiltStructure".
        GameObject[] structures = GameObject.FindGameObjectsWithTag("BuiltStructure");

        foreach (GameObject structure in structures)
        {
            // El nombre del objeto debería coincidir con el PrefabID
            string id = structure.name.Replace("(Clone)", "").Trim();

            BuiltStructureData builtData = new BuiltStructureData(
                id,
                structure.transform.position,
                structure.transform.rotation
            );

            currentData.builtStructures.Add(builtData);
        }

        // 3. Guarda la información en el gestor persistente
        GameStateManager.Instance.SaveSceneState(sceneName, currentData);
    }

    // Sobrecarga simple del método de guardado para la primera visita (estado vacío)
    private void SaveState(SceneData data)
    {
        GameStateManager.Instance.SaveSceneState(sceneName, data);
    }
}