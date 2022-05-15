using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ȱ��ȭ�� Singleton Object�� Instance�� �ʱ�ȭ�� �� �ֵ���
/// Awake�� �����Ű�� �����ϴ� ��ũ��Ʈ
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
