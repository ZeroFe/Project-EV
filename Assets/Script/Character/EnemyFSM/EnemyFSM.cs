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
    // ���ʹ� Player Layer�� �����Ѵ�
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

    [SerializeField, Tooltip("�̵� �ӵ�")]
    protected float moveSpeed = 3.5f;
    [SerializeField, Tooltip("�÷��̾� �� �ν��ϴ� �Ÿ�")]
    public float findDistance = 4.0f;
    [SerializeField, Tooltip("���� ��Ÿ�")]
    protected float attackDistance = 2f;
    protected float realAttackDistance;
    [SerializeField, Tooltip("��ü ������� �ӵ�")]
    public float deathTime = 0.5f;

    // ��� ���ʹ� �⺻������ ������ �����ؾ� �Ѵ�
    protected GameObject originTarget;
    // ���� ��� : �ν� ������ ���� ���� ����� ����ǹǷ� ���� �����Ѵ�
    protected GameObject attackTarget;

    protected EnemyStatus status;
    protected NavMeshAgent agent;
    protected Animator anim;

    private Collider hitCollider;

    // ���� �� Fade Out ���� ó�� ����
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

        // ���� ��� ó��

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
        // �÷��̾ ��ó�� �ִ��� Ž��
        // ���� �ִٸ� ���� ����� �÷��̾�� �ٲ۴�
        Collider[] cols = Physics.OverlapSphere(transform.position, findDistance, playerLayer);
        if (cols.Length > 0)
        {
            float minDistance = Single.MaxValue;
            // ���� ����� ����� �����Ѵ�
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
        // ��ó�� ������ �ٽ� ���� ����
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
        // ���� ���� ���� �Ÿ��� ����ٸ�, 
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
    /// ������ Ÿ�ݵǼ��� �� �ǰ�, ����� �Ѵ�
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
