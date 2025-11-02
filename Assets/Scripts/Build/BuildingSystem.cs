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
    private GameObject selected;
    public PlaceableObject objectToPlace;
    private float rotateAngle = 0f;

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    public void TryPlaceObject()
    {
        if (!objectToPlace) return;

        if (CanBePlaced(objectToPlace))
        {
            Collider collider = objectToPlace.GetComponent<Collider>();
            // TODO activar en true si se quiere colisionar
            if (collider != null) collider.enabled = false;

            objectToPlace.Place();
            Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
            TakeArea(start, objectToPlace.Size);

            InitializeWithObject(selected);
        }
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

        Quaternion rotation = Quaternion.Euler(0, rotateAngle, 0);
        GameObject obj = Instantiate(prefab, position, rotation);
        objectToPlace = obj.GetComponent<PlaceableObject>();

        Collider collider = obj.GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

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
    public void Rotate()
    {
        if (objectToPlace == null) return;

        rotateAngle += 90;
        if (rotateAngle >= 360)
            rotateAngle = 0;
        objectToPlace.Rotate(rotateAngle);
    }
    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, whiteTile,
        start.x, start.y,
        start.x + size.x - 1,
        start.y + size.y - 1);
    }
    public void CancelPlacement()
    {
        if (objectToPlace != null && !objectToPlace.Placed)
        {
            Destroy(objectToPlace.gameObject);
            objectToPlace = null;
            selected = null;
        }
    }
}
