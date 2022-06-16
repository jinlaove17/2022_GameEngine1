using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Boss : Entity
{
    public Animator animator;
    private List<Material> materials = new List<Material>();

    [HideInInspector]
    public StateMachine<Boss> stateMachine;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    [HideInInspector]
    public Rigidbody rigidBody;

    public GameObject[] skillObj;

    public bool isAttackFinished;
    public float recentTransition = 0.0f;
    private int ranAction = 0;

    void Awake()
    {
        stateMachine = new StateMachine<Boss>(this);

        animator = transform.GetComponent<Animator>();
        navMeshAgent = transform.GetComponent<NavMeshAgent>();
        rigidBody = transform.GetComponent<Rigidbody>();

        // �ش� ��ü�� ������ �ִ� ��� ���͸����� ĳ�� �س��´�.
        // �̶�, �̸��� �ߺ��ȴٸ� �ϳ��� �����ϵ��� �Ѵ�.
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

    void Update()
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
            // collision.gameObject.tag�� �ϸ� �浹�� �Ͼ �������� �ֻ���� �������� ������ collider.tag�� �ؾ���!
            if (collision.collider.CompareTag("Floor") || collision.collider.CompareTag("Monster"))
            {
                navMeshAgent.enabled = true;
                stateMachine.InitState(Boss_ChaseState.Instance);
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
        }
        else
        {
            navMeshAgent.enabled = false;
            animator.SetTrigger("Die");

            GameManager.Instance.IncreasePlayerExp(100.0f);
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
        navMeshAgent.enabled = false;
    }

    void AttackDisable()
    {
        isAttackFinished = true;
    }

    public void ThrowPoison()
    {
        animator.SetTrigger("DO_ATTACK1");
        StartCoroutine(Poison());
    }

    IEnumerator Poison()
    {
        Player player = GameManager.Instance.player;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 3; i++)
        {
            Vector3 skillPos = GameManager.Instance.player.transform.position;
            float posX = UnityEngine.Random.Range(-5f, 5f);
            float posY = UnityEngine.Random.Range(-5f, 5f);
            skillPos += GameManager.Instance.player.transform.right * posX;
            skillPos += GameManager.Instance.player.transform.forward * posY;

            GameObject instantPoison = Instantiate(skillObj[0], skillPos, Quaternion.identity);

            if (player.IsAlive && !player.IsHit)
            {
                if (Vector3.Distance(instantPoison.transform.position, player.transform.position) < 2.0f)
                {
                    // ���� �ִϸ��̼��� ���� ���� �÷��̾�� ���ظ� �־�� �ϱ� ������, �ִϸ��̼� �̺�Ʈ�� Ȱ���Ѵ�.
                    StartCoroutine(player.DecreaseHealth(10));

                    // ȭ�� ��ü�� ������ ����Ʈ �ִϸ��̼��� Ȱ��ȭ�Ѵ�.
                    GameManager.Instance.systemUI.ShowBloodEffect();
                }
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    public void SpellMeteor()
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
    public void Explosion()
    {
        StartCoroutine(Explosions());
    }
    IEnumerator Explosions()
    {
        Vector3 skillPos = GameManager.Instance.player.transform.position;
        skillPos.y = 1f;
        GameObject instantExplosion = Instantiate(skillObj[2], skillPos, GameManager.Instance.player.transform.rotation);

        animator.SetTrigger("DO_ATTACK3");
        
        Player player = GameManager.Instance.player;

        yield return new WaitForSeconds(2.1f);

        if (player.IsAlive && !player.IsHit)
        {
            if (Vector3.Distance(instantExplosion.transform.position, player.transform.position) < 5.0f)
            {
                // ���� �ִϸ��̼��� ���� ���� �÷��̾�� ���ظ� �־�� �ϱ� ������, �ִϸ��̼� �̺�Ʈ�� Ȱ���Ѵ�.
                StartCoroutine(player.DecreaseHealth(10));

                // ȭ�� ��ü�� ������ ����Ʈ �ִϸ��̼��� Ȱ��ȭ�Ѵ�.
                GameManager.Instance.systemUI.ShowBloodEffect();
            }
        }
    }

}
