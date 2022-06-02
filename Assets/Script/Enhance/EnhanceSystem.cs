using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 강화 목록을 관리하는 클래스
/// </summary>
[DisallowMultipleComponent]
public class EnhanceSystem : MonoBehaviour
{
    public static EnhanceSystem Instance { get; private set; }

    // 플레이어만 아이템 적용하면 되므로 플레이어를 추가한다
    [SerializeField] private GameObject player;
    private Inventory playerInventory;

    private EnhanceDatabase enhanceDatabase;

    // 선행 조건이 되는 강화들을 관리
    // Key - 선행조건에 해당하는 Original Enhance
    // Value(List) - 이 선행조건을 가지고 있는 Clone Enhance
    private Dictionary<Enhance, List<Enhance>> prerequisitesDict = new Dictionary<Enhance, List<Enhance>>();
    // CodeName을 이용해 원본 Enhance를 가져온다
    private Dictionary<string, Enhance> codeNameToOriginalDict = new Dictionary<string, Enhance>();

    // 선행조건 완수 안 된 강화와 선행조건 완수된(빈 것도 완수 처리) 강화 구분
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
        // 아이템 정보 전부 불러오기
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
            // 선행 조건이 있으면 바로 업그레이드할 수 없다
            if (clone.prerequisites.Count != 0)
            {
                lockedEnhances.Add(clone);
                // 선행 조건 목록에 추가한다
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
            // 선행 조건이 없는 경우 업그레이드할 수 있다
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
        // 선행 조건 없애기
        var original = codeNameToOriginalDict[enhance.CodeName];
        if (prerequisitesDict.ContainsKey(original))
        {
            foreach (var target in prerequisitesDict[original])
            {
                target.prerequisites.Remove(original);
                Debug.Log($"Enhance {target.name}'s prerequisite {original.name} removed");
                // 아이템에 남은 선행 조건이 없으면 미추가 목록에서 지우고 아이템을 추가한다
                if (target.prerequisites.Count == 0)
                {
                    // 미추가 목록에서 지우기
                    lockedEnhances.Remove(target);
                    // 아이템 추가
                    upgradableEnhances.Add(target);
                }
            }

            prerequisitesDict.Remove(enhance);
        }

        // 플레이어에게 강화 적용
        playerInventory.AddItem(enhance);
        // 적용된 강화는 업그레이드 목록에서 제외한다
        upgradableEnhances.Remove(enhance);
    }
}
