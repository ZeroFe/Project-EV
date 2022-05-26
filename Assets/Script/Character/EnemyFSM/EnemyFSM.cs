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

    // ���� �߰��� �������� �ý��ۿ��� ������ �� �ִ� �������̹Ƿ� �Ϻη� public���� ����
    [Tooltip("�̵� �ӵ�")]
    public float moveSpeed = 3.5f;

    protected float currentMoveSpeed;
    [Tooltip("�÷��̾� �� �ν��ϴ� �Ÿ�")]
    public float findDistance = 4.0f;
    [Tooltip("���� ��Ÿ�")]
    public float attackDistance = 2f;
    protected float realAttackDistance;

    [Tooltip("�Ұ��� ���� ����")] 
    public float hurtReactRate = 0.15f;
    [Tooltip("����� ���� ����")]
    public float knockbackReactRate = 0.3f;
    //protected float 

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

    // ��Ÿ� �ȿ� ������ ����
    // �ǹ� �� ��ü�� ū ��� Distance�θ� �ϸ� ������ ���ϴ� ��찡 �ִ�
    // ��� �Ÿ� �ȿ� ������ ���Ѵ� �Ǵ��ϸ� ���̸� ���� �Ÿ� ������ �Ѵ�
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
        // ���� ���¸� �ٷ� ���� ��� �����ϰ� �Ѵ�
        if (state == EnemyState.Die)
        {
            anim.SetTrigger(deadNameHash);
            return;
        }

        // ���� ���� ���� �Ÿ��� ����ٸ�, 
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
            // ���� ���¸� ����� Idle ��ȯ ��, �ٷ� �װ� ó��
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
