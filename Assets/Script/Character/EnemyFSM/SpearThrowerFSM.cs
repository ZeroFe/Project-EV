using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrowerFSM : EnemyFSM
{
    float currentTime = 0;
    float attackDelay = 2f;

    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private Transform shootPosTr;
    [SerializeField]
    private float spearFlightTime = 0.5f;

    public override void OnAttackHit()
    {
        // 창 던지기
        var spear = Instantiate(spearPrefab, shootPosTr.position, Quaternion.LookRotation(transform.forward));
        var projectile = spear.GetComponent<SpearProjectile>();
        if (projectile != null)
        {
            projectile.Shoot(Vec3Parabola.CalculateVelocity(attackTarget.transform.position, shootPosTr.position, spearFlightTime));
            projectile.SetDamage(status.attackPower);
        }
        else
        {
            print("projectile is null");
        }
    }
}
