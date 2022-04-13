using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 게임 오브젝트
    private GameObject explosion;
    private GameObject playerSword;
    private GameObject rightHand;
    public GameObject magicFireObj;
    
    // 카메라 셰이킹 이펙트
    public CameraShake CameraShake;
    public float duration;
    public float magnitude= 0.01f;
    
    // 캐릭터 움직임 및 애니메이션
    public float speed;
    private Animator _anim;
    private CharacterController _controller;
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
    
    void GetInput()
    {
        f1Down = Input.GetButtonDown("Fire1");
        f2Down = Input.GetButtonDown("Fire2");
        f3Down = Input.GetButtonDown("Fire3");
        s1Down = Input.GetButtonDown("Skill1");
    }
    private void Awake()
    {
        _anim = this.GetComponent<Animator>();
        _controller = this.GetComponent<CharacterController>();
        cam = Camera.main;
        explosion = GameObject.Find("ExplosionEffect");
        playerSword = GameObject.FindWithTag("Player").transform.Find("Maria_sword").gameObject;
        rightHand = GameObject.Find("RightHand");
        explosion.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ShakeCamera();
        Attack();
        Move();
        
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true;
        }
        else
        {
            toggleCameraRotation = false;
        }
    }

    private void LateUpdate()
    {
        if (toggleCameraRotation != true)
        {
            Vector3 playerRotate = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate),
                Time.deltaTime * smoothness);
        }
    }

    void Move()
    {
        if (!underAttack)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

            _controller.Move(moveDirection.normalized * speed * Time.deltaTime);

            _anim.SetBool("IS_RUN", moveDirection != Vector3.zero);
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
            _anim.SetTrigger("DO_ATTACK1");
            Invoke("AttackDisable", 5f);
        }

        else if (!underAttack && f2Down)
        {
            underAttack = true;
            playerSword.SetActive(true);
            _anim.SetTrigger("DO_ATTACK2");
            Invoke("AttackDisable", 2f);
        }
        else if (!underAttack && f3Down)
        {
            underAttack = true;
            //explosion.SetActive(true);
            _anim.SetTrigger("DO_ATTACK3");
            Invoke("AttackDisable", 1.3f);
        }
        
        else if (!underAttack && s1Down)
        {
            underAttack = true;
            _anim.SetTrigger("DO_SKILL1");
            Invoke("AttackDisable", 2.4f);
            Invoke("ThrowFire",0.7f);

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

    void ThrowFire()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, 100))
        {
            Vector3 oPos = transform.position;
            GameObject instantMagicFire = Instantiate(magicFireObj, rightHand.transform.position, transform.rotation);
            Rigidbody rigidMagicFire = instantMagicFire.GetComponent<Rigidbody>();
            rigidMagicFire.AddForce(transform.forward * 5.0f, ForceMode.Impulse);
            //rigidMagicFire.AddTorque(Vector3.back * 10, ForceMode.Impulse);
        }
    }
    
    void AttackDisable()
    {
        underAttack = false;
        explosion.SetActive(false);
        playerSword.SetActive(false);
    }

}
