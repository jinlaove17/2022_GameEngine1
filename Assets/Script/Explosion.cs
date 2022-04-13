using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject[] meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;

    private void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(1f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        foreach (GameObject obj in meshObj)
        {
            obj.SetActive(false);
        }
        effectObj.SetActive(true);
    }
    
}
