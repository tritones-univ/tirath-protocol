using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemImage;
    public TextMeshProUGUI quantityText;
    private InventoryItem currentItem;
    public void SetItem(InventoryItem item)
    {
        currentItem = item;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            itemImage.enabled = true;
            quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
        }
        else
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
            quantityText.text = "";
        }
    }

    public InventoryItem GetItem() => currentItem;
    public GameObject GetGameObject() => gameObject;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
            TooltipUI.Instance.Show(currentItem.data.displayName, transform as RectTransform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Hide();
    }
}
