using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAttacker : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    private EnemyState m_State;

    Transform targetTr;

    public float attackDistance = 2f;
    float currentTime = 0;
    float attackDelay = 2f;
    public int attackPower = 3;

    public int hp = 15;

    private NavMeshAgent agent;
    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Transform target)
    {
        targetTr = target;
    }

    void Start()
    {
        // ������ ���ʹ� ���´� ��� ����(Idle)�� �Ѵ�.
        m_State = EnemyState.Idle;

        // �ڽ� ������Ʈ�κ��� �ִϸ����� ���� �޾ƿ���
        //anim = transform.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // ���� ���¸� üũ�Ͽ� �ش� ���º��� ������ ����� �����ϰ� �ϰ� �ʹ�.
        switch (m_State)
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
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }

        // ���� hp(%)�� hp �����̴��� value�� �ݿ��Ѵ�.
        //hpSlider.value = (float)hp / (float)maxHp;
    }

    void Idle()
    {
        if (Vector3.Distance(transform.position, targetTr.position) > attackDistance)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ: Idle -> Move");
        }
    }

    void Move()
    {
        // ����, �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̾ ���� �̵��Ѵ�.
        if (Vector3.Distance(transform.position, targetTr.position) > attackDistance)
        {
            // �׺���̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� �����Ѵ�.
            agent.stoppingDistance = attackDistance;

            // �׺���̼� �������� �÷��̾��� ��ġ�� �����Ѵ�.
            agent.destination = targetTr.position;
        }
        // �׷��� �ʴٸ�, ���� ���¸� Attack ���·� ��ȯ�Ѵ�.
        else
        {
            m_State = EnemyState.Attack;
            print("���� ��ȯ: Move -> Attack");

            // ���� �ð��� ���� ������ �ð���ŭ �̸� ������ѳ��´�.
            currentTime = attackDelay;

            // ���� ��� �ִϸ��̼� �÷���
            //anim.SetTrigger("MoveToAttackDelay");

            // �׺���̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    void Attack()
    {
        // ����, �÷��̾ ���� ���� �̳��� �ִٸ� �÷��̾ �����Ѵ�.
        if (Vector3.Distance(transform.position, targetTr.position) < attackDistance)
        {
            // ������ �ð����� �÷��̾ �����Ѵ�.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                // player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                currentTime = 0;

                // ���� �ִϸ��̼� �÷���
                //anim.SetTrigger("StartAttack");
            }
        }
        // �׷��� �ʴٸ�, ���� ���¸� Move ���·� ��ȯ�Ѵ�(�� �߰� �ǽ�).
        else
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ: Attack -> Move");
            currentTime = 0;

            // �̵� �ִϸ��̼� �÷���
            //anim.SetTrigger("AttackToMove");
        }
    }

    // �÷��̾��� ��ũ��Ʈ�� ������ ó�� �Լ��� �����ϱ�
    public void AttackAction()
    {
        //player.GetComponent<PlayerCtrl>().DamageAction(attackPower);
    }

    // ������ ���� �Լ�
    public void HitEnemy(int hitPower)
    {
        // ����, �̹� �ǰ� �����̰ų� ��� ���� �Ǵ� ���� ���¶�� �ƹ��� ó���� ���� �ʰ� �Լ��� �����Ѵ�.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return;
        }

        // �÷��̾��� ���ݷ¸�ŭ ���ʹ��� ü���� ���ҽ�Ų��.
        //hp -= hitPower;

        // �׺���̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
        agent.isStopped = true;
        agent.ResetPath();

        // ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ�Ѵ�.
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ: Any state -> Damaged");

            // �ǰ� �ִϸ��̼��� �÷����Ѵ�.
            //anim.SetTrigger("Damaged");
            Damaged();
        }
        // �׷��� �ʴٸ�, ���� ���·� ��ȯ�Ѵ�.
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ: Any state -> Die");

            // ���� �ִϸ��̼��� �÷����Ѵ�.
            //anim.SetTrigger("Die");
            Die();
        }
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
        // ĳ���� ��Ʈ�ѷ� ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
        cc.enabled = false;

        // 2�� ���� ��ٸ� �ڿ� �ڱ� �ڽ��� �����Ѵ�.
        yield return new WaitForSeconds(2f);
        print("�Ҹ�!");
        Destroy(gameObject);
    }
}
