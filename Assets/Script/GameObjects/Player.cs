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

    // 카메라 셰이킹 이펙트
    private CameraShake cameraShake;
    public float duration;
    public float magnitude;

    // 캐릭터 움직임 및 애니메이션
    private Animator animator;
    private CharacterController characterController;
    public float speed;

    // 캐릭터 공격 중
    public bool underAttack;

    // 카메라 회전
    public bool toggleCameraRotation;
    public float smoothness = 10.0f;

    // 키 입력
    private int slotIndex = 0;

    // 플레이어의 레벨
    private int level = 1;

    // 플레이어의 체력 관련 데이터
    public Slider healthBar;
    public Text healthText;

    // 플레이어의 경험치 관련 데이터
    public Slider expBar;
    public Text expText;
    private float exp = 0.0f;

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
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void Start()
    {
        SkillManager.Instance.RegisterSkill(SKILL_TYPE.ThrowFire);
    }

    private void Update()
    {
        if (IsAlive)
        {
            GetInput();
            Move();
            Attack();
        }
    }

    private void LateUpdate()
    {
        if (!underAttack && !toggleCameraRotation)
        {
            Vector3 playerRotate = Vector3.Scale(Camera.main.transform.forward, new Vector3(1.0f, 0, 1.0f));

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), smoothness * Time.deltaTime);
        }
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            slotIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slotIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            slotIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            slotIndex = 3;
        }
        else
        {
            // 아무 키도 눌리지 않았다면, -1로 설정한다.
            slotIndex = -1;
        }

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
            Vector3 userInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
            Vector3 moveDirection = transform.right * userInput.x + transform.forward * userInput.z;
            bool isRun = Input.GetKey(KeyCode.LeftShift);
                
            characterController.Move(moveDirection.normalized * speed * (isRun ? 2.0f : 1.0f) * Time.deltaTime);
            animator.SetBool("IS_WALK", moveDirection != Vector3.zero);
            animator.SetBool("IS_RUN", isRun);
            
            if (isRun)
            {
                animator.SetBool("IS_LEFT_RUN", userInput.x < 0.0 && userInput.z == 0.0f);
                animator.SetBool("IS_RIGHT_RUN", userInput.x > 0.0 && userInput.z == 0.0f);
            }
            else
            {
                animator.SetBool("IS_LEFT_WALK", userInput.x < 0.0 && userInput.z == 0.0f);
                animator.SetBool("IS_RIGHT_WALK", userInput.x > 0.0 && userInput.z == 0.0f);
            }
        }
    }

    private void Attack()
    {
        if (!underAttack && slotIndex >= 0)
        {
            SkillManager.Instance.UseSkill(slotIndex);
        }
    }

    private void AttackDisable()
    {
        underAttack = false;
        playerSword.SetActive(false);
    }

    private void ShakeCamera()
    {
        StartCoroutine(cameraShake.Shake(duration, magnitude));
    }

    public void TransAnimation(string triggerName)
    {
        if (animator)
        {
            underAttack = true;
            animator.SetTrigger(triggerName);
        }
    }

    public void GenerateEffect(AnimationEvent animationEvent)
    {
        SkillManager.Instance.GenerateEffect(animationEvent.intParameter);
    }

    public IEnumerator DecreaseHealth(float healthDecrement)
    {
        const float maxHealth = 100.0f;
        const float duration = 0.8f;
        float offsetPerFrame = (healthDecrement / duration) * Time.deltaTime;
        float restHealthIncrement = healthDecrement;
        float healthPer;

        while (restHealthIncrement >= 0.0f)
        {
            Health -= offsetPerFrame;
            restHealthIncrement -= offsetPerFrame;

            healthPer = Health / maxHealth;
            healthBar.value = healthPer;
            healthText.text = (100.0f * healthPer).ToString("F1") + "%";

            if (Health <= 0.0f)
            {
                break;
            }

            yield return null;
        }
    }

    public IEnumerator IncreaseExp(float expIncrement)
    {
        const float maxExp = 500.0f;
        const float duration = 2.0f;
        float offsetPerFrame = (expIncrement / duration) * Time.deltaTime;
        float restExpIncrement = expIncrement;
        float expPer;

        while (restExpIncrement >= 0.0f)
        {
            exp += offsetPerFrame;
            restExpIncrement -= offsetPerFrame;

            if (exp >= maxExp)
            {
                exp = 0.0f;
                level += 1;

                SkillManager.Instance.skillSelectionUI.ActivateUI();
            }

            expPer = exp / maxExp;
            expBar.value = expPer;
            expText.text = (100.0f * expPer).ToString("F1") + "%";

            yield return null;
        }
    }
}
