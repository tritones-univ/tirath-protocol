using UnityEngine;

public class NeighborTrigger : MonoBehaviour
{
    private PlaceableNeighbor parent;
    private BoxCollider box;
    private bool isConnected = false;
    private void Awake()
    {
        parent = GetComponentInParent<PlaceableNeighbor>();
        box = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isConnected || !parent.Self.Placed) return;
        PlaceableNeighbor otherPlaceable = other.GetComponentInParent<PlaceableNeighbor>();
        if (otherPlaceable == null || !otherPlaceable.Self.Placed) return;
        parent.ConnectNeighbor(otherPlaceable, this);
    }

    private void OnTriggerExit(Collider other)
    {
        PlaceableNeighbor otherPlaceable = other.GetComponent<PlaceableNeighbor>();
        if (otherPlaceable != null)
        {
            parent.RemoveNeighbor(otherPlaceable);
            isConnected = false;
        }
    }
    public void SetConnected(bool state)
    {
        isConnected = state;
    }

    public void EnableCollider(bool state)
    {
        box.enabled = state;
    }
}
