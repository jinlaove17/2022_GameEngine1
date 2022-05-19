using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    IEnumerator Destroy()
    {
        WaitForSeconds destroyTick = new WaitForSeconds(10.0f);
        yield return destroyTick;
        Destroy(gameObject);
    }
}
