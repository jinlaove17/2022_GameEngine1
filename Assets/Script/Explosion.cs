using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject fire;
    public GameObject body;

    public GameObject effectObj;

    public Rigidbody rigid;

    private void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        fire.SetActive(false);
        body.SetActive(false);
        effectObj.SetActive(true);
    }
    
}
