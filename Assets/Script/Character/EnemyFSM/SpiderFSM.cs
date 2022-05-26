using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class SpiderFSM : EnemyFSM
{
    float currentTime = 0;
    float attackDelay = 2f;

    public override void OnAttackHit()
    {
        RaycastHit hit;
        if (IsInAttackDistance())
        {
            attackTarget.GetComponent<CharacterStatus>().TakeDamage(status.attackPower);
        }
    }
}
