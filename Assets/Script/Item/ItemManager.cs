using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���� ��Ŭ�� Ŭ����
public class ItemManager : Singleton<ItemManager>
{
    public DroppedItemComponent droppedItemPrefab;

    private List<EquipItem> equipItems = new List<EquipItem>();

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

        // �ӽ�
        //foreach (var item in equipItems)
        //{
        //    Debug.Log($"{item.name}'s Effect");
        //    foreach (var effect in item.passiveEffect)
        //    {
        //        Debug.Log($"{effect.Effect.Explain()}");
        //    } 
        //} 
    }

    #region ������ ��� ���̺� ����
    // �ʱ� ��� ���̺� ����
    // ��� ���̺� ����
    #endregion

    #endregion

    #region ������ ��������

    //public ConsumableItem GetConsumableItem()
    //{

    //}

    //public EquipItem 

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
