using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
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
    private bool attackReady;
    
    // 카메라 회전
    public bool toggleCameraRotation;
    public float smoothness = 10f;
    
    // 키 입력
    bool fDown;
    void GetInput()
    {
        fDown = Input.GetButton("Fire1");
    }
    private void Awake()
    {
        _anim = this.GetComponent<Animator>();
        _controller = this.GetComponent<CharacterController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
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
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        _controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        
        _anim.SetBool("IS_RUN", moveDirection != Vector3.zero);
        print(moveDirection);
    }
    void Attack()
    {
        if (fDown)
        {
            elapsed = 0;
            attackReady = true;
            _anim.SetTrigger("DO_ATTACK");
        }
       
        if (attackReady)
        {
            elapsed += Time.deltaTime;
            if (shakingDelay < elapsed)
            {
                StartCoroutine(CameraShake.Shake(duration, magnitude));
                attackReady = false;
            }
        
        }
    }
}
