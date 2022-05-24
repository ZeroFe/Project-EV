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

    private Collider hitCollider;

    // ���� �� Fade Out ���� ó�� ����
    [SerializeField]
    protected Renderer renderer;
    protected Material mat;

    protected void Awake()
    {
        status = GetComponent<EnemyStatus>();
        status.OnDead += OnDead;

        cc = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();

        hitCollider = GetComponent<Collider>();
        Debug.Assert(hitCollider, "There is no Collider");

        Debug.Assert(renderer, "Error : There is no Renderer");
        mat = renderer.material;
    }

    protected virtual void OnEnable()
    {
        hitCollider.enabled = true;
    }

    public void Init(GameObject target, GameObject route)
    {
        this.target = target;
        targetTr = target.transform;

        transform.position = route.transform.position;
        agent.Warp(transform.position);
    }

    /// <summary>
    /// ������ Ÿ�ݵǼ��� �� �ǰ�, ����� �Ѵ�
    /// </summary>
    protected virtual void OnDead()
    {
        agent.isStopped = true;
        hitCollider.enabled = false;
    }
}
