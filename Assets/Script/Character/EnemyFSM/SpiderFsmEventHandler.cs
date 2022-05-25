using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFsmEventHandler : MonoBehaviour
{
    private SpiderFSM fsm;

    private void Awake()
    {
        fsm = GetComponentInParent<SpiderFSM>();
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
