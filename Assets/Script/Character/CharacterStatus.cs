using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

/// <summary>
/// ĳ���� �ɷ�ġ Ŭ����
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
                    onDead?.Invoke();
                }
            }
            else if (_currentHp > maxHp)
            {
                _currentHp = maxHp;
            }
            onHpChanged?.Invoke(_currentHp, maxHp);
        }
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            // �ִ� ü���� �þ�� ü���� �������� ����
            maxHp = value;
            Debug.Assert(maxHp > 0, "Error - Max Hp is lower than 0");
            onHpChanged?.Invoke(_currentHp, maxHp);
        }
    }

    public event HpChangedHandler onHpChanged;
    public event Action onDead;
    public event Action<int> onDamaged;
    public event Action<int> onHealed;

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
        onDamaged?.Invoke(amount);
    }

    public void TakeHeal(int amount)
    {
        CurrentHp += amount;
        onHealed?.Invoke(amount);
    }
}
