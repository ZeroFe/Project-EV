using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    private float rx = 0.0f;
    private float ry = 0.0f;

    public float rotationSpeed = 200.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 각도 초기화
        transform.rotation = Quaternion.identity;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // 사용자의 마우스 움직임을 누적해서 회전값으로 사용하고 싶다
    void Update()
    {
        //float my = Input.GetAxis("Mouse Y");

        //// 회전 값으로 사용
        //ry -= my * Time.deltaTime * rotationSpeed;
        //rx = Mathf.Clamp(rx, -90.0f, 90.0f);

        //transform.localRotation = Quaternion.AngleAxis(ry, Vector3.right);
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
