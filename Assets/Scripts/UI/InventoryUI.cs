using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform slotContainer;
    public GameObject slotPrefab;
    private InventoryController inventory => InventoryController.Instance;
    private List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();

    public int columns = 6;
    public int rows = 3;

    public void RefreshUI()
    {
        if (inventory == null)
        {
            Debug.LogWarning("InventoryController aún no está listo");
            return;
        }

        foreach (var slot in slotUIs)
            Destroy(slot.GetGameObject());
        slotUIs.Clear();

        int totalSlots = columns * rows;
        List<InventoryItem> allItems = inventory.GetAllItems();

        for (int i = 0; i < totalSlots; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);
            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUIs.Add(slotUI);

            if (i < allItems.Count)
                slotUI.SetItem(allItems[i]);
            else
                slotUI.SetItem(null);
        }
    }
}