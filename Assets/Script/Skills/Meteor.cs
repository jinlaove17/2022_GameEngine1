using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : BaseSkill
{
    private void Awake()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(InActive(3.0f));
    }

    public override void UseSkill()
    {

    }
}