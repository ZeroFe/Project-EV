using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유저가 공격을 실행했을 때 발동되는 이벤트
/// </summary>
[CreateAssetMenu(fileName = "New User Attack Event", menuName = "Enhance/Passive Condition/UserAttackEvent")]
public class UserAttackEvent : PassiveCondition
{
    public override void Register(GameObject target)
    {
        // Test Code : 유저 행동 클래스가 있다고 가정하고 제작
        target.GetComponent<TestPlayer>().onJump += Execute;
    }

    private void Execute()
    {
        ExecuteEquipPassive(1);
    }
}
