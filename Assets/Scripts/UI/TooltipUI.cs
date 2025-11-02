using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    [SerializeField] private GameObject tooltipGO;
    [SerializeField] private TextMeshProUGUI tooltipText;
    void Awake()
    {
        Instance = this;
        tooltipGO.SetActive(false);
    }

    public void Show(string text, RectTransform slotTransform)
    {
        tooltipText.text = text;
        tooltipGO.SetActive(true);
        Vector3[] corners = new Vector3[4];
        slotTransform.GetWorldCorners(corners);
        Vector3 topCenter = (corners[1] + corners[2]) / 2f;
        tooltipGO.transform.position = topCenter + new Vector3(0, 10, 0);
    }

    public void Hide()
    {
        tooltipGO.SetActive(false);
    }

}
