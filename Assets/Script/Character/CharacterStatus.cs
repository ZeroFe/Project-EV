using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    protected int _currentHp = 100;

    public int CurrentHp
    {
        get => _currentHp;
        set
        {
            _currentHp = value;
            if (_currentHp <= 0)
            {
                _currentHp = 0;
                onDead?.Invoke();
            }
            else if (_currentHp > maxHp)
            {
                _currentHp = maxHp;
            }
            onHpChanged?.Invoke(_currentHp, maxHp);
        }
    }

    public event HpChangedHandler onHpChanged;
    public event Action onDead;
    public event Action<int> onDamaged;
    public event Action<int> onHealed;

    void Update()
    {

    }

    public void TakeDamage(int amount)
    {
        CurrentHp -= amount;
        onDamaged?.Invoke(amount);
    }

    public void TakeHeal(int amount)
    {
        CurrentHp += amount;
        onHealed?.Invoke(amount);
    }
}
