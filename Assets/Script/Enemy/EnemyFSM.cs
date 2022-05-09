using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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

    public float moveSpeed = 3.0f;

    [Header("FSM")]
    // 시야 거리 - 벡터 내적을 이용해서 플레이어 위치 탐색
    public float sightRange = 10.0f;
    public float sightAngle = 30.0f;

    [SerializeField, Tooltip("플레이어를 감지하는 거리")]
    public float findDistance = 6.0f;
    public float attackDistance = 1.5f;
    public float returnDistance = 15.0f;
    private bool isDead = false;

    // 초기 위치 저장용 - 일정 거리 이상 벗어나면 돌아간다
    private Vector3 originPos;

    private EnemyState eState = EnemyState.Idle;

    private NavMeshAgent agent;

    private CharacterController characterController;

    private Transform playerTr;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        playerTr = GameObject.Find("Player").transform;
        originPos = transform.position;

        StartCoroutine(CheckMonterState());

    }

    IEnumerator CheckMonterState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.3f);

            // 몬스터가 죽으면 코루틴 종료
            if (eState == EnemyState.Dead)
            {
                yield break;
            }

            float distance = Vector3.Distance(playerTr.position, transform.position);

            if (distance <= attackDistance)
            {
                eState = EnemyState.Attack;
            }
            // 추적 상태
            else if (distance <= findDistance)
            {
                eState = EnemyState.Move;
            }
            else
            {
                eState = EnemyState.Idle;
            }
        }
    }

    private void Idle()
    {
        agent.isStopped = true;
        if (Vector3.Distance(playerTr.position, transform.position) <= findDistance)
        {
            eState = EnemyState.Move;
            // 인식 효과 띄우기
            // 애니메이션 처리
        }
    }

    private void Move()
    {
        agent.SetDestination(playerTr.position);
        agent.isStopped = false;
        if (Vector3.Distance(playerTr.position, transform.position) <= attackDistance)
        {
            eState = EnemyState.Attack;
            // 애니메이션 처리
        }
    }

    private void Attack()
    {
        if (Vector3.Distance(playerTr.position, transform.position) <= attackDistance)
        {
            eState = EnemyState.Attack;
            // 애니메이션 처리
        }
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

    private void OnDrawGizmos()
    {
        
    }
}
