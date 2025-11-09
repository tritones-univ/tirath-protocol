using System.Collections.Generic;
using UnityEngine;

public class CollectorDetector : MonoBehaviour
{
    [SerializeField]
    private PlaceableNeighbor neighbor;
    [SerializeField]
    private LineCollector lineCollector;

    public void DetectCollectables()
    {
        if (!neighbor.Self.Placed) return;
        BoxCollider box = GetComponent<BoxCollider>();
        Vector3 center = transform.TransformPoint(box.center);
        Vector3 halfExtents = box.size * 0.5f;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation);

        foreach (var hit in hits)
        {
            Collectable collectable = hit.GetComponent<Collectable>();
            if (collectable != null)
            {
                lineCollector.collectItem = collectable.item;
                lineCollector.collectable = collectable;
                break;
            }
        }
    }

    public void ClearReferences()
    {
        if (lineCollector != null)
        {
            lineCollector.collectable = null;
            lineCollector.collectItem = null;
        }
    }

}