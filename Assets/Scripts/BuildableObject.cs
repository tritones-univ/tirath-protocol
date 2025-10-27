using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    public BuildType type;
    private Renderer rend;
    public Vector2Int gridPos;
    public bool isConnected = false;

    public void UpdateState(bool connected, Material correctMat, Material errorMat)
    {
        isConnected = connected;
        SetMaterial(connected ? correctMat : errorMat);
    }

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetMaterial(Material mat)
    {
        if (rend != null) rend.material = mat;
    }

    public void SetType(BuildType t)
    {
        type = t;
    }
}
