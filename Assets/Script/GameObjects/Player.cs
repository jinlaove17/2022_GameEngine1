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

    // 플레이어의 경험치
    public Slider expBar = null;
    public Text expText = null;
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

        // 기본 공격 스킬을 추가한다.
        SkillManager.Instance.InsertSkill(0);
        SkillManager.Instance.InsertSkill(1);
        SkillManager.Instance.InsertSkill(2);
        SkillManager.Instance.InsertSkill(3);
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
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            slotIndex = 4;
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
            Vector3 moveDirection = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");

            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
            animator.SetBool("IS_RUN", moveDirection != Vector3.zero);
        }
    }

    private void Attack()
    {
        if (!underAttack && slotIndex >= 0)
        {
            if (!SkillManager.Instance.CheckSlotEmpty(slotIndex))
            {
                SkillManager.Instance.UseSkill(slotIndex);
            }
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

                SkillManager.Instance.skillSelectionUI.UpdateSkillSelectionUI();

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
