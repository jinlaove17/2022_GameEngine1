using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    { 
        yield return new WaitForSeconds(1.0f);

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
