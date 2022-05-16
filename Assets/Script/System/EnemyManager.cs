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


    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ���� �������� ���� �� 
    /// </summary>
    public void DrawDamaged()
    {

    }
}
