using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ������ ���� ��Ŭ�� Ŭ����
public class ItemManager : Singleton<ItemManager>
{
    public DroppedItemComponent droppedItemPrefab;

    private Queue<EquipItem> _equipItemDropList = new Queue<EquipItem>();

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
        
        // 
    }

    #region ������ ��� ���̺� ����
    // �ʱ� ��� ���̺� ����
    void InitDropTable(Item[] items)
    {
        foreach (var item in items)
        {
            
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
        return _equipItemDropList.Dequeue();
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
