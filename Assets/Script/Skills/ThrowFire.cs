using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFire : BaseSkill
{
    private void OnEnable()
    {
        StartCoroutine(InActive(3.0f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }

    public override void UseSkill()
    {
        GameManager.Instance.player.TransAnimation("DO_SKILL2");
    }
}
