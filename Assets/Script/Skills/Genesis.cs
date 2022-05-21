using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genesis : BaseSkill
{
    private void Start()
    {
        StartCoroutine(InActive(1.5f));
    }

    public override void UseSkill()
    {
        GameManager.Instance.player.TransAnimation("DO_SKILL3");
    }
}
