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

    public int attackPower = 3;

    [Header("VFX")]
    [SerializeField] private GameObject hitBloodEffectPrefab;
    [SerializeField] private GameObject criticalBloodEffectPrefab;

    public GameObject HitBloodEffect => hitBloodEffectPrefab;
    public GameObject CriticalBloodEffect => criticalBloodEffectPrefab;

    private void Awake()
    {
        int size = 10;
        if (hitBloodEffectPrefab != null)
        {
            PoolSystem.Instance.InitPool(hitBloodEffectPrefab, size);
        }

        if (criticalBloodEffectPrefab != null)
        {
            PoolSystem.Instance.InitPool(criticalBloodEffectPrefab, size);
        }
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
        // 공통 이벤트 설정
        OnDamaged += (int amount) => EnemyManager.Instance.InvokeEnemyDamaged(gameObject, amount);
        OnDead += () => EnemyManager.Instance.InvokeEnemyDead(gameObject);
    }

    public void PowerUp(float powerupRate)
    {
        MaxHp = (int) (MaxHp * powerupRate);
        CurrentHp = MaxHp;
        attackPower = (int) (attackPower * powerupRate);
    }
}
