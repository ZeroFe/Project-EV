using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Dead
    }

    // 시야 거리 - 벡터 내적을 이용해서 플레이어 위치 탐색
    public float sightRange;
    public float sightAngle;

    [SerializeField, Tooltip("플레이어를 감지하는 거리")]
    public float findDistance;
    public float attackDistance;
    public float returnDistance;

    // 초기 위치 저장용 - 일정 거리 이상 벗어나면 돌아간다
    private Vector3 originPos;

    private EnemyState eState = EnemyState.Idle;

    private CharacterController characterController;

    private Transform playerTr;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        playerTr = GameObject.Find("Player").transform;
        originPos = transform.position;
    }

    private void Update()
    {
        switch (eState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    private void Idle()
    {
        if (Vector3.Distance(playerTr.position, transform.position) < findDistance)
        {
            eState = EnemyState.Move;
            // 인식 효과 띄우기
            // 애니메이션 처리
        }
    }

    private void Move()
    {

    }

    private void Attack()
    {

    }

    private void Return()
    {

    }

    private void Damaged()
    {

    }

    private void Dead()
    {

    }
}
