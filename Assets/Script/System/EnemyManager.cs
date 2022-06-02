using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 전체 몬스터에 적용되는 이벤트 및 효과를 관리
/// </summary>
[DisallowMultipleComponent]
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    // 몹 전체한테 적용하는 이벤트
    public delegate void TakeDamageHandler(GameObject sender, int amount);
    public delegate void EnemyDeadHandler(GameObject sender);

    public event TakeDamageHandler onEnemyTakeDamage;
    public event EnemyDeadHandler onEnemyDead;

    // Monster Object Pool : Pool System과 다르게 몬스터는 SetActive(false)를 사용해야 하므로 따로 제작
    // key : Prefab(GameObject), Value : Enemy Object(GameObject)
    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    [SerializeField] private GameObject damageDrawerPrefab;

    private GameObject[] targets = new GameObject[2];
    private List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        Debug.Assert(damageDrawerPrefab, "Error : Damage Drawer not setting");
    }

    public void Start()
    {
        targets[0] = GameObject.Find("Player");
        targets[1] = GameObject.Find("Base");

        // 적에게 공통적으로 적용되어야 하는 이벤트 설정
        onEnemyDead += (sender) => RoundSystem.Instance.CheckRoundEnd();
        onEnemyTakeDamage += DrawDamaged;
    }

    public void InitEnemy(GameObject enemy, GameObject route)
    {
        Debug.Assert(enemy, "Error : Enemy Target isn't set");

        var eFSM = enemy.GetComponent<EnemyFSM>();
        eFSM.Init(targets[(int)eFSM.firstTarget], route);
    }

    #region Enemy Object Pool
    //private void CreateInstance(GameObject prefab, int poolCount = 20)
    //{
    //    if (!pools.ContainsKey(prefab))
    //    {
    //        Debug.Log("add pool " + prefab.name);
    //        pools.TryAdd(prefab, new Queue<GameObject>());
    //    }

    //    var pool = pools[prefab];
    //    for (int i = 0; i < poolCount; i++)
    //    {
    //        var instance = Instantiate(prefab);
    //        instance.SetActive(false);
    //        pool.Enqueue(instance);
    //        // 몬스터에 Owner를 지정해서 
    //    }
    //}

    //private GameObject GetInstance(GameObject prefab)
    //{
    //    if (pools.TryGetValue(prefab, out var pool))
    //    {
    //        if (!pool.TryDequeue(out var instance))
    //        {
    //            instance = Instantiate(prefab);
    //            instance.SetActive(true);
    //            return instance;
    //        }
    //    }

    //    Debug.LogError("Error : Object Pool을 사용하지 않는 prefab을 대상으로 GetInstance() 사용");
    //    return null;
    //}

    //public void Destroy(GameObject instance)
    //{
    //    if (pools.ContainsKey(prefab))
    //    {
    //        instance.SetActive(false);
    //        pools[prefab].Enqueue(instance);
    //    }
    //    else
    //    {
    //        Debug.LogError($"Error : There is no pool {prefab.name}");
    //    }
    //}

    #endregion


    public void InvokeEnemyDamaged(GameObject sender, int amount)
    {
        onEnemyTakeDamage?.Invoke(sender, amount);
    }

    public void InvokeEnemyDead(GameObject sender)
    {
        onEnemyDead?.Invoke(sender);
        
    }

    /// <summary>
    /// 몹이 데미지를 입을 때 출력
    /// </summary>
    public void DrawDamaged(GameObject sender, int amount)
    {
        var go = Instantiate(damageDrawerPrefab, sender.transform.position, Quaternion.identity);
        var damagedDrawer = go.GetComponent<EnemyDamagedDrawer>();
        damagedDrawer.SetDraw(amount);
        //damagedDrawer.SetDrawInfo(amount, Color.red, sender.transform.position);
    }
}
