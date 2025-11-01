using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : MonoBehaviour
{
    public GameObject slotGO;
    public Image iconImage;
    public CanvasGroup canvasGroup;

    public HotbarSlotUI(GameObject go)
    {
        slotGO = go;
        iconImage = go.transform.GetChild(0).GetComponent<Image>();
        canvasGroup = go.GetComponent<CanvasGroup>();
    }

    public void SetSelected(bool selected)
    {
        if (canvasGroup != null)
            canvasGroup.alpha = selected ? 1f : 0.7f;
    }

    public void SetIcon(Sprite sprite)
    {
        if (iconImage != null)
            iconImage.sprite = sprite;
    }
}
