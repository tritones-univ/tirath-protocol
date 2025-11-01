using UnityEngine;

[CreateAssetMenu(fileName = "HotbarItem", menuName = "ScriptableObjects/HotbarItem")]
public class HotbarItem : ScriptableObject
{
  public Sprite icon;
  public GameObject prefab;
}