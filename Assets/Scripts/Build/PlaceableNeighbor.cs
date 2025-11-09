using System.Collections.Generic;
using UnityEngine;

public class PlaceableNeighbor : MonoBehaviour
{
    public PlaceableObject Self;
    private NeighborTrigger[] triggers;
    public PlaceableNeighbor[] neighbors;
    public int maxNeighbors = 2;
    private void Awake()
    {
        neighbors = new PlaceableNeighbor[maxNeighbors];
        triggers = GetComponentsInChildren<NeighborTrigger>();
    }
    public virtual void ConnectNeighbor(PlaceableNeighbor neighbor, NeighborTrigger trigger)
    {
        if (neighbor == null || neighbor == this) return;
        if (IsInvalidConnection(neighbor) || neighbor.GetNeighborCount() >= neighbor.maxNeighbors) return;
        if (GetNeighborCount() >= maxNeighbors) return;

        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] == null)
            {
                neighbors[i] = neighbor;
                trigger.SetConnected(true);
                neighbor.AddNeighbor(this);
                break;
            }
        }
    }
    private void AddNeighbor(PlaceableNeighbor neighbor)
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] == null)
            {
                neighbors[i] = neighbor;
                break;
            }
        }
    }
    protected virtual bool IsInvalidConnection(PlaceableNeighbor neighbor)
    {
        if ((Self.type == PlaceableType.Collector && neighbor.Self.type == PlaceableType.Collector) ||
            (Self.type == PlaceableType.Storage && neighbor.Self.type == PlaceableType.Storage))
            return true;

        if (GetNeighborCount() >= maxNeighbors) return true;

        return false;
    }
    public int GetNeighborCount()
    {
        int count = 0;
        foreach (var n in neighbors)
            if (n != null) count++;
        return count;
    }

    public void RemoveNeighbor(PlaceableNeighbor neighbor)
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] == neighbor) neighbors[i] = null;
        }
    }

    public void Disconnect()
    {

        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] != null)
            {
                neighbors[i].RemoveNeighbor(this);
                neighbors[i] = null;
            }
        }
    }
    private void OnDestroy()
    {
        Disconnect();
    }
    public void CheckForNeighborsOnPlace()
    {
        HashSet<PlaceableNeighbor> processed = new HashSet<PlaceableNeighbor>();

        foreach (var trigger in triggers)
        {
            Vector3 center = trigger.transform.TransformPoint(trigger.GetComponent<BoxCollider>().center);
            Vector3 halfExtents = Vector3.Scale(trigger.GetComponent<BoxCollider>().size * 0.5f, trigger.transform.lossyScale);


            Collider[] hits = Physics.OverlapBox(center, halfExtents, trigger.transform.rotation);
            foreach (var hit in hits)
            {
                PlaceableNeighbor other = hit.GetComponentInParent<PlaceableNeighbor>();
                if (other != null && other != this && other.Self.Placed && !processed.Contains(other))
                {

                    ConnectNeighbor(other, trigger);
                    processed.Add(other);
                }
            }
        }
    }
}
