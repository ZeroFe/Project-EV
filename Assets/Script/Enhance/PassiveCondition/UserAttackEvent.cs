using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ������ �������� �� �ߵ��Ǵ� �̺�Ʈ
/// </summary>
[CreateAssetMenu(fileName = "New User Attack Event", menuName = "Enhance/Passive Condition/UserAttackEvent")]
public class UserAttackEvent : PassiveCondition
{
    public override void Register(GameObject target)
    {
        // Test Code : ���� �ൿ Ŭ������ �ִٰ� �����ϰ� ����
    }

    private void Execute()
    {
        ExecuteEquipPassive(1);
    }
}
