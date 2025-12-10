using System.Collections.Generic;
using UnityEngine;
public class InventoryItem
{
    public ItemData data;
    public int quantity;
}
public class InventoryController : MonoBehaviour
{
    // Usa un patrón Singleton estático para acceder desde cualquier lugar
    public static InventoryController Instance;

    // Tu diccionario privado para almacenar los ítems
    private Dictionary<string, InventoryItem> items;

    void Awake()
    {
        // 1. Implementación del Singleton
        if (Instance == null)
        {
            // Si no hay otra instancia, esta es la instancia única
            Instance = this;

            // ¡Paso clave! Evita que este GameObject sea destruido al cargar una nueva escena.
            DontDestroyOnLoad(gameObject);

            // Inicializa el diccionario si es la primera vez que se crea
            if (items == null)
            {
                items = new Dictionary<string, InventoryItem>();
            }
        }
        else
        {
            // Si ya existe una instancia, destruye este nuevo objeto duplicado
            Destroy(gameObject);
        }
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
