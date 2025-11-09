using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipPanel : MonoBehaviour
{
    [System.Serializable]
    public class ShipSlot
    {
        public GameObject slotObject;
        public ItemData requieredItem;
        [HideInInspector] public Image itemImage;
        [HideInInspector] public Image borderSlot;
        [HideInInspector] public Button placeButton;
        [HideInInspector] public bool isCompleted = false;
    }
    public ShipSlot[] slots;
    public Button launchButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        launchButton.gameObject.SetActive(false);
        foreach (var slot in slots)
        {
            slot.borderSlot = slot.slotObject.transform.Find("borde").GetComponent<Image>();
            slot.itemImage = slot.slotObject.transform.Find("ItemImage").GetComponent<Image>();
            slot.placeButton = slot.slotObject.transform.Find("PlaceButton").GetComponent<Button>();


            SetImageOpacity(slot.itemImage, 0.2f);
            slot.itemImage.sprite = slot.requieredItem.icon;
            slot.borderSlot.color = Color.white;

            slot.placeButton.onClick.AddListener(() => TryPlaceItem(slot));
        }
        CheckAllSlots();
    }
    void OnEnable()
    {
        ResetVisuals();
    }

    void TryPlaceItem(ShipSlot slot)
    {
        if (InventoryController.Instance.CanReduceItem(slot.requieredItem, 1))
        {
            slot.isCompleted = true;
            slot.borderSlot.color = Color.green;
            SetImageOpacity(slot.itemImage, 1f);
            slot.placeButton.gameObject.SetActive(false);
            InventoryController.Instance.ReduceItem(slot.requieredItem, 1);
        }
        else
        {
            slot.isCompleted = false;
            slot.borderSlot.color = Color.red;
        }
        CheckAllSlots();
    }
    void CheckAllSlots()
    {
        bool allCompleted = true;
        foreach (var slot in slots)
        {
            if (!slot.isCompleted)
            {
                allCompleted = false;
                break;
            }
        }
        launchButton.gameObject.SetActive(allCompleted);
    }
    public void ResetVisuals()
    {
        foreach (var slot in slots)
        {
            if (slot.borderSlot != null && !slot.isCompleted)
            {
                slot.borderSlot.color = Color.white;
            }
        }
    }
    void SetImageOpacity(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
    public void LaunchShip()
    {
        SceneManager.LoadScene("GodEndScene");
    }
}
