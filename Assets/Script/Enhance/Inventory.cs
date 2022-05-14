using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
public class Inventory : MonoBehaviour
{
    #region Events
    public delegate void AddItemHandler(Enhance enhance);
    #endregion

    public event AddItemHandler OnAddItem;

    // int�� ���� ���� Enhance Code
    public class AddItemEvent : UnityEvent<int> { }
    public AddItemEvent addItemEvent { get; set; } = new AddItemEvent();

    private List<Enhance> enhances = new List<Enhance>();
    public IReadOnlyList<Enhance> Enhances => enhances;


    private void Awake()
    {

    }

    #region Add Enhance
    // ��ȭ �����ϱ�
    public bool AddItem(Enhance newEnhance)
    {
        Debug.Log($"add enhance - {newEnhance.name}");

        enhances.Add(newEnhance);
        //AddEquipAbility(newEnhance);

        RegisterPassive(newEnhance);

        //addItemEvent.Invoke(newEnhance.name.GetHashCode());

        return true;
    }

    private void RegisterPassive(Enhance enhance)
    {
        enhance.Register(gameObject);
    }
    #endregion


    /*
    public int GetEquipAbilityIncreaseSize(EAbility ability)
    {
        return _equipAbilityIncreaseSizeArr[(int)ability];
    }

    /// <summary>
    /// ��� �������� �ջ� �ɷ�ġ �� ȿ�� ����
    /// </summary>
    private void AddEquipAbility(EquipItem equip)
    {
        foreach (var ability in equip.equipAbilities)
        {
            _equipAbilityIncreaseSizeArr[(int)ability.equipEffect] += ability.value;
        }
    }

    public void AddAbility(int value, EAbility type)
    {
        equipAbilityIncreaseSizeArrPublic[(int)type] += value;
    }

    private void DeleteEquipAbility(EquipItem equip)
    {
        foreach (var ability in equip.equipAbilities)
        {
            _equipAbilityIncreaseSizeArr[(int)ability.equipEffect] -= ability.value;
        }
    }
    */
}