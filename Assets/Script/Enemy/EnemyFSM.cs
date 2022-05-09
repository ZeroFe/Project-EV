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
    // �þ� �Ÿ� - ���� ������ �̿��ؼ� �÷��̾� ��ġ Ž��
    public float sightRange = 10.0f;
    public float sightAngle = 30.0f;

    [SerializeField, Tooltip("�÷��̾ �����ϴ� �Ÿ�")]
    public float findDistance = 6.0f;
    public float attackDistance = 1.5f;
    public float returnDistance = 15.0f;
    private bool isDead = false;

    // �ʱ� ��ġ ����� - ���� �Ÿ� �̻� ����� ���ư���
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

            // ���Ͱ� ������ �ڷ�ƾ ����
            if (eState == EnemyState.Dead)
            {
                yield break;
            }

            float distance = Vector3.Distance(playerTr.position, transform.position);

            if (distance <= attackDistance)
            {
                eState = EnemyState.Attack;
            }
            // ���� ����
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
            // �ν� ȿ�� ����
            // �ִϸ��̼� ó��
        }
    }

    private void Move()
    {
        agent.SetDestination(playerTr.position);
        agent.isStopped = false;
        if (Vector3.Distance(playerTr.position, transform.position) <= attackDistance)
        {
            eState = EnemyState.Attack;
            // �ִϸ��̼� ó��
        }
    }

    private void Attack()
    {
        if (Vector3.Distance(playerTr.position, transform.position) <= attackDistance)
        {
            eState = EnemyState.Attack;
            // �ִϸ��̼� ó��
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
