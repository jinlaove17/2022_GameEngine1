using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Animator animator = null;
    
    private bool isDie;
    
    void Start()
    {
        if (animator == null)
        {
            animator = transform.GetComponent<Animator>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (animator != null)
        {
            animator.SetBool("Land", true);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            if (!isDie)
            {
                animator.SetTrigger("Do_Die");
                isDie = true;
            }
        }
    }
}
