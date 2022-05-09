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
        // ���� �ʱ�ȭ
        transform.rotation = Quaternion.identity;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // ������� ���콺 �������� �����ؼ� ȸ�������� ����ϰ� �ʹ�
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // ȸ�� ������ ���
        ry += mx * Time.deltaTime * rotationSpeed;
        rx -= my * Time.deltaTime * rotationSpeed;
        rx = Mathf.Clamp(rx, -90.0f, 90.0f);

        transform.eulerAngles = new Vector3(rx, ry, 0);
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
