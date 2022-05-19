using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnhance : EnhanceEffect
{
    [SerializeField] 
    private int hpIncreaseAmount = 0;

    public override void ApplyEnhance(GameObject target)
    {
        var playerStatus = target.GetComponent<PlayerStatus>();
        Debug.Assert(playerStatus, $"Error - PlayerStatus not find in {target.name}");

        playerStatus.MaxHp += hpIncreaseAmount;
    }
}
