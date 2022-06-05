using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine;

public class Boss : MonoBehaviour
{

    public Transform target;
    private Animator animator;
    private NavMeshAgent nav;
    private bool isAttack;
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        if (!isAttack)
        {
            nav.SetDestination(target.position);
            if (UnityEngine.Vector3.Distance(transform.position, target.position) < 10.0f)
                StartCoroutine(Think());
        }
    }

    IEnumerator Think()
    {
        isAttack = true;
        nav.isStopped = true;
        transform.LookAt(target);
        yield return new WaitForSeconds(0.1f);
    
        int ranAction = Random.Range(0, 2);
        switch (ranAction)
        {
            case 0:
                ThrowPoison();
                break;
            case 1:
                SpellMeteor();
                break;
        }
    }

    void AttackDisable()
    {
        isAttack = false;
        nav.isStopped = false;
    }
    
    void ThrowPoison()
    {
        animator.SetTrigger("DO_ATTACK1");
    }

    void SpellMeteor()
    {
        animator.SetTrigger("DO_ATTACK2");
    }

}
