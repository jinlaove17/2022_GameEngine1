using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SKILL_TYPE
{
    ThrowFire,
    DeadExplode,
    Genesis,
    EnergyDischarge,
    LightningArrow,
    GravityField,
    FlameThrowing,

    None = int.MinValue
};

[CreateAssetMenu(fileName = "Skill DB", menuName = "Create Skill DB", order = 1)]
public class SkillDB : ScriptableObject
{
    public SkillData[] skillBundles;
}

[Serializable]
public class SkillData
{
    public string skillName;
    public string skillInfo;
    public float skillCoolTime;
    public float skillDamage;
    public float hitDuration;
    public Sprite skillIcon;

    public GameObject prefab;
}
