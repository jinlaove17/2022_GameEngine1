using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningArrow : BaseSkill
{
    private void Awake()
    {
        skillType = SKILL_TYPE.LightningArrow;
    }

    private void OnEnable()
    {
        StartCoroutine(InActive(5.0f));
    }

    public override void UseSkill()
    {
        GameManager.Instance.player.TransAnimation("LightningArrow");
    }
}
