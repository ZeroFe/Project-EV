using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public static readonly float cooldownTime = 0.1f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    
    private float finalSpeed;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 10.0f;
    [SerializeField] private AnimationCurve DashSpeedCurve;
    [SerializeField] private int maxDashCount = 2;
    private float currDashSpeed = 0.0f;
    private bool isDashing = false;
    private int currDashCount = 1;
    private float currDashCooldown = 3.0f;
    private float maxDashCooldown = 3.0f;

    // Jump
    [SerializeField] private float jumpPower = 7.0f;
    private bool isJumping = false;
    private float gravity = -20.0f;
    private float yVelocity = 0.0f;

    private CharacterController cc;

    private float horizontalInput;
    private float verticalInput;

    #region Rotate
    private float rx = 0.0f;
    private float ry = 0.0f;

    public float rotationSpeed = 200.0f;
    #endregion

    public event Action onJump;
    public event Action onDash;

    #region Init
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        StartCoroutine(RecoverDashCooldown());
    }

    #endregion

    #region Movement
    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // ȸ�� ������ ���
        ry += mx * Time.deltaTime * rotationSpeed;
        rx -= my * Time.deltaTime * rotationSpeed;
        rx = Mathf.Clamp(rx, -90.0f, 90.0f);

        transform.rotation = Quaternion.AngleAxis(ry, Vector3.up);
        Camera.main.transform.localRotation = Quaternion.AngleAxis(rx, Vector3.right);
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontalInput, 0, verticalInput);
        dir = dir.normalized;

        // 2-1. ���� ī�޶� �������� ������ ��ȯ�Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);

        CheckDash();

        finalSpeed = currDashSpeed + moveSpeed;

        Jump();

        // 2-4. ĳ���� ���� �ӵ��� �߷� ���� �����Ѵ�.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 3. �̵� �ӵ��� ���� �̵��Ѵ�.
        cc.Move(dir * finalSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        // 2-2. ����, ���� ���̾���, �ٽ� �ٴڿ� �����ߴٸ�...
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }

        // 2-3. ����, Ű���� <Space> ��ư�� �Է��߰�, ������ �� �� ���¶��...
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
    }

    private void CheckDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("LShift");
            // Dash Ű ����
            if (!isDashing && currDashCount > 0)
            {
                currDashCount--;
                StartCoroutine(Dash());
            }
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        // �뽬 ����
        currDashSpeed = dashSpeed;
        float currTime = 0.0f;
        while (currDashSpeed > 0)
        {
            currTime += Time.fixedDeltaTime;
            currDashSpeed = DashSpeedCurve.Evaluate(currTime) * dashSpeed;
            yield return new WaitForFixedUpdate();
        }
        currDashSpeed = 0.0f;
        isDashing = false;
    }

    IEnumerator RecoverDashCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownTime);
            if (currDashCount != maxDashCount)
            {
                currDashCooldown -= cooldownTime;
                // ���� �˸�!
                if (currDashCooldown <= 0.0f)
                {
                    currDashCount++;
                    // �� á���� DashCooldown �� ������, �� ��á���� �ٽ� max���� ����
                    currDashCooldown = maxDashCooldown;
                }
            }
        }
    }
    #endregion
}
