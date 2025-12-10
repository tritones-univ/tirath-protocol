using System.Collections.Generic;
using UnityEngine;

// *************************************************************************
// NOTA IMPORTANTE: 
// Las clases PlaceableNeighbor, PlaceableType, InventoryController, 
// UIManager, LineCollector y ItemData deben estar definidas en tu proyecto 
// para que este código compile correctamente.
// *************************************************************************

public class LineManager : MonoBehaviour
{
    [Header("Configuración")]
    public float collectionInterval = 1f;
    public int collectionAmount = 1;

    // Patrón Singleton para acceso global
    public static LineManager Instance;

    // Lista de componentes que actúan como "collectors" o inicio de línea
    // Usamos 'readonly' para que solo se pueda inicializar aquí
    private readonly List<PlaceableNeighbor> collectors = new List<PlaceableNeighbor>();

    void Awake()
    {
        // Implementación del Singleton (asegura que solo exista uno)
        if (Instance == null)
        {
            Instance = this;
            // Opcional: Si quieres que persista entre escenas, descomenta la siguiente línea
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Inicia el proceso repetitivo de recolección
        InvokeRepeating(nameof(ProcessAllCollectors), 0f, collectionInterval);
    }

    public void RegisterCollector(PlaceableNeighbor collector)
    {
        if (collector != null && !collectors.Contains(collector))
        {
            collectors.Add(collector);
        }
    }

    public void UnregisterCollector(PlaceableNeighbor collector)
    {
        if (collector != null && collectors.Contains(collector))
        {
            collectors.Remove(collector);
        }
    }

    private void ProcessAllCollectors()
    {
        // Usamos una copia de la lista para iterar. Esto previene errores 
        // si un objeto es destruido o se registra/desregistra durante el bucle.
        List<PlaceableNeighbor> currentCollectors = new List<PlaceableNeighbor>(collectors);

        foreach (var collector in currentCollectors)
        {
            // ** VERIFICACIÓN DE NULIDAD CLAVE 1 **
            // Asegura que el objeto en la lista no haya sido destruido
            if (collector == null)
            {
                // Si encuentras un nulo, es una buena idea eliminarlo de la lista original
                collectors.Remove(collector);
                continue;
            }

            // ** VERIFICACIÓN DE NULIDAD CLAVE 2 **
            // Asegura que la propiedad 'Self' (que probablemente es el componente principal Placeable) no sea nula.
            if (collector.Self == null)
            {
                Debug.LogError($"El PlaceableNeighbor en el GameObject '{collector.gameObject.name}' no tiene asignado su componente 'Self'.");
                continue;
            }

            // Aquí estaba tu línea 30:
            if (!collector.Self.Placed) continue;

            List<PlaceableNeighbor> productionLine = GetProductionLine(collector);
            if (productionLine != null && productionLine.Count > 0)
            {
                CollectResources(productionLine);
            }
        }
    }

    private List<PlaceableNeighbor> GetProductionLine(PlaceableNeighbor startCollector)
    {
        List<PlaceableNeighbor> line = new List<PlaceableNeighbor>();
        HashSet<PlaceableNeighbor> visited = new HashSet<PlaceableNeighbor>();
        PlaceableNeighbor current = startCollector;

        bool reachedStorage = false;

        while (current != null && !visited.Contains(current))
        {
            visited.Add(current);

            // ** VERIFICACIÓN DE NULIDAD ADICIONAL **
            if (current.Self == null) break;

            if (current.Self.type == PlaceableType.Storage)
            {
                line.Add(current);
                reachedStorage = true;
                break;
            }

            line.Add(current);

            PlaceableNeighbor next = null;

            // Asumiendo que 'current.neighbors' es una colección de PlaceableNeighbor
            foreach (var neighbor in current.neighbors)
            {
                if (neighbor == null) continue;
                // ** VERIFICACIÓN DE NULIDAD ADICIONAL **
                if (neighbor.Self == null) continue;

                if (visited.Contains(neighbor)) continue;
                if (neighbor.Self.type == PlaceableType.Conveyor || neighbor.Self.type == PlaceableType.Storage)
                {
                    next = neighbor;
                    break;
                }
            }

            current = next;
        }

        if (!reachedStorage)
            return null;

        return line;
    }

    private void CollectResources(List<PlaceableNeighbor> productionLine)
    {
        // Asumiendo que productionLine[0] nunca será null aquí si GetProductionLine tuvo éxito.
        PlaceableNeighbor collector = productionLine[0];

        // ** VERIFICACIÓN DE NULIDAD 3 **
        LineCollector lineCollector = collector.GetComponent<LineCollector>();
        if (lineCollector == null)
        {
            Debug.LogError($"No se encontró el componente LineCollector en el GameObject: {collector.gameObject.name}");
            return;
        }

        if (lineCollector.collectItem == null)
        {
            Debug.Log("No hay un item asignado, o se acabo el recurso");
            return;
        }

        // Se asume que InventoryController.Instance y UIManager.Instance existen
        // (y que también usan el patrón Singleton).
        InventoryController.Instance.AddItem(lineCollector.collectItem, lineCollector.Collect(collectionAmount));
        UIManager.Instance.inventoryUI.RefreshUI();
    }
}