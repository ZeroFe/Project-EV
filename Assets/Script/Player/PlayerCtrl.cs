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
    [SerializeField] 
    private float jumpPower = 7.0f;
    private bool isJumping = false;
    [SerializeField]
    private float gravity = 9.81f;
    private float yVelocity = 0.0f;

    [Header("Attack")] 
    [SerializeField] private Weapon weapon;
    public Weapon PlayerWeapon => weapon;

    [Header("Bomb Skill")]
    public Transform shootPoint;
    public LayerMask layer;
    public GameObject shootCursor;
    public LineRenderer lineVisual;
    private Vector3 bombVelocity;
    public Rigidbody bombPrefab;

    [Header("Missile Skill")]
    public GameObject projectilePrefab;

    // other
    private CharacterController cc;
    private Camera _main;

    private float horizontalInput;
    private float verticalInput;

    // Action
    public event Action OnJump;
    public event Action OnDash;
    public event Action OnDashEnd;
    
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
        Fire();
        Reload();

        // 임시
        FireBomb();
        //FireHomingMissiles();
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

        // 2-1. 메인 카메라를 기준으로 방향을 변환한다.
        dir = _main.transform.TransformDirection(dir);

        CheckDash();

        finalSpeed = currentDashSpeed + moveSpeed;

        Jump();

        // 2-4. 캐릭터 수직 속도에 중력 값을 적용한다.
        yVelocity -= gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 3. 이동 속도에 맞춰 이동한다.
        cc.Move(dir * finalSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        // 2-2. 만일, 점프 중이었고, 다시 바닥에 착지했다면...
        if (isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }

        // 2-3. 만일, 키보드 <Space> 버튼을 입력했고, 점프를 안 한 상태라면...
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
            // Dash 키 누름
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
        OnDash?.Invoke();

        float currTime = 0.0f;
        while (currentDashSpeed > 0.1f)
        {
            currTime += Time.fixedDeltaTime;
            currentDashSpeed = DashSpeedCurve.Evaluate(currTime) * dashSpeed;
            yield return new WaitForFixedUpdate();
        }

        currentDashSpeed = 0.0f;
        isDashing = false;
        OnDashEnd?.Invoke();
    }

    IEnumerator RecoverDashCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownTime);
            if (currentDashCount != maxDashCount)
            {
                currentDashCooldown -= cooldownTime;
                // 갱신 알림!
                if (currentDashCooldown <= 0.0f)
                {
                    currentDashCount++;
                    // 꽉 찼으면 DashCooldown 안 돌리고, 안 꽉찼으면 다시 max부터 시작
                    currentDashCooldown = maxDashCooldown;
                }
            }
        }
    }
    #endregion

    #region Weapon Management

    void Fire()
    {
        weapon.TriggerDown = Input.GetButton("Fire");
        if (Input.GetButtonDown("Fire"))
        {
            weapon.Fire();
        }
    }

    void Reload()
    {
        // 일단 애니메이션 안 하고 Reload만 하게
        if (Input.GetButtonDown("Reload"))
        {
            print("Reload");
            weapon.Reload();
        }
    }

    #endregion

    #region Skill

    private void FireBomb()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Aim();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Launch(bombPrefab);
        }
    }

    private void Aim()
    {
        Ray ray = new Ray(_main.transform.position, _main.transform.forward);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, 100f, layer))
        {
            shootCursor.SetActive(true);
            shootCursor.transform.position = hit.point + Vector3.up * 0.1f;

            bombVelocity = Vec3Parabola.CalculateVelocity(hit.point, shootPoint.position, 0.7f);

            //we include the cursor position as the final nodes for the line visual position
            Visualize(bombVelocity, shootCursor.transform.position);

            //var mySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //mySphere.transform.localScale = Vector3(...);
            //mySphere.transform.position = Vector3(...);


            //transform.rotation = Quaternion.LookRotation(vo);
        }
    }

    private Rigidbody Launch(Rigidbody projectile)
    {
        Rigidbody obj = Instantiate(projectile, shootPoint.position, Quaternion.identity);
        obj.velocity = bombVelocity;
        obj.AddTorque(bombVelocity);

        shootCursor.SetActive(false);
        lineVisual.gameObject.SetActive(false);

        return obj;
    }

    //added final position argument to draw the last line node to the actual target
    void Visualize(Vector3 vo, Vector3 finalPos)
    {
        int lineSegment = lineVisual.positionCount;
        lineVisual.gameObject.SetActive(true);
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = Vec3Parabola.CalculatePosInTime(transform.position, vo, (i / (float)lineSegment) * 0.7f);
            lineVisual.SetPosition(i, pos);
        }

        lineVisual.SetPosition(lineSegment, finalPos);
    }

    private void FireHomingMissiles()
    {
        if (Input.GetKey(KeyCode.E))
        {
            StartCoroutine(HomingShots());
        }
    }

    IEnumerator HomingShots()
    {
        float maxBulletAngle = 45.0f;
        int bulletCount = 7;
        float startAngle = -maxBulletAngle;
        float slicedAngle = (2 * maxBulletAngle) / (float)(bulletCount - 1);
        for (int i = 0; i < bulletCount; i++)
        {
            var dir = transform.forward + transform.right * (i + bulletCount / 2);
            dir.Normalize();
            dir += transform.up;
            dir.Normalize();
            var go = Instantiate(projectilePrefab, transform.position + transform.forward + Vector3.up * 3, Quaternion.identity);
            go.GetComponent<HomingMissile>().Launch(dir, 30.0f);

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CreateProjectile(float angle)
    {
        float angleRad = Mathf.Deg2Rad * angle;
        var rotVec = new Vector3(angleRad, 0, 0);
        //var rotVec = new Vector3(Mathf.Sin(angleRad), , Mathf.Cos(angleRad));
        var go = Instantiate(projectilePrefab, transform.position, Quaternion.AngleAxis(-angle, Vector3.forward));
        go.GetComponent<HomingMissile>().Launch(transform.forward, 30.0f);
    }


    #endregion
}
