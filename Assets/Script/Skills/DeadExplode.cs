using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadExplode : BaseSkill
{
    private void Start()
    {
        StartCoroutine(InActive(2.0f));
    }

    public override void UseSkill()
    {
        GameManager.Instance.player.TransAnimation("DO_SKILL1");
    }
}
