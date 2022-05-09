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

    // �þ� �Ÿ� - ���� ������ �̿��ؼ� �÷��̾� ��ġ Ž��
    public float sightRange;
    public float sightAngle;

    [SerializeField, Tooltip("�÷��̾ �����ϴ� �Ÿ�")]
    public float findDistance;
    public float attackDistance;
    public float returnDistance;

    // �ʱ� ��ġ ����� - ���� �Ÿ� �̻� ����� ���ư���
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
            // �ν� ȿ�� ����
            // �ִϸ��̼� ó��
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
