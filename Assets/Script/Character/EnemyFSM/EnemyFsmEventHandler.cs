using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFsmEventHandler : MonoBehaviour
{
    private EnemyFSM fsm;

    private void Awake()
    {
        fsm = GetComponentInParent<EnemyFSM>();
    }

    public void OnAttackHit()
    {
        fsm.OnAttackHit();
    }

    public void OnAttackFinished()
    {
        fsm.OnAttackFinished();
    }

    public void OnReactFinished()
    {
        fsm.OnReactFinished();
    }

    public void OnDeathFinished()
    {
        fsm.OnDeathFinished();
    }
}
