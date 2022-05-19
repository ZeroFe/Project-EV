using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAttacker : EnemyFSM
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

    public float attackDistance = 2f;
    float currentTime = 0;
    float attackDelay = 2f;

    void Start()
    {
        // 최초의 에너미 상태는 대기 상태(Idle)로 한다.
        m_State = EnemyState.Idle;

        // 자식 오브젝트로부터 애니메이터 변수 받아오기

        status.onDead += Die;
    }

    void Update()
    {
        // 현재 상태를 체크하여 해당 상태별로 정해진 기능을 수행하게 하고 싶다.
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
    }

    void Idle()
    {
        if (Vector3.Distance(transform.position, targetTr.position) > attackDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");
        }
    }

    void Move()
    {
        // 만일, 플레이어와의 거리가 공격 범위 밖이라면 플레이어를 향해 이동한다.
        if (Vector3.Distance(transform.position, targetTr.position) > attackDistance)
        {
            // 네비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정한다.
            agent.stoppingDistance = attackDistance;

            // 네비게이션 목적지를 플레이어의 위치로 설정한다.
            agent.destination = targetTr.position;
        }
        // 그렇지 않다면, 현재 상태를 Attack 상태로 전환한다.
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");

            // 누적 시간을 공격 딜레이 시간만큼 미리 진행시켜놓는다.
            currentTime = attackDelay;

            // 공격 대기 애니메이션 플레이
            //anim.SetTrigger("MoveToAttackDelay");

            // 네비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    void Attack()
    {
        // 만일, 플레이어가 공격 범위 이내에 있다면 플레이어를 공격한다.
        if (Vector3.Distance(transform.position, targetTr.position) < attackDistance)
        {
            // 일정한 시간마다 플레이어를 공격한다.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                AttackAction();
                print("공격");
                currentTime = 0;

                // 공격 애니메이션 플레이
                //anim.SetTrigger("StartAttack");
            }
        }
        // 그렇지 않다면, 현재 상태를 Move 상태로 전환한다(재 추격 실시).
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = 0;

            // 이동 애니메이션 플레이
            //anim.SetTrigger("AttackToMove");
        }
    }

    // 플레이어의 스크립트의 데미지 처리 함수를 실행하기
    public void AttackAction()
    {
        target.GetComponent<CharacterStatus>().TakeDamage(status.attackPower);
    }

    // 데미지 실행 함수
    public void HitEnemy(int hitPower)
    {
        // 만일, 이미 피격 상태이거나 사망 상태 또는 복귀 상태라면 아무런 처리도 하지 않고 함수를 종료한다.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return;
        }

        // 네비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
        agent.isStopped = true;
        agent.ResetPath();

        // 에너미의 체력이 0보다 크면 피격 상태로 전환한다.
        if (status.CurrentHp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");

            // 피격 애니메이션을 플레이한다.
            //anim.SetTrigger("Damaged");
            Damaged();
        }
        // 그렇지 않다면, 죽음 상태로 전환한다.
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");

            // 죽음 애니메이션을 플레이한다.
            //anim.SetTrigger("Die");
            Die();
        }
    }

    void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DamageProcess());
    }

    // 데미지 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간만큼 기다린다.
        yield return new WaitForSeconds(1f);

        // 현재 상태를 이동 상태로 전환한다.
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }

    // 죽음 상태 함수
    public override void Die()
    {
        // 진행중인 피격 코루틴을 중지한다.
        StopAllCoroutines();

        // 죽음 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 콘트롤러 컴포넌트를 비활성화한다.
        cc.enabled = false;

        // 2초 동안 기다린 뒤에 자기 자신을 제거한다.
        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);
    }
}
