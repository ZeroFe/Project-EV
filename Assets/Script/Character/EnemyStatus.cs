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

    private Collider hitCollider;

    private void Awake()
    {
        hitCollider = GetComponent<Collider>();
        Debug.Assert(hitCollider, "There is no Collider");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        hitCollider.enabled = true;
        OnDead += RoundSystem.Instance.CheckRoundEnd;
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

    public void PowerUp(float powerupRate)
    {
        MaxHp = (int) (MaxHp * powerupRate);
        CurrentHp = MaxHp;
        attackPower = (int) (attackPower * powerupRate);
    }

    private void DeactiveCollider()
    {
        hitCollider.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnDamaged += (int amount) => EnemyManager.Instance.DrawDamaged(gameObject, amount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
