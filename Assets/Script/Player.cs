using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class Player : MonoBehaviour
{
    // 게임 오브젝트
    private GameObject playerSword;
    private GameObject rightHand;
    private GameObject deadExplodePos;
    public GameObject magicFireObj;
    public GameObject deadExplodeObj;
    public GameObject meteorObj;
    
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
    public bool underAttack;
    
    // 카메라 회전
    public bool toggleCameraRotation;
    public float smoothness = 10f;
    
    // 키 입력
    private bool f1Down;
    private bool f2Down;
    private bool s1Down;
    private bool s2Down;
    private bool s3Down;
    
    void GetInput()
    {
        f1Down = Input.GetButtonDown("Fire1");
        f2Down = Input.GetButtonDown("Fire2");
        s1Down = Input.GetButtonDown("Skill1");
        s2Down = Input.GetButtonDown("Skill2");
        s3Down = Input.GetButtonDown("Skill3");
    }
    private void Awake()
    {
        _anim = this.GetComponent<Animator>();
        _controller = this.GetComponent<CharacterController>();
        cam = Camera.main;
        playerSword = GameObject.Find("mixamorig:RightHand").transform.Find("Sword").gameObject;
        deadExplodePos = GameObject.Find("DeadExplodePos");
        rightHand = GameObject.Find("mixamorig:RightHand");
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
        if (!toggleCameraRotation && !underAttack)
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
        }

        else if (!underAttack && f2Down)
        {
            underAttack = true;
            playerSword.SetActive(true);
            _anim.SetTrigger("DO_ATTACK2");
        }
        else if (!underAttack && s1Down)
        {
            underAttack = true;
            _anim.SetTrigger("DO_SKILL1");
        }
        
        else if (!underAttack && s2Down)
        {
            underAttack = true;
            _anim.SetTrigger("DO_SKILL2");
        }
        
        else if (!underAttack && s3Down)
        {
            underAttack = true;
            _anim.SetTrigger("DO_SKILL3");
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
        Vector3 skillPos = transform.position;
        skillPos += transform.forward * 20;
        GameObject instantDeadExplode = Instantiate(deadExplodeObj, skillPos, transform.rotation);
    }
    
    void ThrowFire()
    {
        GameObject instantMagicFire = Instantiate(magicFireObj, rightHand.transform.position, transform.rotation);
        Rigidbody rigidMagicFire = instantMagicFire.GetComponent<Rigidbody>();
        rigidMagicFire.AddForce(transform.forward * 20.0f, ForceMode.Impulse);
        //rigidMagicFire.AddTorque(Vector3.back * 10, ForceMode.Impulse);
    }

    void Meteor()
    {
        StartCoroutine("SpawnMeteor");
    }
    
    IEnumerator SpawnMeteor()
    {
        WaitForSeconds spawnTime = new WaitForSeconds(0.4f);
        for (int i = 0; i < 5; i++)
        {
            Vector3 skillPos = transform.position;
            var forward = UnityEngine.Random.Range(10f, 40f);
            var side = UnityEngine.Random.Range(-20f, 20f);
            
            skillPos += transform.forward * forward;
            skillPos += transform.right * side;
            
            GameObject instantMeteor = Instantiate(meteorObj, skillPos, transform.rotation);
            yield return spawnTime;
        }
    
    }
    
    void AttackDisable()
    {
        underAttack = false;
        playerSword.SetActive(false);
    }

}
