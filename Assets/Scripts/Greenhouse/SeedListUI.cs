using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SeedListUI : MonoBehaviour
{
    public List<SeedData> availableSeeds;
    public Transform seedButtonContainer;
    public Button seedButtonPrefab;

    private Button selectedButton;

    void Start()
    {
        foreach (var seed in availableSeeds)
        {
            Button btn = Instantiate(seedButtonPrefab, seedButtonContainer);

            var seedButtonUI = btn.GetComponent<SeedButtonUI>();
            if (seedButtonUI != null)
            {
                seedButtonUI.quantity.text = "0";
                seedButtonUI.label.text = seed.seedName;
                seedButtonUI.icon.sprite = seed.seedSprite;
            }

            btn.onClick.AddListener(() => OnSeedSelected(seed, btn));
        }
    }

    void OnSeedSelected(SeedData seed, Button btn)
    {
        SeedManager.Instance.SelectSeed(seed);

        // Desresalta el botón anterior
        if (selectedButton != null)
        {
            SetButtonHighlight(selectedButton, false);
        }

        // Resalta el nuevo
        selectedButton = btn;
        SetButtonHighlight(btn, true);
    }

    void SetButtonHighlight(Button btn, bool active)
    {
        Image img = btn.GetComponent<Image>();
        if (img == null) return;

        Color c = img.color;

        if (active)
        {
            c.a = 0.1f;
        }
        else
        {
            c.a = 0f;
        }

        img.color = c;
    }
}
