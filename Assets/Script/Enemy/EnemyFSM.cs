using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

[RequireComponent(typeof(EnemyStatus))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyFSM : MonoBehaviour
{
    public enum FirstTarget
    {
        Player,
        Base,
        None
    }

    public FirstTarget firstTarget;

    protected GameObject target;
    protected Transform targetTr;

    protected EnemyStatus status;
    protected NavMeshAgent agent;
    protected CharacterController cc;
    //protected Animator anim;

    // 죽을 때 Fade Out 투명도 처리 예정
    [SerializeField]
    protected Renderer renderer;
    protected Material mat;

    protected void Awake()
    {
        status = GetComponent<EnemyStatus>();
        cc = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();

        Debug.Assert(renderer, "Error : There is no Renderer");
        mat = renderer.material;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        targetTr = target.transform;
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

    // 데미지 실행 함수
    public virtual void HitEnemy(int hitPower)
    {

    }

    void Damaged()
    {
    }

    // 죽음 상태 함수
    public virtual void Die()
    {
        mat.DOFade(0.0f, 0.5f);
    }
}
