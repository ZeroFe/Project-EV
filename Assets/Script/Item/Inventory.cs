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

    // int에 들어가는 값은 Item Code
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
    /// 가방에 장비 아이템 넣기
    /// </summary>
    /// <returns>아이템 넣기 성공했는지</returns>
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
    /// 소비 아이템을 적용
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool UseConsumableItem(ConsumableItem item)
    {
        // 소비 아이템 획득 시 이벤트 발생

        
        // 소비 아이템 적용
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
    /// 장비 아이템의 합산 능력치 및 효과 갱신
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