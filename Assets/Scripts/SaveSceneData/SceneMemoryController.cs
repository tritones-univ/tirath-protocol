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

        string currentSceneName = SceneManager.GetActiveScene().name;

        // --- CAMBIO CLAVE: RECUPERAR DATOS ANTERIORES EN LUGAR DE CREAR NUEVOS ---
        SceneData currentData = GameStateManager.Instance.GetSceneState(currentSceneName);

        // Si no existía (raro, pero posible si no se llamó al Start), creamos el base.
        if (currentData == null)
        {
            currentData = new SceneData();
        }

        // --- LIMPIAR Y RECALCULAR ESTRUCTURAS CONSTRUIDAS (para no duplicar) ---
        currentData.builtStructures.Clear(); // Limpiamos la lista de estructuras

        // 2. Encuentra todas las estructuras construidas actualmente en la escena
        GameObject[] structures = GameObject.FindGameObjectsWithTag("BuiltStructure");

        foreach (GameObject structure in structures)
        {
            string id = structure.name.Replace("(Clone)", "").Trim();

            BuiltStructureData builtData = new BuiltStructureData(
                id,
                structure.transform.position,
                structure.transform.rotation
            );

            currentData.builtStructures.Add(builtData);
        }

        // 3. Guarda la información actualizada (incluyendo la lista de destruidos que YA estaba allí)
        GameStateManager.Instance.SaveSceneState(currentSceneName, currentData);
        Debug.Log($"Guardado final: {currentData.builtStructures.Count} estructuras, {currentData.destroyedObjects.Count} objetos destruidos.");
    }

    // Sobrecarga simple del método de guardado para la primera visita (estado vacío)
    private void SaveState(SceneData data)
    {
        GameStateManager.Instance.SaveSceneState(sceneName, data);
    }
}