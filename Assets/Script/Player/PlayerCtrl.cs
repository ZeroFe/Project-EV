using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public static readonly float cooldownTime = 0.1f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    
    private float finalSpeed;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 10.0f;
    [SerializeField] private AnimationCurve DashSpeedCurve;
    [SerializeField] private int maxDashCount = 2;
    private float currentDashSpeed = 0.0f;
    private bool isDashing = false;
    private int currentDashCount = 1;
    private float currentDashCooldown = 3.0f;
    private float maxDashCooldown = 3.0f;

    [Header("Jump")]
    [SerializeField] private float jumpPower = 7.0f;
    private bool isJumping = false;
    private float gravity = -20.0f;
    private float yVelocity = 0.0f;

    [Header("Rotation")]
    public float rotationSpeed = 200.0f;
    [SerializeField, Tooltip("How far in degrees can you move the camera up")]
    private float topClamp = 90.0f;
    [SerializeField, Tooltip("How far in degrees can you move the camera down")]
    private float bottomClamp = -90.0f;
    private float rx = 0.0f;
    private float ry = 0.0f;
    private Camera _main;

    [Header("Attack")] 
    [SerializeField] private Weapon weapon;
    public Weapon PlayerWeapon => weapon;

    // other
    private CharacterController cc;

    private float horizontalInput;
    private float verticalInput;

    // Action
    public event Action onJump;
    public event Action onDash;
    public event Action onDashEnd;
    
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        _main = Camera.main;
    }

    private void Start()
    {
        weapon.PickedUp(this);
        StartCoroutine(RecoverDashCooldown());
    }

    #region Movement
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void LateUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // ȸ�� ������ ���
        ry += mx * Time.deltaTime * rotationSpeed;
        rx -= my * Time.deltaTime * rotationSpeed;
        rx = Mathf.Clamp(rx, bottomClamp, topClamp);

        transform.rotation = Quaternion.AngleAxis(ry, Vector3.up);
        _main.transform.localRotation = Quaternion.AngleAxis(rx, Vector3.right);
    }


    //private void GroundedCheck()
    //{
    //    // set sphere position, with offset
    //    Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
    //    Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    //}

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontalInput, 0, verticalInput);
        dir = dir.normalized;

        // 2-1. ���� ī�޶� �������� ������ ��ȯ�Ѵ�.
        dir = _main.transform.TransformDirection(dir);

        CheckDash();

        finalSpeed = currentDashSpeed + moveSpeed;

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
            if (!isDashing && currentDashCount > 0)
            {
                currentDashCount--;
                StartCoroutine(Dash());
            }
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        currentDashSpeed = dashSpeed;

        float currTime = 0.0f;
        while (currentDashSpeed > 0.1f)
        {
            currTime += Time.fixedDeltaTime;
            currentDashSpeed = DashSpeedCurve.Evaluate(currTime) * dashSpeed;
            yield return new WaitForFixedUpdate();
        }

        currentDashSpeed = 0.0f;
        isDashing = false;
        onDashEnd?.Invoke();
    }

    IEnumerator RecoverDashCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownTime);
            if (currentDashCount != maxDashCount)
            {
                currentDashCooldown -= cooldownTime;
                // ���� �˸�!
                if (currentDashCooldown <= 0.0f)
                {
                    currentDashCount++;
                    // �� á���� DashCooldown �� ������, �� ��á���� �ٽ� max���� ����
                    currentDashCooldown = maxDashCooldown;
                }
            }
        }
    }
    #endregion

    #region Weapon Management

    //void 

    void Reload()
    {
        // �ϴ� �ִϸ��̼� �� �ϰ� Reload�� �ϰ�
        if (Input.GetButtonDown("Reload"))
        {
            
        }
    }

    #endregion
}
