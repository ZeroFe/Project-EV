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

    private GameObject player;
    private GameObject playerBase;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        Debug.Assert(damageDrawerPrefab, "Error : Damage Drawer not setting");

        player = GameObject.Find("Player");
        playerBase = GameObject.Find("Base");
    }

    public void SetEnemyTarget(GameObject enemy)
    {
        Debug.Assert(enemy, "Error : Enemy Target isn't set");

        var eFSM = enemy.GetComponent<EnemyFSM>();
        if (eFSM.firstTarget == EnemyFSM.FirstTarget.Player)
        {
            eFSM.SetTarget(player);
        }
        else if (eFSM.firstTarget == EnemyFSM.FirstTarget.Base)
        {
            eFSM.SetTarget(playerBase);
        }
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
