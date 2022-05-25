using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 적 능력치를 관리하는 클래스
/// EnemyManager와 연계하여 이벤트를 호출한다
/// </summary>
[RequireComponent(typeof(Collider))]
public class EnemyStatus : CharacterStatus
{
    //public delegate void Damage

    [FormerlySerializedAs("damage")] public int attackPower = 3;

    private void Awake()
    {
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    protected void OnDisable()
    {
        
    }

    /// <summary>
    /// Enemy의 경우 Object Pool을 이용해 재사용되므로
    /// OnEnable에서 초기화해서 쓸 수 있도록 
    /// </summary>
    private void Init()
    {
        CurrentHp = maxHp;
    }

    void Start()
    {
        OnDamaged += (int amount) => EnemyManager.Instance.DrawDamaged(gameObject, amount);
        OnDead += RoundSystem.Instance.CheckRoundEnd;
    }

    void Update()
    {

    }

    public void PowerUp(float powerupRate)
    {
        MaxHp = (int) (MaxHp * powerupRate);
        CurrentHp = MaxHp;
        attackPower = (int) (attackPower * powerupRate);
    }
}
