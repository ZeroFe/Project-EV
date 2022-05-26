using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrowerFSM : EnemyFSM
{
    float currentTime = 0;
    float attackDelay = 2f;

    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private Transform shootPosTr;

    public override void OnAttackHit()
    {
        print("Spear Throw");
        // 창 던지기
        var spear = Instantiate(spearPrefab, shootPosTr.position, Quaternion.LookRotation(transform.forward));
        var projectile = spear.GetComponent<SpearProjectile>();
        if (projectile != null)
        {
            projectile.Shoot(transform.forward * 10);
            projectile.SetDamage(status.attackPower);
        }
        else
        {
            print("projectile is null");
        }
    }
}
