using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnIndicator : MonoBehaviour
{
    [SerializeField, Tooltip("�ִϸ��̼��� ���� ũ��, ��ǥ ũ��")] 
    public Vector2 toScale;
    [SerializeField, Tooltip("�ִϸ��̼��� ���� ũ��, ��ǥ ũ��")]
    public Vector2 toRotate;

    public float duration = 1.5f;
    public float destroyWaitTime = 0.5f;

    private void OnEnable()
    {
        transform.localScale = toScale.x * Vector3.one;
        transform.DOScale(toScale.y * Vector3.one, duration);
        transform.DORotate(transform.eulerAngles + toRotate.y * Vector3.up, duration);

        Invoke(nameof(Destroy), duration + destroyWaitTime);
    }

    // Object Ǯ�� �ݳ�
    void Destroy()
    {
        gameObject.SetActive(false);
    }
}
