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

    public class ConsumableItemSlot
    {
        public ConsumableItem item;
        public int stack;

        public ConsumableItemSlot(ConsumableItem newConsumable)
        {
            item = newConsumable;
            stack = 1;
        }
    }

    public class EquipItemSlot
    {
        public EquipItem item;
        public int equipPassiveCount;

        public EquipItemSlot(EquipItem newEquip)
        {
            item = newEquip;
            equipPassiveCount = 0;
        }
    }

    public List<EquipItemSlot> EquipItems { get; private set; } = new List<EquipItemSlot>();
    public List<ConsumableItemSlot> ConsumableItems { get; private set; } = new List<ConsumableItemSlot>();

    private GameObject _inventoryUser;

    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }
    private int _gold;

    private int[] _equipAbilityIncreaseSizeArr = new int[Enum.GetValues(typeof(EAbility)).Length];
    public int[] equipAbilityIncreaseSizeArrPublic = new int[Enum.GetValues(typeof(EAbility)).Length];

    public Inventory(GameObject userObject, int equipCapacity = 10, int consumableCapacity = 3, int gold = 0)
    {
        _inventoryUser = userObject;
        EquipItems.Capacity = equipCapacity;
        ConsumableItems.Capacity = consumableCapacity;
        _gold = gold;

        RegisterEquipEvent();
    }

    #region Implement Passive, Upgrade
    private void RegisterEquipEvent()
    {
        //var playerStat = _inventoryUser.GetComponent<Player>();
        //_inventoryUser.GetComponent<Player>().OnGiveDamage.Subscribe((data) => CallItemEvent(data.eventType, data.damageAmount));
    }

    /// <summary>
    /// �ش� �̺�Ʈ�� �Ͼ�� ��� ȿ�� �ߵ�(�нú�, ����)
    /// </summary>
    /// <param name="condition">�нú� �ߵ� ����</param>
    /// <param name="count">�нú� ���� ����</param>
    //private void CallItemEvent(EventObject eventType, int addedCount)
    //{
    //    foreach (var equipSlot in EquipItems)
    //    {
    //        if (equipSlot.item.passiveCondition.conditionEvent == eventType)
    //        {
    //            ExecuteEquipPassive(equipSlot, addedCount);
    //        }
    //    }
    //}

    /// <summary>
    /// ��� �нú긦 �߰� ���ǿ� ���� ����
    /// ex) nȸ�� �� �� ���� / n �̻��� �� ���� / n ������ �� ����
    /// </summary>
    /// <param name="equipSlot">���� ���</param>
    /// <param name="addedCount"></param>
    private void ExecuteEquipPassive(EquipItemSlot equipSlot, int addedCount)
    {
        equipSlot.equipPassiveCount = equipSlot.item.passiveCondition.conditionOption.AddCount(equipSlot.equipPassiveCount, addedCount);
        if (equipSlot.item.passiveCondition.SatisfyCondition(equipSlot.item.passiveCount, equipSlot.equipPassiveCount))
        {
            foreach (var item in equipSlot.item.passiveEffect)
            {
                item.GetReference().TakeUseEffect();
            }
        }
        equipSlot.equipPassiveCount = equipSlot.item.passiveCondition.NextUserCount(equipSlot.equipPassiveCount, addedCount);
    }
    #endregion

    #region Add / Delete Item
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
        if (EquipItems.Count >= EquipItems.Capacity)
        {
            return false;
        }

        EquipItems.Add(new EquipItemSlot(newEquip));
        AddEquipAbility(newEquip);

        addItemEvent.Invoke(newEquip.name.GetHashCode());

        return true;
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