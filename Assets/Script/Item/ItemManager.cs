using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 아이템 관리 싱클톤 클래스
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
        // 아이템 정보 전부 불러오기
        var items = Resources.LoadAll<Item>("Items");
        // 불러온 아이템 정보로 드랍 테이블 만들기
        
        // 
    }

    #region 아이템 드롭 테이블 관리
    // 초기 드랍 테이블 세팅
    void InitDropTable(Item[] items)
    {
        foreach (var item in items)
        {
            
        }
    }
    // 드랍 테이블 리필
    #endregion

    #endregion

    #region 아이템 가져오기

    //public ConsumableItem GetConsumableItem()
    //{

    //}

    public EquipItem DequeueEquipItem()
    {
        return _equipItemDropList.Dequeue();
    }

    #endregion

    #region 아이템 드롭
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
