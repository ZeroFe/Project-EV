using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class SpiderFSM : EnemyFSM
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    private EnemyState state = EnemyState.Idle;

    [SerializeField]
    private float moveSpeed = 3.5f;
    public float attackDistance = 2f;
    float currentTime = 0;
    float attackDelay = 2f;
    public float deathTime = 0.5f;

    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
            anim.SetFloat(speedNameHash, moveSpeed);
            agent.speed = moveSpeed;
        }
    }

    private static readonly int speedNameHash = Animator.StringToHash("Speed");
    private static readonly int attackNameHash = Animator.StringToHash("Attack");
    private static readonly int deadNameHash = Animator.StringToHash("Dead");
    private static readonly int getHitNameHash = Animator.StringToHash("GetHit");


    void Start()
    {
        state = EnemyState.Idle;

        MoveSpeed = moveSpeed;
        agent.stoppingDistance = attackDistance;

        // 죽음 모션 처리
        status.OnDead += OnDead;

        StartCoroutine(UpdateState());
    }

    IEnumerator UpdateState()
    {
        while (true)
        {
            switch (state)
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
                default:
                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    void Idle()
    {
        if (Vector3.Distance(transform.position, targetTr.position) > attackDistance)
        {
            state = EnemyState.Move;
        }
    }


    void Move()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < agent.stoppingDistance)
        {
            state = EnemyState.Attack;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
        }
    }

    #region Attack
    void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) >= agent.stoppingDistance)
        {
            state = EnemyState.Move;
            return;
        }

        agent.isStopped = true;
        anim.SetBool(attackNameHash, true);
        //if (!isAttack)
        //{
        //    isAttack = true;
        //}
    }

    public void OnAttackHit()
    {
        print("OnSpearThrow");
        if (Vector3.Distance(transform.position, target.transform.position) < attackDistance)
        {
            target.GetComponent<CharacterStatus>().TakeDamage(status.attackPower);
        }
    }

    public void OnAttackFinished()
    {
        print("OnAttackFinished");
        // 만약 공격 가능 거리를 벗어났다면, 
        if (Vector3.Distance(transform.position, target.transform.position) >= agent.stoppingDistance)
        {
            state = EnemyState.Move;
            //anim.SetTrigger(spe);
            anim.SetBool(attackNameHash, false);
        }
    }

    #endregion

    internal void OnDamaged()
    {
        print("Damaged Animation");
        agent.isStopped = true;
        anim.SetTrigger("React");
        //StopCoroutine(Flash());
        //StartCoroutine(Flash());
    }

    //IEnumerator Flash()
    //{
        //for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        //{
        //    skinnedMeshRenderers[i].material = matEnemy;
        //}

        //yield return new WaitForSeconds(0.2f);

        //for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        //{
        //    skinnedMeshRenderers[i].material = prevMaterials[i];
        //}
    //}

    public void OnReactFinished()
    {
        //if (state == EnemyState.Death) return;

        agent.isStopped = false;
        state = EnemyState.Move;
        //anim.SetTrigger("Move");
    }

    protected override void OnDead()
    {
        base.OnDead();
        state = EnemyState.Die;
        print("Death Animation");
        anim.SetTrigger(deadNameHash);
    }

    public void OnDeathFinished()
    {
        print("Death Finished");
        agent.isStopped = false;
        gameObject.SetActive(false);
    }
}
