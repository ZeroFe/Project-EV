using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 강화 목록을 관리하는 클래스
/// </summary>
[DisallowMultipleComponent]
public class ItemManager : Singleton<ItemManager>
{
    // 플레이어만 아이템 적용하면 되므로 플레이어를 추가한다
    [SerializeField] private GameObject player;
    private Inventory playerInventory;

    public GameObject upgradeUI;

    // 선행 조건이 되는 강화들을 관리
    private Dictionary<Item, List<Item>> PrerequisitesDict = new Dictionary<Item, List<Item>>();
    // 미추가된 아이템
    private HashSet<Item> items = new HashSet<Item>();

    private List<EquipItem> _equipItemDropList = new List<EquipItem>();
    private int currEquipIndex = 0;

    #region Init
    private void Awake()
    {
        //droppedItemPool = new ObjectPooling<DroppedItemComponent>(transform, droppedItemPrefab);
        Debug.Assert(player, "Player not be set in Enhance Manager");

        playerInventory = GetComponent<Inventory>();
    }

    public void Start()
    {
        // 아이템 정보 전부 불러오기
        var items = Resources.LoadAll<Item>("Items");
        // 불러온 아이템 정보로 드랍 테이블 만들기
        InitDropTable(items);
        // 
    }

    // 초기 드랍 테이블 세팅
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
    #endregion

    #region 아이템 가져오기

    public EquipItem DequeueEquipItem()
    {
        // 순회식 드롭
        if (currEquipIndex >= _equipItemDropList.Count)
        {
            currEquipIndex = 0;
        }

        return _equipItemDropList[currEquipIndex];
    }

    #endregion

    public void EnhancePlayer()
    {
        // 업그레이드 창 띄우기
        GameManager.Instance.SetCursorDisplay(true);
        upgradeUI.SetActive(true);
    }

    public void EndEnhance(Item enhance)
    {
        // 선행 조건 없애기
        if (PrerequisitesDict.ContainsKey(enhance))
        {
            foreach (var item in PrerequisitesDict[enhance])
            {
                //item.
                // 아이템에 남은 선행 조건이 없으면 미추가 목록에서 지우고 아이템을 추가한다
            }

            PrerequisitesDict.Remove(enhance);
        }
        // 플레이어에게 강화 적용
        playerInventory.AddItem(enhance);

        // 다음 라운드 시작
        GameManager.Instance.SetCursorDisplay(false);
        RoundSystem.Instance.NextRound();
    }
}
