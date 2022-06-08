using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowing : BaseSkill
{
    private void OnEnable()
    {
        StartCoroutine(InActive(5.0f));
    }

    public override void UseSkill()
    {
        GameManager.Instance.player.TransAnimation("FlameThrowing");
    }
}
