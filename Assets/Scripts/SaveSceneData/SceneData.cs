using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuiltStructureData
{
    public string prefabID;

    public Vector3 position;
    public Quaternion rotation;

    public BuiltStructureData(string id, Vector3 pos, Quaternion rot)
    {
        prefabID = id;
        position = pos;
        rotation = rot;
    }
}

[System.Serializable]
public class SceneData
{
    public List<BuiltStructureData> builtStructures = new List<BuiltStructureData>();
    public List<string> destroyedObjects = new List<string>();
    public SceneData() { }
}
