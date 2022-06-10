using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster DB", menuName = "Create Monster DB", order = 0)]
public class MonsterDB : ScriptableObject
{
    public MonsterData[] monsterBundles;
}

[Serializable]
public class MonsterData
{
    public string monsterName;

    public GameObject prefab;
}
