using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Entity
{
    // 게임 오브젝트
    private GameObject playerSword;
    private GameObject rightHand;
    private GameObject deadExplodePos;
    public GameObject magicFireObj;
    public GameObject deadExplodeObj;

    // 카메라 셰이킹 이펙트
    public CameraShake CameraShake;
    public float duration;
    public float magnitude = 0.01f;

    // 캐릭터 움직임 및 애니메이션
    public float speed;
    private Animator animator;
    private CharacterController characterController;
    private Camera cam;

    // 캐릭터 공격
    private float elapsed;
    public float shakingDelay;
    private bool shakingReady;
    private bool underAttack;

    // 카메라 회전
    public bool toggleCameraRotation;
    public float smoothness = 10f;

    // 키 입력
    private bool f1Down;
    private bool f2Down;
    private bool f3Down;
    private bool s1Down;

    // 플레이어의 레벨
    private int level = 1;

    // 플레이어의 경험치
    public Slider expBar = null;
    public TMP_Text expText = null;
    private float exp = 0;

    private void Awake()
    {
        playerSword = GameObject.Find("RightHand").transform.Find("Maria_sword").gameObject;
        deadExplodePos = GameObject.Find("DeadExplodePos");
        rightHand = GameObject.Find("RightHand");

        animator = transform.GetComponent<Animator>();
        characterController = transform.GetComponent<CharacterController>();
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (toggleCameraRotation != true)
        {
            Vector3 playerRotate = Vector3.Scale(cam.transform.forward, new Vector3(1.0f, 0.0f, 1.0f));

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }

    private void Update()
    {
        GetInput();
        ShakeCamera();
        Attack();
        Move();
    }

    void GetInput()
    {
        f1Down = Input.GetButtonDown("Fire1");
        f2Down = Input.GetButtonDown("Fire2");
        f3Down = Input.GetButtonDown("Skill1");
        s1Down = Input.GetButtonDown("Skill2");

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true;
        }
        else
        {
            toggleCameraRotation = false;
        }
    }

    void Move()
    {
        if (!underAttack)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
            animator.SetBool("IS_RUN", moveDirection != Vector3.zero);
        }
    }

    void Attack()
    {
        if (!underAttack && f1Down)
        {
            elapsed = 0;
            playerSword.SetActive(true);
            shakingReady = true;
            underAttack = true;
            animator.SetTrigger("DO_ATTACK1");

            Invoke("AttackDisable", 5f);
        }
        else if (!underAttack && f2Down)
        {
            underAttack = true;
            playerSword.SetActive(true);
            animator.SetTrigger("DO_ATTACK2");

            Invoke("AttackDisable", 2f);
        }
        else if (!underAttack && f3Down)
        {
            underAttack = true;
            animator.SetTrigger("DO_SKILL1");

            Invoke("AttackDisable", 1.3f);
            Invoke("DeadExplode", 0.2f);
        }

        else if (!underAttack && s1Down)
        {
            underAttack = true;
            animator.SetTrigger("DO_SKILL2");

            Invoke("AttackDisable", 1.25f);
            Invoke("ThrowFire", 0.46f);
        }
    }

    void ShakeCamera()
    {
        if (shakingReady)
        {
            elapsed += Time.deltaTime;

            if (shakingDelay < elapsed)
            {
                StartCoroutine(CameraShake.Shake(duration, magnitude));
                shakingReady = false;
            }
        }
    }

    void DeadExplode()
    {
        GameObject instantDeadExplode = Instantiate(deadExplodeObj, deadExplodePos.transform.position, transform.rotation);
        Rigidbody rigidDeadExplode = instantDeadExplode.GetComponent<Rigidbody>();

        rigidDeadExplode.MovePosition(deadExplodePos.transform.forward * 20f);
    }

    void ThrowFire()
    {
        GameObject instantMagicFire = Instantiate(magicFireObj, rightHand.transform.position, transform.rotation);
        Rigidbody rigidMagicFire = instantMagicFire.GetComponent<Rigidbody>();

        rigidMagicFire.AddForce(transform.forward * 20.0f, ForceMode.Impulse);
        //rigidMagicFire.AddTorque(Vector3.back * 10, ForceMode.Impulse);
    }

    void AttackDisable()
    {
        underAttack = false;
        playerSword.SetActive(false);
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
            }

            expPer = exp / maxExp;
            expBar.value = expPer;
            expText.text = 100.0f * expPer + "%";

            yield return null;
        }
    }
}
