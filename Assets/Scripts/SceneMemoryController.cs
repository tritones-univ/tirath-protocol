using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMemoryController : MonoBehaviour
{
    // Implementación del patrón Singleton (¡CRUCIAL para el SmartSceneLoader!)
    public static SceneMemoryController Instance;

    private string sceneName;

    // Referencia al Player (para evitar FindGameObjectWithTag repetidos)
    private GameObject playerReference;

    // Usaremos Awake para establecer el Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Opcional: Si quieres que el controlador de memoria persista
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // Destruye el duplicado
            Destroy(gameObject);
        }

        // Buscar el Player y guardar la referencia (solo si no es persistente)
        // Nota: Si el Player es persistente (DontDestroyOnLoad), esta búsqueda es más simple
        playerReference = GameObject.FindGameObjectWithTag("Player");
        if (playerReference == null)
        {
            Debug.LogWarning("SceneMemoryController no encontró un objeto con la etiqueta 'Player' en Awake.");
        }
    }

    void Start()
    {
        // Obtiene el nombre de la escena una vez al inicio
        sceneName = SceneManager.GetActiveScene().name;

        // Si ya se visitó antes, restaurar. De lo contrario, guardar el estado inicial.
        if (GameStateManager.Instance != null && GameStateManager.Instance.HasVisited(sceneName))
        {
            Debug.Log($"Restaurando estado de escena: {sceneName}");
            LoadState();
        }
        else
        {
            // Primera vez que se visita (guardar el estado base)
            Debug.Log($"Guardando estado inicial de escena: {sceneName}");
            SaveState();
        }
    }

    // --- MÉTODOS DE GUARDADO Y CARGA ---

    public void SaveState()
    {
        // Verifica si el gestor de estado existe
        if (GameStateManager.Instance == null) return;

        SceneData data = new SceneData();

        // Usamos la referencia guardada si existe
        if (playerReference != null)
        {
            data.playerPosition = playerReference.transform.position;
            Debug.Log($"Posición del jugador ({playerReference.transform.position}) guardada.");
        }
        else
        {
            // Esto puede ocurrir si el jugador fue destruido
            Debug.LogWarning("No se puede guardar la posición: playerReference es nulo.");
        }

        // Aquí podrías recorrer cofres, enemigos, etc.
        // ...

        GameStateManager.Instance.SaveSceneState(sceneName, data);
    }

    public void LoadState()
    {
        // Verifica si el gestor de estado existe
        if (GameStateManager.Instance == null) return;

        SceneData data = GameStateManager.Instance.GetSceneState(sceneName);

        if (data == null)
        {
            Debug.LogError($"No se pudo cargar el estado de la escena '{sceneName}'. Creando nuevo estado.");
            SaveState(); // Guardar estado base si no se encuentra
            return;
        }

        // Restaurar posición del jugador
        if (playerReference != null)
        {
            playerReference.transform.position = data.playerPosition;
            Debug.Log($"Posición del jugador restaurada a: {data.playerPosition}");
        }
        else
        {
            // Esto es crucial: si el Player se carga dinámicamente o persiste, 
            // asegúrate de que esté en la escena antes de intentar moverlo.
            Debug.LogWarning("No se puede restaurar la posición: playerReference es nulo al cargar.");
        }

        // Restaurar cofres, enemigos, etc.
    }
}