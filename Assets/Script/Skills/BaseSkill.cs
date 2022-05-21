using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public bool isUsable = true;

    public string skillInfo = null;
    public int skillLevel = 0;

    public float skillCoolTime = 0.0f;
    public float currentTime = 0.0f;

    public virtual void UseSkill()
    {

    }

    protected IEnumerator InActive(float duration)
    {
        yield return new WaitForSeconds(duration);

        gameObject.SetActive(false);
    }
}
