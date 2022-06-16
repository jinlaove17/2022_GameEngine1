using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Entity
{
    public Animator animator;
    private List<Material> materials = new List<Material>();

    [HideInInspector]
    public StateMachine<Monster> stateMachine;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    [HideInInspector]
    public Rigidbody rigidBody;

    private void Awake()
    {
        stateMachine = new StateMachine<Monster>(this);

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
        stateMachine.PhysicsUpdate();
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
                stateMachine.InitState(Monster_ChaseState.Instance);
            }
            else if (collision.collider.CompareTag("Weapon"))
            {
                if (!IsHit)
                {
                    StartCoroutine(Hit(collision.gameObject.GetComponent<BaseSkill>().SkillType));
                }
            }
        }
    }

    private void DamageToPlayer(AnimationEvent animationEvent)
    {
        Player player = GameManager.Instance.player;

        if (player.IsAlive && !player.IsHit && navMeshAgent.enabled)
        {
            if (navMeshAgent.remainingDistance < 1.0f)
            {
                // 공격 애니메이션이 끝날 때쯤 플레이어에게 피해를 주어야 하기 때문에, 애니메이션 이벤트를 활용한다.
                StartCoroutine(player.DecreaseHealth(animationEvent.floatParameter));

                // 화면 전체에 블러드 이펙트 애니메이션을 활성화한다.
                GameManager.Instance.systemUI.ShowBloodEffect();
            }
        }
    }

    private IEnumerator Hit(SKILL_TYPE skillType)
    {
        SkillData skill = SkillManager.Instance.skillDB.skillBundles[(int)skillType];
        int skillLevel = SkillManager.Instance.GetSkillLevel(skillType);

        IsHit = true;
        Health -= skillLevel * skill.skillDamage;

        foreach (Material material in materials)
        {
            material.color = new Color(1.0f, 0.8f, 0.8f, 1.0f);
        }

        if (IsAlive)
        {
            animator.SetTrigger("Hit");

            SoundManager.Instance.PlaySFX("ZombiePain");
        }
        else
        {
            navMeshAgent.enabled = false;

            animator.SetTrigger("Die");

            GameManager.Instance.RestMonsterCount -= 1;
            GameManager.Instance.IncreasePlayerExp(100.0f);

            SoundManager.Instance.PlaySFX("ZombieDeath");
        }

        yield return new WaitForSeconds(skill.hitDuration);

        foreach (Material material in materials)
        {
            material.color = Color.white;
        }

        IsHit = false;
    }

    IEnumerator ReserveToDestroyObject()
    {
        yield return new WaitForSeconds(1.0f);

        transform.gameObject.SetActive(false);
    }
}
