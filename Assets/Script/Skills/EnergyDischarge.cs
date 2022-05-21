using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDischarge : BaseSkill
{
    private void Start()
    {
        StartCoroutine(InActive(4.0f));
    }

    public override void UseSkill()
    {
        GameManager.Instance.player.TransAnimation("DO_SKILL4");
    }
}
