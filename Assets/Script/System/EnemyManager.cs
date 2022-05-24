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

    [FormerlySerializedAs("damageDrawer")] [SerializeField]
    private GameObject damageDrawerPrefab;

    private GameObject[] targets = new GameObject[2];
    private GameObject player;
    private GameObject playerBase;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        Debug.Assert(damageDrawerPrefab, "Error : Damage Drawer not setting");

        targets[0] = GameObject.Find("Player");
        targets[1] = GameObject.Find("Base");
    }

    public void InitEnemy(GameObject enemy, GameObject route)
    {
        Debug.Assert(enemy, "Error : Enemy Target isn't set");

        var eFSM = enemy.GetComponent<EnemyFSM>();
        eFSM.Init(targets[(int)eFSM.firstTarget], route);
    }

    /// <summary>
    /// ���� �������� ���� �� ���
    /// </summary>
    public void DrawDamaged(GameObject sender, int amount)
    {
        var go = Instantiate(damageDrawerPrefab, sender.transform.position, Quaternion.identity);
        var damagedDrawer = go.GetComponent<EnemyDamagedDrawer>();
        damagedDrawer.SetDrawInfo(amount, Color.red, sender.transform.position);
    }
}
