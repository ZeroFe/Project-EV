using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 비활성화한 Singleton Object의 Instance가 초기화될 수 있도록
/// Awake를 실행시키고 종료하는 스크립트
/// </summary>
public class AwakeExcuter : MonoBehaviour
{
    [SerializeField] private GameObject[] singletonObjects;

    private void Awake()
    {
        foreach (var singletonObject in singletonObjects)
        {
            singletonObject.SetActive(true);
            singletonObject.SetActive(false);
        }
    }
}
