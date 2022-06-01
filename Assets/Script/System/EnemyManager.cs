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



    [SerializeField] private GameObject damageDrawerPrefab;

    private GameObject[] targets = new GameObject[2];

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
