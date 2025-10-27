using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BuildType { None, Collector, Conveyor, Storage }

public class GridManager : MonoBehaviour
{
    public float cellSize = 1f;
    public int width = 20;
    public int height = 20;

    public GameObject collectorPrefab;
    public GameObject conveyorPrefab;
    public GameObject storagePrefab;

    public Renderer playerRenderer;
    public Material playerCollectorMat;
    public Material playerConveyorMat;
    public Material playerStorageMat;

    public Material validMat;   // Verde
    public Material invalidMat; // Rojo
    public Material correctMat; // Azul
    public Material errorMat;   // Amarillo

    private Dictionary<Vector2Int, BuildableObject> grid = new Dictionary<Vector2Int, BuildableObject>();

    private BuildType selectedBuild = BuildType.None;
    private BuildableObject previewObject;

    void Update()
    {
        HandleSelection();
        HandlePreview();
        HandlePlacement();
    }

    // Cambia las opcinonse de construccion
    void HandleSelection()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectBuild(BuildType.Collector);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectBuild(BuildType.Conveyor);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectBuild(BuildType.Storage);
    }

    void SelectBuild(BuildType type)
    {
        selectedBuild = type;

        if (playerRenderer != null)
        {
            switch (type)
            {
                case BuildType.Collector: playerRenderer.material = playerCollectorMat; break;
                case BuildType.Conveyor: playerRenderer.material = playerConveyorMat; break;
                case BuildType.Storage: playerRenderer.material = playerStorageMat; break;
            }
        }

        if (previewObject != null) Destroy(previewObject.gameObject);

        GameObject prefab = GetPrefab(type);
        if (prefab != null)
        {
            GameObject obj = Instantiate(prefab);
            previewObject = obj.GetComponent<BuildableObject>();
            previewObject.SetMaterial(validMat);
        }
    }

    // Selecciona el prefab para colocar en el grid
    GameObject GetPrefab(BuildType type)
    {
        switch (type)
        {
            case BuildType.Collector: return collectorPrefab;
            case BuildType.Conveyor: return conveyorPrefab;
            case BuildType.Storage: return storagePrefab;
            default: return null;
        }
    }

    // Previsualizacion de la construccion
    void HandlePreview()
    {
        if (previewObject == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 pos = hit.point;
            Vector2Int cell = new Vector2Int(
                Mathf.RoundToInt(pos.x / cellSize),
                Mathf.RoundToInt(pos.z / cellSize)
            );

            previewObject.transform.position = new Vector3(cell.x * cellSize, 0.5f, cell.y * cellSize);

            if (!grid.ContainsKey(cell))
                previewObject.SetMaterial(validMat);
            else
                previewObject.SetMaterial(invalidMat);
        }
    }

    // coloca un elemento
    void HandlePlacement()
    {
        if (previewObject == null) return;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2Int cell = new Vector2Int(
                Mathf.RoundToInt(previewObject.transform.position.x / cellSize),
                Mathf.RoundToInt(previewObject.transform.position.z / cellSize)
            );

            if (!grid.ContainsKey(cell))
            {
                GameObject placedGO = Instantiate(GetPrefab(selectedBuild));
                placedGO.transform.position = previewObject.transform.position;

                BuildableObject placedObject = placedGO.GetComponent<BuildableObject>();
                placedObject.SetType(selectedBuild);
                placedObject.gridPos = cell;
                placedObject.SetMaterial(correctMat);

                grid[cell] = placedObject;

                SelectBuild(selectedBuild);
                CheckConnections();
            }
        }
    }

    void CheckConnections()
    {
        foreach (var obj in grid.Values)
            obj.UpdateState(false, correctMat, errorMat);

        foreach (var kvp in grid)
        {
            BuildableObject start = kvp.Value;
            if (start.type != BuildType.Collector) continue;

            Queue<BuildableObject> queue = new Queue<BuildableObject>();
            HashSet<BuildableObject> visited = new HashSet<BuildableObject>();
            queue.Enqueue(start);
            visited.Add(start);

            bool foundStorage = false;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                Vector2Int pos = current.gridPos;

                // Revisar vecinos cardinales
                Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
                foreach (var d in dirs)
                {
                    Vector2Int neighborPos = pos + d;
                    if (grid.TryGetValue(neighborPos, out BuildableObject neighbor))
                    {
                        if (!visited.Contains(neighbor))
                        {
                            if (neighbor.type == BuildType.Conveyor || neighbor.type == BuildType.Storage)
                            {
                                queue.Enqueue(neighbor);
                                visited.Add(neighbor);

                                if (neighbor.type == BuildType.Storage)
                                    foundStorage = true;
                            }
                        }
                    }
                }
            }

            foreach (var obj in visited)
                obj.UpdateState(foundStorage, correctMat, errorMat);
        }
    }
}
