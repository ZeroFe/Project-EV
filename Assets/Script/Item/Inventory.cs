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
    public delegate void AddItemHandler(Item item);
    #endregion

    public event AddItemHandler OnAddItem;

    // int�� ���� ���� Item Code
    public class AddItemEvent : UnityEvent<int> { }
    public AddItemEvent addItemEvent { get; set; } = new AddItemEvent();


    public UnityEvent UseConsumableEvent { get; set; } = new UnityEvent();

    public List<EquipItem> EquipItems { get; private set; } = new List<EquipItem>();

    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }
    private int _gold;

    private int[] _equipAbilityIncreaseSizeArr = new int[Enum.GetValues(typeof(EAbility)).Length];
    public int[] equipAbilityIncreaseSizeArrPublic = new int[Enum.GetValues(typeof(EAbility)).Length];

    private void Awake()
    {

    }

    #region Add Item
    public bool AddItem(Item newItem)
    {
        if (newItem is EquipItem)
        {
            return AddEquip(newItem as EquipItem);
        }
        else if (newItem is ConsumableItem)
        {
            return UseConsumableItem(newItem as ConsumableItem);
        }
        else
        {
            Debug.Log("Can't Add Unknown Type Item");
            return false;
        }
    }

    /// <summary>
    /// ���濡 ��� ������ �ֱ�
    /// </summary>
    /// <returns>������ �ֱ� �����ߴ���</returns>
    private bool AddEquip(EquipItem newEquip)
    {
        Debug.Log($"add item - {newEquip.name}");

        EquipItems.Add(newEquip);
        AddEquipAbility(newEquip);

        RegisterPassive(newEquip);

        addItemEvent.Invoke(newEquip.name.GetHashCode());

        return true;
    }

    private void RegisterPassive(EquipItem equip)
    {
        equip.Register(gameObject);
    }

    /// <summary>
    /// �Һ� �������� ����
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool UseConsumableItem(ConsumableItem item)
    {
        // �Һ� ������ ȹ�� �� �̺�Ʈ �߻�

        
        // �Һ� ������ ����
        foreach (var useEffectReference in item.consumeEffects)
        {
            useEffectReference.GetReference().TakeUseEffect();
        }

        return true;
    }
    #endregion

    #region EquipAbility
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
    #endregion

    private void UseConsumable(ConsumableItem consumable)
    {
        //_inventoryUser.GetComponent<CPlayerPara>().TakeUseEffectHandleList(consumable.UseEffectList, _inventoryUser);
        UseConsumableEvent?.Invoke();
    }
}