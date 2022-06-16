using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : BaseSkill
{
    private void Awake()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(InActive(3.5f));
    }

    public override void UseSkill()
    {

    }
}
