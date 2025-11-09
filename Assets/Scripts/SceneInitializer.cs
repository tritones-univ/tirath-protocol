using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private BuildingSystem buildingSystem;
    [SerializeField] private GameObject preplacedContainer;
    private void Start()
    {
        if (buildingSystem == null)
            buildingSystem = BuildingSystem.current;

        if (preplacedContainer == null)
        {
            Debug.LogWarning("Preplaced container no asignado");
            return;
        }

        foreach (Transform child in preplacedContainer.transform)
        {
            PlaceableObject obj = child.GetComponent<PlaceableObject>();
            if (obj == null) continue;
            obj.Placed = true;
            Vector3Int start = buildingSystem.gridLayout.WorldToCell(obj.GetStartPosition());
            buildingSystem.TakeArea(start, obj.Size);
            PlaceableNeighbor neighbor = obj.GetComponent<PlaceableNeighbor>();
            if (neighbor != null)
                neighbor.CheckForNeighborsOnPlace();
            CollectorDetector detector = obj.GetComponentInChildren<CollectorDetector>();
            if (detector != null)
                detector.DetectCollectables();
            if (obj.type == PlaceableType.Collector)
            {
                LineManager.Instance.RegisterCollector(obj.GetComponent<PlaceableNeighbor>());
            }
        }
    }
}
