using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 관리 싱클톤 클래스
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
        // 아이템 정보 전부 불러오기
        var items = Resources.LoadAll<Item>("Items");
        // 불러온 아이템 정보로 드랍 테이블 만들기
        // 

        // 임시
        //foreach (var item in equipItems)
        //{
        //    Debug.Log($"{item.name}'s Effect");
        //    foreach (var effect in item.passiveEffect)
        //    {
        //        Debug.Log($"{effect.Effect.Explain()}");
        //    } 
        //} 
    }

    #region 아이템 드롭 테이블 관리
    // 초기 드랍 테이블 세팅
    // 드랍 테이블 리필
    #endregion

    #endregion

    #region 아이템 가져오기

    //public ConsumableItem GetConsumableItem()
    //{

    //}

    //public EquipItem 

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
