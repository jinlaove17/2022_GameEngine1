using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDischarge : BaseSkill
{
    private void Awake()
    {
        skillType = SKILL_TYPE.EnergyDischarge;
    }

    private void OnEnable()
    {
        StartCoroutine(InActive(4.0f));
    }

    public override void UseSkill()
    {
        GameManager.Instance.player.TransAnimation("EnergyDischarge");
    }
}
