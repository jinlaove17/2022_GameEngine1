using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Boss : MonoBehaviour
{

    public Transform target;
    public GameObject[] skillObj;
    private Animator animator;
    private NavMeshAgent nav;
    private bool isAttack;
    private int ranAction = 0;
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        if (!isAttack)
        {
            animator.SetBool("IS_CHASE", true);
            nav.SetDestination(target.position);
            if (UnityEngine.Vector3.Distance(transform.position, target.position) < 10.0f)
                StartCoroutine(Think());
        }
    }

    IEnumerator Think()
    {
        animator.SetBool("IS_CHASE", false);
        isAttack = true;
        nav.isStopped = true;
        yield return new WaitForSeconds(2.0f);
        transform.LookAt(target);
        
        ranAction = UnityEngine.Random.Range(0, 3);
        switch (ranAction)
        {
            case 0:
                ThrowPoison();
                break;
            case 1:
                SpellMeteor();
                break;
            case 2:
                Explosion();
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
        StartCoroutine(Poison());
    }

    IEnumerator Poison()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 3; i++)
        {
            Vector3 skillPos = target.transform.position;

            skillPos += target.transform.right * 3 * (i - 1);

            GameObject instantMeteor = Instantiate(skillObj[0], skillPos, Quaternion.identity);
            yield return new WaitForSeconds(0.4f);
        }
    }

    void SpellMeteor()
    {
        animator.SetTrigger("DO_ATTACK2");
        StartCoroutine(Meteor());
    }

    IEnumerator Meteor()
    {
        for (int i = 0; i <20; i++)
        { 
            yield return new WaitForSeconds(0.1f);
            var posX = UnityEngine.Random.Range(-20.0f, 20.0f);
            var posY = UnityEngine.Random.Range(-30f, -20f);
            var posZ = UnityEngine.Random.Range(20f, 30f);
            
            Vector3 skillPos = transform.position;
            skillPos += transform.right * posX;
            skillPos += transform.forward * posY;
            skillPos += transform.up * posZ;
            
            Vector3 skillVec = transform.forward;
            skillVec.y = -0.8f;
            
            GameObject instantMeteor = Instantiate(skillObj[1], skillPos, transform.rotation);
            Rigidbody rigidMeteor = instantMeteor.GetComponent<Rigidbody>();
            rigidMeteor.AddForce(skillVec * 20, ForceMode.Impulse);
        }
    }
    
    void Explosion()
    {
        Vector3 skillPos = target.position;
        skillPos.y = 1f;
        GameObject instantMeteor = Instantiate(skillObj[2], skillPos, target.rotation);
        animator.SetTrigger("DO_ATTACK3");
    }

}
