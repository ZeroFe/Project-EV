using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ��ȭ ����� �����ϴ� Ŭ����
/// </summary>
[DisallowMultipleComponent]
public class EnhanceSystem : MonoBehaviour
{
    public static EnhanceSystem Instance { get; private set; }

    // �÷��̾ ������ �����ϸ� �ǹǷ� �÷��̾ �߰��Ѵ�
    [SerializeField] private GameObject player;
    private Inventory playerInventory;

    private EnhanceDatabase enhanceDatabase;

    // ���� ������ �Ǵ� ��ȭ���� ����
    // Key - �������ǿ� �ش��ϴ� Original Enhance
    // Value(List) - �� ���������� ������ �ִ� Clone Enhance
    private Dictionary<Enhance, List<Enhance>> prerequisitesDict = new Dictionary<Enhance, List<Enhance>>();
    // CodeName�� �̿��� ���� Enhance�� �����´�
    private Dictionary<string, Enhance> codeNameToOriginalDict = new Dictionary<string, Enhance>();

    // �������� �ϼ� �� �� ��ȭ�� �������� �ϼ���(�� �͵� �ϼ� ó��) ��ȭ ����
    private HashSet<Enhance> lockedEnhances = new HashSet<Enhance>();
    private List<Enhance> upgradableEnhances = new List<Enhance>();

    private void Awake()
    {
        Instance = this;

        enhanceDatabase = Resources.Load<EnhanceDatabase>("EnhanceDatabase");

        player = GameObject.Find("Player");
        Debug.Assert(player, "Player not be set in Enhance Manager");

        playerInventory = player.GetComponent<Inventory>();
    }

    public void Start()
    {
        // ������ ���� ���� �ҷ�����
        var enhanceDB = enhanceDatabase.Enhances.GetEnumerator();
        while (enhanceDB.MoveNext())
        {
            var enhance = enhanceDB.Current;
            if (!enhance)
            {
                Debug.LogError("error case : enhance is null");
            }

            codeNameToOriginalDict.Add(enhance.CodeName, enhance);
            var clone = enhance.Clone();
            // ���� ������ ������ �ٷ� ���׷��̵��� �� ����
            if (clone.prerequisites.Count != 0)
            {
                lockedEnhances.Add(clone);
                // ���� ���� ��Ͽ� �߰��Ѵ�
                Debug.Log($"add locked enhance " + clone.name);
                foreach (var pre in clone.prerequisites)
                {
                    if (!prerequisitesDict.ContainsKey(pre))
                    {
                        prerequisitesDict.Add(pre, new List<Enhance>());
                    }
                    prerequisitesDict[pre].Add(clone);
                }
            }
            // ���� ������ ���� ��� ���׷��̵��� �� �ִ�
            else
            {
                upgradableEnhances.Add(clone);
            }
        }
    }
    
    public List<Enhance> GetRandomEnhances(int enhanceCount)
    {
        var enhances = new List<Enhance>();

        while (enhances.Count < enhanceCount)
        {
            int randomIndex = UnityEngine.Random.Range(0, upgradableEnhances.Count);
            if (!enhances.Exists(x => x == upgradableEnhances[randomIndex]))
            {
                enhances.Add(upgradableEnhances[randomIndex]);
            }
        }

        return enhances;
    }

    public void ApplyEnhance(Enhance enhance)
    {
        // ���� ���� ���ֱ�
        var original = codeNameToOriginalDict[enhance.CodeName];
        if (prerequisitesDict.ContainsKey(original))
        {
            foreach (var target in prerequisitesDict[original])
            {
                target.prerequisites.Remove(original);
                Debug.Log($"Enhance {target.name}'s prerequisite {original.name} removed");
                // �����ۿ� ���� ���� ������ ������ ���߰� ��Ͽ��� ����� �������� �߰��Ѵ�
                if (target.prerequisites.Count == 0)
                {
                    // ���߰� ��Ͽ��� �����
                    lockedEnhances.Remove(target);
                    // ������ �߰�
                    upgradableEnhances.Add(target);
                }
            }

            prerequisitesDict.Remove(enhance);
        }

        // �÷��̾�� ��ȭ ����
        playerInventory.AddItem(enhance);
        // ����� ��ȭ�� ���׷��̵� ��Ͽ��� �����Ѵ�
        upgradableEnhances.Remove(enhance);
    }
}
