using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Entity
{
    private Animator animator = null;
    private List<Material> materials = new List<Material>();

    [HideInInspector]
    public StateMachine<Monster> stateMachine;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    [HideInInspector]
    public Rigidbody rigidBody;

    private void Awake()
    {
        stateMachine = new StateMachine<Monster>(this, Monster_ChaseState.Instance);

        animator = transform.GetComponent<Animator>();
        navMeshAgent = transform.GetComponent<NavMeshAgent>();
        rigidBody = transform.GetComponent<Rigidbody>();

        // 해당 객체가 가지고 있는 모든 메터리얼을 캐싱 해놓는다.
        // 이때, 이름이 중복된다면 하나만 저장하도록 한다.
        SkinnedMeshRenderer[] skinnedMeshRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        List<string> isIncluded = new List<string>();

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            foreach (Material material in skinnedMeshRenderer.materials)
            {
                if (!isIncluded.Contains(material.name))
                {
                    materials.Add(material);
                    isIncluded.Add(material.name);
                }
            }
        }
    }

    private void Update()
    {
        if (IsAlive && !IsHit)
        {
            stateMachine.LogicUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (IsAlive && !IsHit)
        {
            stateMachine.PhysicsUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsAlive)
        {
            // collision.gameObject.tag로 하면 충돌이 일어난 프레임의 최상단을 가져오기 때문에 collider.tag로 해야함!
            if (collision.collider.CompareTag("Floor") || collision.collider.CompareTag("Monster"))
            {
                animator.SetTrigger("Land");
                navMeshAgent.enabled = true;
            }
            else if (collision.collider.CompareTag("Weapon"))
            {
                if (!IsHit)
                {
                    StartCoroutine(Hit());
                }
            }
        }
    }

    private IEnumerator Hit()
    {
        IsHit = true;
        Health -= 50;

        if (IsAlive)
        {
            foreach (Material material in materials)
            {
                material.color = new Color(1.0f, 0.7f, 0.7f, 1.0f);
            }

            animator.SetTrigger("Hit");

            yield return new WaitForSeconds(0.2f);

            foreach (Material material in materials)
            {
                material.color = Color.white;
            }
        }
        else
        {
            navMeshAgent.enabled = false;
            animator.SetTrigger("Die");

            GameManager.Instance.RestMonsterCount -= 1;
            GameManager.Instance.IncreasePlayerExp(100.0f);
        }

        IsHit = false;
    }

    IEnumerator ReserveToDestroyObject()
    {
        yield return new WaitForSeconds(1.0f);

        transform.gameObject.SetActive(false);
        navMeshAgent.enabled = false;
    }
}
