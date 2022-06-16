using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : BaseSkill
{
    private void Awake()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(InActive(2.0f));
    }

    public override void UseSkill()
    {

    }
}
