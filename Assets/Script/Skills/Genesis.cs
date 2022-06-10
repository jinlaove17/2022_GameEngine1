using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genesis : BaseSkill
{
    private void Awake()
    {
        skillType = SKILL_TYPE.Genesis;
    }

    private void OnEnable()
    {
        StartCoroutine(InActive(1.5f));
    }

    public override void UseSkill()
    {
        GameManager.Instance.player.TransAnimation("Genesis");
    }
}
