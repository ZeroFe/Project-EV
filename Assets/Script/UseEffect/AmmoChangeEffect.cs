using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoChangeEffect : UseEffect
{
    [SerializeField] 
    private int addedAmmoAmount = 0;

    public override void TakeUseEffect(GameObject sender, GameObject target)
    {
        var weapon = target.GetComponent<PlayerCtrl>().PlayerWeapon;
        Debug.Assert(weapon, $"Error - {target.name} has not weapon");

        weapon.CurrentAmmoCount += addedAmmoAmount;
    }
}
