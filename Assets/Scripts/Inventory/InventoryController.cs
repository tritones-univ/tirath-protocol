using System.Collections.Generic;
using UnityEngine;
public class InventoryItem
{
    public ItemData data;
    public int quantity;
}
public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;
    private Dictionary<string, InventoryItem> items;
    void Awake()
    {
        if (Instance == null) Instance = this;
        if (items == null) items = new Dictionary<string, InventoryItem>();

        else Destroy(gameObject);
    }

    public void AddItem(ItemData item, int quantity)
    {
        if (items == null || quantity <= 0) return;
        if (!items.ContainsKey(item.id))
        {
            InventoryItem inventoryItem = new InventoryItem
            {
                data = item,
                quantity = quantity
            };
            items.Add(item.id, inventoryItem);
        }
        else
        {
            InventoryItem inventoryItem = items.GetValueOrDefault(item.id);
            inventoryItem.quantity += quantity;
        }
    }
    public void ReduceItem(ItemData item, int quantity)
    {
        if (items == null || quantity <= 0) return;
        if (!items.ContainsKey(item.id)) return;
        InventoryItem inventoryItem = items.GetValueOrDefault(item.id);
        if (inventoryItem.quantity < quantity) return;
        inventoryItem.quantity -= quantity;
        if (inventoryItem.quantity <= 0)
            items.Remove(item.id);
    }
    public bool CanReduceItem(ItemData item, int quantity)
    {
        if (items == null || quantity <= 0) return false;
        if (!items.ContainsKey(item.id)) return false;
        InventoryItem inventoryItem = items.GetValueOrDefault(item.id);
        if (inventoryItem.quantity < quantity) return false;
        return true;
    }

    public List<InventoryItem> GetAllItems()
    {
        return new List<InventoryItem>(items.Values);
    }
    public InventoryItem GetItem(string id) => items.GetValueOrDefault(id);

    public void SaveData()
    {
        // TODO, retornar la lista, el gestor global de guardado, este deberia tener referencia a todos los elementos de donde se pueda guardar
        List<InventoryItemDTO> dtoItems = new List<InventoryItemDTO>();
        foreach (InventoryItem invitem in items.Values)
        {
            dtoItems.Add(new InventoryItemDTO(invitem.data.id, invitem.quantity));
        }
    }

}


[System.Serializable]
public class InventoryItemDTO
{
    public string id;
    public int quantity;

    public InventoryItemDTO(string id, int quantity)
    {
        this.id = id;
        this.quantity = quantity;
    }
}
