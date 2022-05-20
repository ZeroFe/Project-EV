using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnhance : PassiveCondition
{
    public override void Register(GameObject target)
    {
        target.GetComponent<Inventory>().OnAddItem += Execute;
    }

    private void Execute(string enhanceCodeName)
    {
        ExecuteEquipPassive(enhanceCodeName.GetHashCode());
    }
}
