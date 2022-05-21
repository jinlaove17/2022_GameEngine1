using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster DB", menuName = "Create Monster DB", order = 0)]
public class MonsterDB : ScriptableObject
{
    public MonsterPrefab[] monsterPrefabs = null;
}

[Serializable]
public class MonsterPrefab
{
    public string prefabName = null;
    public GameObject prefab = null;
}
