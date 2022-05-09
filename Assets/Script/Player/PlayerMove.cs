using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    [FormerlySerializedAs("speed")] public float moveSpeed = 50.0f;
    
    public float jumpPower = 7.0f;
    private bool isJumping = false;

    private CharacterController cc;
    [SerializeField]
    private float gravity = -20.0f;

    public float yVelocity = 0.0f;


    private Rigidbody _rigidbody;

    private float horizontalInput;
    private float verticalInput;

    #region Init
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    #endregion

    #region Movement
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontalInput, 0, verticalInput);
        dir = dir.normalized;

        // 2-1. 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);

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

        // 2-4. 캐릭터 수직 속도에 중력 값을 적용한다.
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        // 3. 이동 속도에 맞춰 이동한다.
        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
    #endregion
}
