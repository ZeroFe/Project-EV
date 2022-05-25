using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrowerFsmEventHandler : MonoBehaviour
{
    private SpearThrowerFSM fsm;

    private void Awake()
    {
        fsm = GetComponentInParent<SpearThrowerFSM>();
    }

    public void OnSpearThrow()
    {
        //fsm.OnSpearThrow();
    }

    //public void OnAttackFinished()
    //{
    //    fsm.OnAttackFinished();
    //}

    //public void OnReactFinished()
    //{
    //    fsm.OnReactFinished();
    //}

    public void OnDeathFinished()
    {
        //fsm.OnDeathFinished();
    }
}
