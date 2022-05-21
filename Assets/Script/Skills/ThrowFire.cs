using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFire : BaseSkill
{
    private void Start()
    {
        StartCoroutine(InActive(3.0f));
    }

    public override void UseSkill()
    {
        GameManager.Instance.player.TransAnimation("DO_SKILL2");
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
