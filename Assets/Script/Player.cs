using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private float hAxis;
    private float vAxis;
    
    private Vector3 moveVec;
    private Animator _anim;
    private CharacterController _controller;
    private Camera cam;

    public bool toggleCameraRotation;
    public float smoothness = 10f;
    private void Awake()
    {
        _anim = this.GetComponent<Animator>();
        _controller = this.GetComponent<CharacterController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true;
        }
        else
        {
            toggleCameraRotation = false;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        _controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        
        _anim.SetBool("IS_RUN", moveDirection != Vector3.zero);

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
}
