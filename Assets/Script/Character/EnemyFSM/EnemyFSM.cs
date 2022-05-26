using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

[RequireComponent(typeof(EnemyStatus))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyFSM : MonoBehaviour
{
    // 몬스터는 Player Layer만 공격한다
    protected static int playerLayer;
    protected enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damaged,
        Die
    }

    public enum FirstTarget
    {
        Player,
        Base,
        None
    }

    protected EnemyState state = EnemyState.Idle;

    public FirstTarget firstTarget;

    // 차후 추가될 돌연변이 시스템에서 변경할 수 있는 변수들이므로 일부러 public으로 선언
    [Tooltip("이동 속도")]
    public float moveSpeed = 3.5f;

    protected float currentMoveSpeed;
    [Tooltip("플레이어 등 인식하는 거리")]
    public float findDistance = 4.0f;
    [Tooltip("공격 사거리")]
    public float attackDistance = 2f;
    protected float realAttackDistance;

    [Tooltip("소경직 반응 비율")] 
    public float hurtReactRate = 0.15f;
    [Tooltip("대경직 반응 비율")]
    public float knockbackReactRate = 0.3f;
    //protected float 

    [SerializeField, Tooltip("시체 사라지는 속도")]
    public float deathTime = 0.5f;

    // 모든 몬스터는 기본적으로 기지를 공격해야 한다
    protected GameObject originTarget;
    // 공격 대상 : 인식 범위에 들어가면 공격 대상이 변경되므로 따로 저장한다
    protected GameObject attackTarget;

    protected EnemyStatus status;
    protected NavMeshAgent agent;
    protected Animator anim;

    private Collider hitCollider;

    // 죽을 때 Fade Out 투명도 처리 예정
    [SerializeField]
    protected Renderer renderer;
    protected Material mat;

    public float CurrentMoveSpeed
    {
        get => currentMoveSpeed;
        set
        {
            currentMoveSpeed = value;
            anim.SetFloat(speedNameHash, currentMoveSpeed);
            agent.speed = moveSpeed;
        }
    }

    protected static readonly int speedNameHash = Animator.StringToHash("Speed");
    protected static readonly int attackNameHash = Animator.StringToHash("Attack");
    protected static readonly int deadNameHash = Animator.StringToHash("Dead");
    protected static readonly int hurtNameHash = Animator.StringToHash("Hurt");
    protected static readonly int knockbackNameHash = Animator.StringToHash("Knockback");

    protected void Awake()
    {
        playerLayer = 1 << LayerMask.NameToLayer("Player");

        status = GetComponent<EnemyStatus>();
        status.OnDamaged += OnDamaged;
        status.OnDead += OnDead;

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        hitCollider = GetComponent<Collider>();
        Debug.Assert(hitCollider, "There is no Collider");

        //Debug.Assert(renderer, "Error : There is no Renderer");
        //mat = renderer.material;
    }

    protected virtual void OnEnable()
    {
        hitCollider.enabled = true;

        state = EnemyState.Idle;

        CurrentMoveSpeed = moveSpeed;
        agent.stoppingDistance = attackDistance;

        // 죽음 모션 처리

        StartCoroutine(UpdateState());
    }

    public void Init(GameObject target, GameObject route)
    {
        originTarget = target;
        attackTarget = target;

        transform.position = route.transform.position;
        agent.Warp(transform.position);
    }

    // 사거리 안에 들어가는지 판정
    // 건물 등 물체가 큰 경우 Distance로만 하면 공격을 못하는 경우가 있다
    // 고로 거리 안에 들어오지 못한다 판단하면 레이를 쏴서 거리 보정을 한다
    public bool IsInAttackDistance()
    {
        RaycastHit hit;
        return Vector3.Distance(transform.position, attackTarget.transform.position) < attackDistance || 
               Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, attackDistance, playerLayer);
    }


    IEnumerator UpdateState()
    {
        yield return new WaitForSeconds(0.1f);
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
        if (Vector3.Distance(transform.position, originTarget.transform.position) > attackDistance)
        {
            attackTarget = originTarget;
            state = EnemyState.Move;
        }
    }

    void Move()
    {
        // 플레이어가 근처에 있는지 탐색
        // 만약 있다면 공격 대상을 플레이어로 바꾼다
        Collider[] cols = Physics.OverlapSphere(transform.position, findDistance, playerLayer);
        if (cols.Length > 0)
        {
            float minDistance = Single.MaxValue;
            // 가장 가까운 대상을 공격한다
            for (int i = 0; i < cols.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, cols[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    attackTarget = cols[i].gameObject;
                }
            }
        }
        // 근처에 없으면 다시 기지 공격
        else
        {
            attackTarget = originTarget;
        }

        if (IsInAttackDistance())
        {
            print("In attack Distance");
            state = EnemyState.Attack;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(attackTarget.transform.position);
        }
    }

    #region Attack
    void Attack()
    {
        if (IsInAttackDistance())
        {
            agent.isStopped = true;
            anim.SetBool(attackNameHash, true);
        }
    }

    public virtual void OnAttackHit()
    {
        RaycastHit hit;
        if (IsInAttackDistance())
        {
            attackTarget.GetComponent<CharacterStatus>().TakeDamage(status.attackPower);
        }
    }

    public virtual void OnAttackFinished()
    {
        // 죽은 상태면 바로 죽음 모션 수행하게 한다
        if (state == EnemyState.Die)
        {
            anim.SetTrigger(deadNameHash);
            return;
        }

        // 만약 공격 가능 거리를 벗어났다면, 
        RaycastHit hit;
        if (!IsInAttackDistance())
        {
            state = EnemyState.Move;
            //anim.SetTrigger(spe);
            anim.SetBool(attackNameHash, false);
        }
    }
    #endregion

    #region Hit Reaction
    public virtual void OnDamaged(int amount)
    {
        float lostHpRate = (float) amount / status.MaxHp;
        if (lostHpRate > hurtReactRate)
        {
            print("Damaged Animation");
            agent.isStopped = true;
            if (lostHpRate > knockbackReactRate)
            {
                anim.SetTrigger(knockbackNameHash);
            }
            else
            {
                anim.SetTrigger(hurtNameHash);
            }
        }
        
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

    public virtual void OnReactFinished()
    {
        if (state == EnemyState.Die)
        {
            // 죽은 상태면 모션은 Idle 전환 후, 바로 죽게 처리
            print("already die");
            anim.SetBool(attackNameHash, false);
            CurrentMoveSpeed = 0.0f;
            anim.SetTrigger(deadNameHash);
            return;
        }

        agent.isStopped = false;
        state = EnemyState.Move;
    }
    #endregion

    /// <summary>
    /// 죽으면 타격되서도 안 되고, 멈춰야 한다
    /// </summary>
    protected virtual void OnDead()
    {
        print("Enemy FSM On Dead");
        agent.isStopped = true;
        hitCollider.enabled = false;
        state = EnemyState.Die;
        print("Death Animation");
        anim.SetTrigger(deadNameHash);
    }

    public virtual void OnDeathFinished()
    {
        print("Death Finished");
        agent.isStopped = false;
        gameObject.SetActive(false);
    }
}
