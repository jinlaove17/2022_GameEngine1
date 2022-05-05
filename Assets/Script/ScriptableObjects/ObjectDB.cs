using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object DB", menuName = "Create Object DB", order = 0)]
public class ObjectDB : ScriptableObject
{
    public ObjectPrefab[] objectPrefabs = null;
}

[Serializable]
public class ObjectPrefab
{
    public string prefabName;
    public GameObject prefab;
}
