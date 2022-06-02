using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// ��ü ���Ϳ� ����Ǵ� �̺�Ʈ �� ȿ���� ����
/// </summary>
[DisallowMultipleComponent]
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    // �� ��ü���� �����ϴ� �̺�Ʈ
    public delegate void TakeDamageHandler(GameObject sender, int amount);
    public delegate void EnemyDeadHandler(GameObject sender);

    public event TakeDamageHandler onEnemyTakeDamage;
    public event EnemyDeadHandler onEnemyDead;

    // Monster Object Pool : Pool System�� �ٸ��� ���ʹ� SetActive(false)�� ����ؾ� �ϹǷ� ���� ����
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

        // ������ ���������� ����Ǿ�� �ϴ� �̺�Ʈ ����
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
    //        // ���Ϳ� Owner�� �����ؼ� 
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

    //    Debug.LogError("Error : Object Pool�� ������� �ʴ� prefab�� ������� GetInstance() ���");
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
    /// ���� �������� ���� �� ���
    /// </summary>
    public void DrawDamaged(GameObject sender, int amount)
    {
        var go = Instantiate(damageDrawerPrefab, sender.transform.position, Quaternion.identity);
        var damagedDrawer = go.GetComponent<EnemyDamagedDrawer>();
        damagedDrawer.SetDraw(amount);
        //damagedDrawer.SetDrawInfo(amount, Color.red, sender.transform.position);
    }
}
