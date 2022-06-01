using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    public float attackPower = 10.0f;

    [SerializeField, Tooltip("위기 상태에 해당하는 체력 비율"), Range(0.01f, 1.0f)] 
    private float crisisHpRate = 0.3f;
    [SerializeField] private AudioClip crisisHeartbeatSound;
    private bool isCrisis = false;

    public event Action onEnterCrisis;
    public event Action onExitCrisis;

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (!isCrisis && (float)currentHp / maxHp <= crisisHpRate)
        {
            isCrisis = true;
            onEnterCrisis?.Invoke();
        }
    }
    public override void TakeHeal(int amount)
    {
        base.TakeHeal(amount);
        if (isCrisis && (float)currentHp / maxHp > crisisHpRate)
        {
            isCrisis = false;
            onExitCrisis?.Invoke();
        }
    }
}
