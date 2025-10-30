using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;
    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    private GameObject selected;
    private PlaceableObject objectToPlace;
    private BuildControls controls;

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
        controls = new BuildControls();

        controls.BuildActions.Build1.performed += ctx => InitializeWithObject(prefab1);
        controls.BuildActions.Build2.performed += ctx => InitializeWithObject(prefab2);
        controls.BuildActions.Build3.performed += ctx => InitializeWithObject(prefab3);
        controls.BuildActions.Rotate.performed += ctx => { if (objectToPlace) objectToPlace.Rotate(); };
        controls.BuildActions.Place.performed += ctx => TryPlaceObject();
        controls.BuildActions.Cancel.performed += ctx =>
        {
            if (objectToPlace)
            {
                Destroy(objectToPlace.gameObject);
            }
            objectToPlace = null;

        };
    }
    private void OnEnable() => controls.BuildActions.Enable();
    private void OnDisable() => controls.BuildActions.Disable();

    private void TryPlaceObject()
    {
        if (!objectToPlace) return;

        if (CanBePlaced(objectToPlace))
        {
            objectToPlace.Place();
            Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
            TakeArea(start, objectToPlace.Size);
            InitializeWithObject(selected);
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        if (Camera.main == null) return Vector3.zero;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        return Physics.Raycast(ray, out RaycastHit hit) ? hit.point : Vector3.zero;
    }
    public static Vector3 GetMouseWorldPositionOnGrid(float gridY)
    {
        if (Camera.main == null || Mouse.current == null) return Vector3.zero;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Plane gridPlane = new Plane(Vector3.up, new Vector3(0, gridY, 0));
        if (gridPlane.Raycast(ray, out float enter))
            return ray.GetPoint(enter);
        return Vector3.zero;
    }
    public Vector3 SnapCoordinatesToGrid(Vector3 position, float yOverride = float.NaN)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        Vector3 snapped = grid.GetCellCenterWorld(cellPos);

        if (!float.IsNaN(yOverride))
            snapped.y = yOverride;

        return snapped;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x + area.size.y + area.size.z];
        int counter = 0;
        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;

    }
    public void InitializeWithObject(GameObject prefab)
    {
        if (objectToPlace != null && !objectToPlace.Placed)
        {
            Destroy(objectToPlace.gameObject);
            objectToPlace = null;
        }
        selected = prefab;
        Vector3 position = SnapCoordinatesToGrid(Vector3.zero, 0.5f);
        position.y = 0.5f;
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }
    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }
        return true;
    }
    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, whiteTile,
        start.x, start.y,
        start.x + size.x - 1,
        start.y + size.y - 1);
    }
}
