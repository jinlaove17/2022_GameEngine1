using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadExplode : BaseSkill
{
    private void Awake()
    {
        skillType = SKILL_TYPE.DeadExplode;
    }

    private void OnEnable()
    {
        StartCoroutine(InActive(2.0f));
    }

    public override void UseSkill()
    {
        GameManager.Instance.player.TransAnimation("DeadExplode");
    }
}
