using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerCollector : MonoBehaviour
{
    private InventoryController inventory => InventoryController.Instance;

    void OnTriggerEnter(Collider other)
    {
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();
        if (itemWorld != null && inventory != null)
        {
            inventory.AddItem(itemWorld.data, itemWorld.quantity);
            Destroy(other.gameObject);
        }
    }
}