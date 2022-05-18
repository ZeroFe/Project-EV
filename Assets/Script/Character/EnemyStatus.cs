using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 
/// </summary>
public class EnemyStatus : CharacterStatus
{
    //public delegate void Damage

    [FormerlySerializedAs("damage")] public int attackPower = 3;

    private void OnEnable()
    {
        //EnemyManager.Instance
        Init();
    }

    /// <summary>
    /// Enemy�� ��� Object Pool�� �̿��� ����ǹǷ�
    /// OnEnable���� �ʱ�ȭ�ؼ� �� �� �ֵ��� 
    /// </summary>
    private void Init()
    {
        CurrentHp = maxHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}