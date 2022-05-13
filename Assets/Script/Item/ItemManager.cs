using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ��ȭ ����� �����ϴ� Ŭ����
/// </summary>
[DisallowMultipleComponent]
public class ItemManager : Singleton<ItemManager>
{
    // �÷��̾ ������ �����ϸ� �ǹǷ� �÷��̾ �߰��Ѵ�
    [SerializeField] private GameObject player;
    private Inventory playerInventory;

    public GameObject upgradeUI;

    // ���� ������ �Ǵ� ��ȭ���� ����
    private Dictionary<Item, List<Item>> PrerequisitesDict = new Dictionary<Item, List<Item>>();
    // ���߰��� ������
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
        // ������ ���� ���� �ҷ�����
        var items = Resources.LoadAll<Item>("Items");
        // �ҷ��� ������ ������ ��� ���̺� �����
        InitDropTable(items);
        // 
    }

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
    #endregion

    #region ������ ��������

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

    public void EnhancePlayer()
    {
        // ���׷��̵� â ����
        GameManager.Instance.SetCursorDisplay(true);
        upgradeUI.SetActive(true);
    }

    public void EndEnhance(Item enhance)
    {
        // ���� ���� ���ֱ�
        if (PrerequisitesDict.ContainsKey(enhance))
        {
            foreach (var item in PrerequisitesDict[enhance])
            {
                //item.
                // �����ۿ� ���� ���� ������ ������ ���߰� ��Ͽ��� ����� �������� �߰��Ѵ�
            }

            PrerequisitesDict.Remove(enhance);
        }
        // �÷��̾�� ��ȭ ����
        playerInventory.AddItem(enhance);

        // ���� ���� ����
        GameManager.Instance.SetCursorDisplay(false);
        RoundSystem.Instance.NextRound();
    }
}
