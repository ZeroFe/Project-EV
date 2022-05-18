using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ü ���Ϳ� ����Ǵ� �̺�Ʈ �� ȿ���� ����
/// </summary>
[DisallowMultipleComponent]
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    // �� ��ü���� �����ϴ� �̺�Ʈ
    public delegate void TakeDamageHandler(int amount);

    private GameObject player;
    private GameObject playerBase;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
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
    public void DrawDamaged()
    {

    }
}
