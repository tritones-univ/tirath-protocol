using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlantSlot : MonoBehaviour
{
    public Image plantImage;
    public Button plantButton;
    public Button collectButton;
    public Image borderImage;

    private SeedData currentSeed;
    private bool isPlanted = false;
    private bool isReady = false;

    private float plantedTime;
    private float growEndTime;

    void Start()
    {
        plantButton.onClick.AddListener(Plant);
        collectButton.onClick.AddListener(Collect);
        collectButton.gameObject.SetActive(false);
        plantImage.enabled = false;

    }
    void Update()
    {
        if (isPlanted && !isReady)
        {
            if (Time.time >= growEndTime)
            {
                OnGrowthComplete();
            }
        }
    }

    void Plant()
    {
        if (isPlanted) return;

        SeedData selectedSeed = SeedManager.Instance.currentSelectedSeed;
        if (selectedSeed == null)
        {
            Debug.LogWarning("❌ No hay semilla seleccionada para plantar.");
            return;
        }

        // Aquí puedes verificar inventario antes de plantar
        Debug.Log($"✅ Semilla {currentSeed.seedName} plantada");
        currentSeed = selectedSeed;
        isPlanted = true;

        plantImage.sprite = currentSeed.seedSprite;
        plantImage.enabled = true;
        plantButton.gameObject.SetActive(false);

        plantedTime = Time.time;
        growEndTime = Time.time + currentSeed.growTime;
    }
    void OnGrowthComplete()
    {
        isReady = true;
        borderImage.color = Color.green;
        collectButton.gameObject.SetActive(true);
    }

    void Collect()
    {
        if (!isReady) return;

        Debug.Log($"✅ Cosechada {currentSeed.seedName}");
        isReady = false;
        isPlanted = false;
        plantImage.enabled = false;
        collectButton.gameObject.SetActive(false);
        borderImage.color = Color.white;
        plantButton.gameObject.SetActive(true);
    }
}
