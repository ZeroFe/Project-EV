using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

/// <summary>
/// 캐릭터 능력치 클래스
/// 
/// </summary>
public class CharacterStatus : MonoBehaviour
{
    public delegate void HpChangedHandler(int current, int max);

    [SerializeField]
    protected int maxHp = 100;
    protected int _currentHp;

    protected bool isDead = false;

    public int CurrentHp
    {
        get => _currentHp;
        set
        {
            _currentHp = value;
            if (_currentHp <= 0)
            {
                _currentHp = 0;
                if (!isDead)
                {
                    isDead = true;
                    OnDead?.Invoke();
                }
            }
            else if (_currentHp > maxHp)
            {
                _currentHp = maxHp;
            }
            OnHpChanged?.Invoke(_currentHp, maxHp);
        }
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            // 최대 체력이 늘어나면 증감량만큼 현재체력 증가
            int changedAmount = Math.Max(value - maxHp, 0);
            maxHp = value;
            CurrentHp += changedAmount;
            Debug.Assert(maxHp > 0, "Error - Max Hp is lower than 0");
            OnHpChanged?.Invoke(_currentHp, maxHp);
        }
    }

    public event HpChangedHandler OnHpChanged;
    public event Action OnDead;
    public event Action<int> OnDamaged;
    public event Action<int> OnHealed;

    private void OnEnable()
    {
        _currentHp = maxHp;
        isDead = false;
    }

    void Update()
    {

    }

    public void TakeDamage(int amount)
    {
        CurrentHp -= amount;
        OnDamaged?.Invoke(amount);
    }

    public void TakeHeal(int amount)
    {
        CurrentHp += amount;
        OnHealed?.Invoke(amount);
    }
}
