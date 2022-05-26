using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrowerFSM : EnemyFSM
{
    float currentTime = 0;
    float attackDelay = 2f;

    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private Transform shootPosTr;

    private float spearFlightTime = 0.5f;

    public override void OnAttackHit()
    {
        print("Spear Throw");
        // 창 던지기
        var spear = Instantiate(spearPrefab, shootPosTr.position, Quaternion.LookRotation(transform.forward));
        var projectile = spear.GetComponent<SpearProjectile>();
        if (projectile != null)
        {
            projectile.Shoot(CalculateVelocty(attackTarget.transform.position, shootPosTr.position, spearFlightTime));
            projectile.SetDamage(status.attackPower);
        }
        else
        {
            print("projectile is null");
        }
    }

    Vector3 CalculateVelocty(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz / time;
        float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }
}
