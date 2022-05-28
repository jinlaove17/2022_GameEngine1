using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill DB", menuName = "Create Skill DB", order = 1)]
public class SkillDB : ScriptableObject
{
    public SkillPrefab[] skillPrefabs;
}

[Serializable]
public class SkillPrefab
{
    public string skillName;
    public string skillInfo;
    public float skillCoolTime;

    public GameObject prefab;
}
