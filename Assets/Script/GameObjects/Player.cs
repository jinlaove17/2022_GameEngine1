using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    // 게임 오브젝트
    public GameObject playerSword;
    public GameObject rightHand;

    public GameObject[] skillObj;

    // 카메라 셰이킹 이펙트
    private CameraShake cameraShake;
    public float duration;
    public float magnitude;

    // 캐릭터 움직임 및 애니메이션
    private Animator animator;
    private CharacterController characterController;
    private Rigidbody rigidBody;
    public float speed;

    // 캐릭터 공격 중
    public bool underAttack;

    // 카메라 회전
    public bool toggleCameraRotation;
    public float smoothness = 10.0f;

    // 키 입력
    private bool f1Down;
    private bool f2Down;
    private bool s1Down;
    private bool s2Down;
    private bool s3Down;
    private bool s4Down;

    // 플레이어의 레벨
    private int level = 1;

    // 플레이어의 경험치
    public Slider expBar = null;
    public Text expText = null;
    private float exp = 0;

    private void Awake()
    {
        if (playerSword == null)
        {
            Debug.LogError("PlayerSword가 장착되지 않았습니다.");
        }

        if (rightHand == null)
        {
            Debug.LogError("RightHand가 장착되지 않았습니다.");
        }

        animator = transform.GetComponent<Animator>();
        characterController = transform.GetComponent<CharacterController>();
        rigidBody = transform.GetComponent<Rigidbody>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void Update()
    {
        if (IsAlive)
        {
            GetInput();
            Attack();
            Move();
        }
    }

    private void LateUpdate()
    {
        if (!toggleCameraRotation && !underAttack)
        {
            Vector3 playerRotate = Vector3.Scale(Camera.main.transform.forward, new Vector3(1.0f, 0, 1.0f));

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }

    private void GetInput()
    {
        f1Down = Input.GetButtonDown("Fire1");
        f2Down = Input.GetButtonDown("Fire2");
        s1Down = Input.GetButtonDown("Skill1");
        s2Down = Input.GetButtonDown("Skill2");
        s3Down = Input.GetButtonDown("Skill3");
        s4Down = Input.GetButtonDown("Skill4");

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true;
        }
        else
        {
            toggleCameraRotation = false;
        }
    }

    private void Move()
    {
        if (!underAttack)
        {
            Vector3 moveDirection = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");

            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
            animator.SetBool("IS_RUN", moveDirection != Vector3.zero);
        }
    }

    private void Attack()
    {
        if (!underAttack)
        {
            if (f1Down)
            {
                playerSword.SetActive(true);

                underAttack = true;
                animator.SetTrigger("DO_ATTACK1");
            }
            else if (f2Down)
            {
                playerSword.SetActive(true);

                underAttack = true;
                animator.SetTrigger("DO_ATTACK2");
            }
            else if (s1Down)
            {
                underAttack = true;
                animator.SetTrigger("DO_SKILL1");
            }
            else if (s2Down)
            {
                underAttack = true;
                animator.SetTrigger("DO_SKILL2");
            }
            else if (s3Down)
            {
                underAttack = true;
                animator.SetTrigger("DO_SKILL3");
            }
            else if (s4Down)
            {
                underAttack = true;
                animator.SetTrigger("DO_SKILL4");
            }
        }
    }

    private void ShakeCamera()
    {
        StartCoroutine(cameraShake.Shake(duration, magnitude));
    }

    private void ThrowFire()
    {
        GameObject fireInstance = Instantiate(skillObj[1], rightHand.transform.position, Quaternion.identity);
        Rigidbody fireRigidBody = fireInstance.GetComponent<Rigidbody>();

        fireRigidBody.AddForce(15.0f * transform.forward, ForceMode.Impulse);
    }

    private void DeadExplode()
    {
        Vector3 skillPos = transform.position;

        skillPos += transform.forward * 20.0f;
        GameObject instantDeadExplode = Instantiate(skillObj[0], skillPos, Quaternion.identity);
    }

    private void Meteor()
    {
        StartCoroutine("SpawnMeteor");
    }

    private IEnumerator SpawnMeteor()
    {
        WaitForSeconds spawnTime = new WaitForSeconds(0.4f);

        for (int i = 0; i < 5; i++)
        {
            Vector3 skillPos = transform.position;
            var forward = UnityEngine.Random.Range(10.0f, 20.0f);
            var side = UnityEngine.Random.Range(-20.0f, 20.0f);

            skillPos += transform.forward * forward;
            skillPos += transform.right * side;

            GameObject instantMeteor = Instantiate(skillObj[2], skillPos, transform.rotation);

            yield return spawnTime;
        }
    }

    private void EnegyExplode()
    {
        Transform skillPos = GameObject.FindWithTag("Player").transform.Find("EnegyExplodePos").transform;
        GameObject instantEnergyExplode = Instantiate(skillObj[3], skillPos.transform.position, skillPos.transform.rotation);

        StartCoroutine(ActivateAttackCollision(instantEnergyExplode));
    }

    private void AttackDisable()
    {
        underAttack = false;
        playerSword.SetActive(false);
    }

    private IEnumerator ActivateAttackCollision(GameObject instantObj)
    {
        yield return new WaitForSeconds(2.0f);

        GameObject attackCollide = instantObj.transform.Find("DamageRange").gameObject;
        attackCollide.SetActive(true);
    }

    public IEnumerator IncreaseExp(float expIncrement)
    {
        const float maxExp = 500.0f;
        const float duration = 2.0f;
        float offsetPerFrame = (expIncrement / duration) * Time.deltaTime;
        float restExpIncrement = expIncrement;
        float expPer = 0.0f;

        while (restExpIncrement >= 0.0f)
        {
            exp += offsetPerFrame;
            restExpIncrement -= offsetPerFrame;

            if (exp >= maxExp)
            {
                exp = 0.0f;
                level += 1;

                GameManager.Instance.skillSelectionUI.UpdateSkillSelectionUI();
                GameManager.Instance.skillSelectionUI.gameObject.SetActive(true);

                Time.timeScale = 0.0f;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }

            expPer = exp / maxExp;
            expBar.value = expPer;
            expText.text = 100.0f * expPer + "%";

            yield return null;
        }
    }
}
