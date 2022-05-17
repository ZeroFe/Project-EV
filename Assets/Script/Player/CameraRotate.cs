using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField, Range(100, 1000)]
    private float rotationSpeed = 200.0f;
    private float rx = 0.0f;
    private float ry = 0.0f;

    private float bottomClamp = -90.0f;
    private float topClamp = 90.0f;

    private Camera _main;

    private void Awake()
    {
        _main = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        ry += mx * Time.deltaTime * rotationSpeed;
        rx -= my * Time.deltaTime * rotationSpeed;
        rx = Mathf.Clamp(rx, bottomClamp, topClamp);

        transform.eulerAngles = new Vector3(rx, ry, 0);
    }
}
