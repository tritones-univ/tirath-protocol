using UnityEngine;

public class LineCollector : MonoBehaviour
{
    public ItemData collectItem;
    public Collectable collectable;

    public int Collect(int amount = 1)
    {
        if (collectable == null) return 0;
        return collectable.Extract(amount);
    }
    private void OnDestroy()
    {
        CollectorDetector detector = GetComponentInChildren<CollectorDetector>();
        if (detector != null)
            detector.ClearReferences();
    }
}
