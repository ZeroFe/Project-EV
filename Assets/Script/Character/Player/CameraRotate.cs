using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [System.Serializable]
    private class AdvencedSetting
    {
        public Transform targetTr;
        public bool isRotateX = false;
        public bool isRotateY = true;
    }

    [SerializeField, Range(100, 1000)]
    private float rotationSpeed = 200.0f;
    private float rx = 0.0f;
    private float ry = 0.0f;

    [SerializeField]
    private float bottomClamp = -90.0f;
    [SerializeField]
    private float topClamp = 90.0f;

    [Header("Advanced Setting")] [SerializeField]
    private List<AdvencedSetting> advencedSettings = new List<AdvencedSetting>();

    private Camera _main;

    private void Awake()
    {
        _main = Camera.main;
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        ry += mx * Time.deltaTime * rotationSpeed;
        rx -= my * Time.deltaTime * rotationSpeed;
        rx = Mathf.Clamp(rx, bottomClamp, topClamp);

        transform.eulerAngles = new Vector3(rx, ry, 0);

        for (int i = 0; i < advencedSettings.Count; i++)
        {
            var targetTr = advencedSettings[i].targetTr;
            float newRx = advencedSettings[i].isRotateX ? rx : targetTr.eulerAngles.x;
            float newRy = advencedSettings[i].isRotateY ? ry : targetTr.eulerAngles.y;
            targetTr.eulerAngles = new Vector3(newRx, newRy, 0);
        }
    }
}
