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


    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 몹이 데미지를 입을 때 
    /// </summary>
    public void DrawDamaged()
    {

    }
}
