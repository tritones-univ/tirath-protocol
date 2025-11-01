using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{
    [Header("Prefabs & UI")]
    public GameObject slotPrefab;
    public Transform slotContainer;
    [Header("Hotbar Items")]
    public List<HotbarItem> hotbarItems;
    private List<HotbarSlotUI> slots = new List<HotbarSlotUI>();

    [Header("Selection")]
    public int selectedIndex = -1;
    void Awake()
    {
        GenerateSlots();
        UpdateSelectionVisual();
        InitializeSelected();
    }

    void GenerateSlots()
    {
        slots.Clear();

        for (int i = 0; i < hotbarItems.Count; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotContainer);
            HotbarSlotUI slotUI = go.GetComponent<HotbarSlotUI>();
            slotUI.SetIcon(hotbarItems[i].icon);
            slots.Add(slotUI);
        }
    }

    void UpdateSelectionVisual()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetSelected(i == selectedIndex);
        }
    }

    void InitializeSelected()
    {
        if (hotbarItems.Count == 0 || selectedIndex == -1) return;
        BuildingSystem.current.InitializeWithObject(hotbarItems[selectedIndex].prefab);
    }

    #region Input Handlers

    public void HandleSelectSlot(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!UIManager.Instance.IsHUDOpen) return;
        int keyIndex = int.Parse(context.control.name);
        int index = (keyIndex == 0) ? 9 : keyIndex - 1;
        if (index >= 0 && index < hotbarItems.Count)
        {
            selectedIndex = index;
            UpdateSelectionVisual();
            InitializeSelected();
        }
    }

    public void HandleMouseScroll(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.IsHUDOpen) return;
        float delta = context.ReadValue<float>();
        if (delta > 0) selectedIndex--;
        else if (delta < 0) selectedIndex++;

        if (selectedIndex < 0) selectedIndex = hotbarItems.Count - 1;
        else if (selectedIndex >= hotbarItems.Count) selectedIndex = 0;

        UpdateSelectionVisual();
        InitializeSelected();
    }

    public void HandleRotate(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (BuildingSystem.current != null && BuildingSystem.current.objectToPlace != null)
        {
            BuildingSystem.current.Rotate();
        }
    }

    public void HandlePlace(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (BuildingSystem.current != null)
        {
            BuildingSystem.current.TryPlaceObject();
        }
    }

    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        selectedIndex = -1;
        UpdateSelectionVisual();
    }

    #endregion
}
