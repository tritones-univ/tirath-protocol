using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        if (Mouse.current == null || cam == null) return;

        PlaceableObject p = GetComponent<PlaceableObject>();
        if (p != null && p.Placed) return;

        Vector3 mouseWorld = BuildingSystem.GetMouseWorldPositionOnGrid(transform.position.y);
        Vector3 pos = BuildingSystem.current.SnapCoordinatesToGrid(mouseWorld, 0.5f);
        transform.position = pos;
    }
}
