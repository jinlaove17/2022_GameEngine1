using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public virtual void UseSkill()
    {

    }

    protected IEnumerator InActive(float duration)
    {
        yield return new WaitForSeconds(duration);

        gameObject.SetActive(false);
    }
}
