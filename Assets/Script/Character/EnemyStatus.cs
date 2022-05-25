using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// �� �ɷ�ġ�� �����ϴ� Ŭ����
/// EnemyManager�� �����Ͽ� �̺�Ʈ�� ȣ���Ѵ�
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
    /// Enemy�� ��� Object Pool�� �̿��� ����ǹǷ�
    /// OnEnable���� �ʱ�ȭ�ؼ� �� �� �ֵ��� 
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
