using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEndEvent : PassiveCondition
{
    public override void Register(GameObject target)
    {
        target.GetComponent<PlayerCtrl>().OnDashEnd += Execute;
    }

    private void Execute()
    {
        ExecuteEquipPassive(1);
    }
}
