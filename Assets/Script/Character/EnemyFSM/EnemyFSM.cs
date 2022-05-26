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

    [SerializeField, Tooltip("이동 속도")]
    protected float moveSpeed = 3.5f;
    [SerializeField, Tooltip("플레이어 등 인식하는 거리")]
    public float findDistance = 4.0f;
    [SerializeField, Tooltip("공격 사거리")]
    protected float attackDistance = 2f;
    protected float realAttackDistance;
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


    protected static readonly int speedNameHash = Animator.StringToHash("Speed");
    protected static readonly int attackNameHash = Animator.StringToHash("Attack");
    protected static readonly int deadNameHash = Animator.StringToHash("Dead");
    protected static readonly int getHitNameHash = Animator.StringToHash("GetHit");

    protected void Awake()
    {
        playerLayer = 1 << LayerMask.NameToLayer("Player");

        status = GetComponent<EnemyStatus>();
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

        MoveSpeed = moveSpeed;
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

    public bool IsInAttackDistance(out RaycastHit hit)
    {
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), 
            out hit, attackDistance, playerLayer);
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

        RaycastHit hit;
        if (IsInAttackDistance(out hit))
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
        RaycastHit hit;
        if (IsInAttackDistance(out hit))
        {
            agent.isStopped = true;
            anim.SetBool(attackNameHash, true);
        }
    }

    public virtual void OnAttackHit()
    {
        RaycastHit hit;
        if (IsInAttackDistance(out hit))
        {
            attackTarget.GetComponent<CharacterStatus>().TakeDamage(status.attackPower);
        }
    }

    public virtual void OnAttackFinished()
    {
        print("OnAttackFinished");
        // 만약 공격 가능 거리를 벗어났다면, 
        RaycastHit hit;
        if (!IsInAttackDistance(out hit))
        {
            state = EnemyState.Move;
            //anim.SetTrigger(spe);
            anim.SetBool(attackNameHash, false);
        }
    }
    #endregion

    public virtual void OnDamaged()
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

    public virtual void OnReactFinished()
    {
        //if (state == EnemyState.Death) return;

        agent.isStopped = false;
        state = EnemyState.Move;
        //anim.SetTrigger("Move");
    }

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
