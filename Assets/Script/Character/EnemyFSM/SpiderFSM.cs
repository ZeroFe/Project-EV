using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class SpiderFSM : EnemyFSM
{
    public override void OnAttackHit()
    {
        AudioManager.Instance.PlaySFX(attackSound, transform.position);
        if (IsInAttackDistance())
        {
            attackTarget.GetComponent<CharacterStatus>().TakeDamage(status.attackPower);
        }
    }
}
