using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public float collectionInterval = 1f;
    public int collectionAmount = 1;
    public static LineManager Instance;
    private readonly List<PlaceableNeighbor> collectors = new();
    private void Start()
    {
        Instance = this;
        InvokeRepeating(nameof(ProcessAllCollectors), 0f, collectionInterval);
    }
    public void RegisterCollector(PlaceableNeighbor collector)
    {
        if (!collectors.Contains(collector))
            collectors.Add(collector);
    }

    public void UnregisterCollector(PlaceableNeighbor collector)
    {
        if (collectors.Contains(collector))
            collectors.Remove(collector);
    }
    private void ProcessAllCollectors()
    {
        foreach (var collector in collectors)
        {
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

            if (current.Self.type == PlaceableType.Storage)
            {
                line.Add(current);
                reachedStorage = true;
                break;
            }

            line.Add(current);

            PlaceableNeighbor next = null;
            foreach (var neighbor in current.neighbors)
            {
                if (neighbor == null) continue;
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
        PlaceableNeighbor collector = productionLine[0];
        LineCollector lineCollector = collector.GetComponent<LineCollector>();
        if (lineCollector == null)
        {
            Debug.Log("No existe el line collector");
            return;
        }
        if (lineCollector.collectItem == null)
        {
            Debug.Log("No hay un item asignado, o se acabo el recurso");
            return;
        }
        ;
        InventoryController.Instance.AddItem(lineCollector.collectItem, lineCollector.Collect(collectionAmount));
        UIManager.Instance.inventoryUI.RefreshUI();
    }
}
