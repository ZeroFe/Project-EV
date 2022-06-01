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

    public int attackPower = 3;

    [Header("VFX")]
    [SerializeField] private ParticleSystem hitBloodEffectPrefab;
    [SerializeField] private ParticleSystem criticalBloodEffectPrefab;

    public ParticleSystem HitBloodEffect => hitBloodEffectPrefab;
    public ParticleSystem CriticalBloodEffect => criticalBloodEffectPrefab;

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
    /// Enemy�� ��� Object Pool�� �̿��� ����ǹǷ�
    /// OnEnable���� �ʱ�ȭ�ؼ� �� �� �ֵ��� 
    /// </summary>
    private void Init()
    {
        CurrentHp = maxHp;
    }

    void Start()
    {
        // ���� �̺�Ʈ ����
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
