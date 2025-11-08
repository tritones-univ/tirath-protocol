using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public static SeedManager Instance { get; private set; }

    public SeedData currentSelectedSeed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SelectSeed(SeedData seed)
    {
        currentSelectedSeed = seed;
    }
}
