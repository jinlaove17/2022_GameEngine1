using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill DB", menuName = "Create Skill DB", order = 1)]
public class SkillDB : ScriptableObject
{
    public SkillPrefab[] skillPrefabs = null;
}

[Serializable]
public class SkillPrefab
{
    public string prefabName = null;
    public GameObject prefab = null;
}
