using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public float camSpeed = 10000f;
    public float mouseSensitivity = 100f;
    public float maxAngle = 70f;

    private float rotX;
    private float rotY;

    public Transform realCamera;
    public Vector3 dirNormalized;
    public Vector3 finalDir;

    // 뒤에 장애물이 있으면 카메라가 플레이어에게 근접
    public float minDistance;
    public float maxDistance;
    public float finalDistance;
    public float smoothness = 10f;

    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * mouseSensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        
        rotX = Mathf.Clamp(rotX, -maxAngle, maxAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, camSpeed * Time.deltaTime);

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        RaycastHit hit;

        if (Physics.Linecast(transform.position, finalDir, out hit))
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        
        else
        {
            finalDistance = maxDistance;
        }

        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance,
            Time.deltaTime * smoothness);
    }
}