using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDB", menuName = "ScriptableObjects/InventoryDB")]
public class InventoryDB : ScriptableObject
{
  public static InventoryDB Instance;

  [SerializeField] public List<ItemData> items;
  private Dictionary<string, ItemData> lookup;

  void OnEnable()
  {
    Instance = this;
    if (lookup == null)
      lookup = new Dictionary<string, ItemData>();
    if (items != null)
      lookup = items.ToDictionary(i => i.id, i => i);
  }

  public ItemData Get(string id)
  {
    if (lookup == null || lookup.Count == 0)
      OnEnable();
    if (lookup.TryGetValue(id, out var data))
      return data;
    Debug.LogWarning("No se encontro el item");
    return null;
  }
}
