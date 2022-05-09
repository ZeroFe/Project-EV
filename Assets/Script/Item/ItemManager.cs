using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ������ ���� ��Ŭ�� Ŭ����
public class ItemManager : Singleton<ItemManager>
{
    public DroppedItemComponent droppedItemPrefab;

    private List<EquipItem> _equipItemDropList = new List<EquipItem>();
    private int currEquipIndex = 0;

    #region Init
    private void Awake()
    {
        //droppedItemPool = new ObjectPooling<DroppedItemComponent>(transform, droppedItemPrefab);
    }

    public void Start()
    {
        // ������ ���� ���� �ҷ�����
        var items = Resources.LoadAll<Item>("Items");
        // �ҷ��� ������ ������ ��� ���̺� �����
        InitDropTable(items);
        // 
    }

    #region ������ ��� ���̺� ����
    // �ʱ� ��� ���̺� ����
    void InitDropTable(Item[] items)
    {
        Debug.Log("init drop table");
        foreach (var item in items)
        {
            if (item is EquipItem equipItem)
            {
                _equipItemDropList.Add(equipItem);
                Debug.Log($"add item - {equipItem.name}");
            }
        }
    }
    // ��� ���̺� ����
    #endregion

    #endregion

    #region ������ ��������

    //public ConsumableItem GetConsumableItem()
    //{

    //}

    public EquipItem DequeueEquipItem()
    {
        // ��ȸ�� ���
        if (currEquipIndex >= _equipItemDropList.Count)
        {
            currEquipIndex = 0;
        }

        return _equipItemDropList[currEquipIndex];
    }

    #endregion

    #region ������ ���
    public void DropEquip()
    {

    }

    public void DropConsumable()
    {

    }

    //public void DropItem(Item item, Vector3 dropPos)
    //{
    //    var droppedItem = droppedItemPool.Rent();
    //    droppedItem.ItemData = item;
    //    droppedItem.transform.position = dropPos;
    //}

    //public void RemoveDroppedItem(DroppedItemComponent droppedItem)
    //{
    //    droppedItemPool.Return(droppedItem);
    //}
    #endregion
}
