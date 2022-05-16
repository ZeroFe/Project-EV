using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class EnemyStatus : CharacterStatus
{
    //public delegate void Damage

    private void OnEnable()
    {
        //EnemyManager.Instance
    }

    /// <summary>
    /// Enemy의 경우 Object Pool을 이용해 재사용되므로
    /// OnEnable에서 초기화해서 쓸 수 있도록 
    /// </summary>
    private void Init()
    {

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
