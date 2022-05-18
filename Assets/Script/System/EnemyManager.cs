using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전체 몬스터에 적용되는 이벤트 및 효과를 관리
/// </summary>
[DisallowMultipleComponent]
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    // 몹 전체한테 적용하는 이벤트
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
    /// 몹이 데미지를 입을 때 출력
    /// </summary>
    public void DrawDamaged()
    {

    }
}
