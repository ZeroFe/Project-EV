using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFSM : MonoBehaviour
{
    // ���ʹ� ���� ���
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    protected Transform targetTr;

    protected EnemyStatus status;
    protected NavMeshAgent agent;
    protected CharacterController cc;

    protected float moveSpeed = 5.0f;

    // ���ʹ� ���� ����
    EnemyState m_State;

    // ���� ���� ����
    public float attackDistance = 2f;

    // ���� �ð�
    float currentTime = 0;

    // ���� ������ �ð�
    float attackDelay = 2f;

    // ���ʹ� ���ݷ�
    public int attackPower = 3;

    // �̵� ���� ����
    public float moveDistance = 20f;
    
    // ���ʹ� hp Slider ����
    public Slider hpSlider;

    // �ִϸ����� ����
    //Animator anim;

    protected void Awake()
    {
        status = GetComponent<EnemyStatus>();
        cc = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    protected virtual void Idle()
    {
        
    }

    protected virtual void Move()
    {
        
    }

    void Attack()
    {
        
    }

    // ������ ���� �Լ�
    public virtual void HitEnemy(int hitPower)
    {
        // ����, �̹� �ǰ� �����̰ų� ��� ���� �Ǵ� ���� ���¶�� �ƹ��� ó���� ���� �ʰ� �Լ��� �����Ѵ�.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return;
        }

        // �׺���̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
        agent.isStopped = true;
        agent.ResetPath();
    }

    void Damaged()
    {
        // �ǰ� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DamageProcess());
    }

    // ������ ó���� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(1f);

        // ���� ���¸� �̵� ���·� ��ȯ�Ѵ�.
        m_State = EnemyState.Move;
        print("���� ��ȯ: Damaged -> Move");
    }

    // ���� ���� �Լ�
    void Die()
    {
        // �������� �ǰ� �ڷ�ƾ�� �����Ѵ�.
        StopAllCoroutines();

        // ���� ���¸� ó���ϱ� ���� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);
        print("�Ҹ�!");
        Destroy(gameObject);
    }
}
