using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnIndicator : MonoBehaviour
{
    [SerializeField, Tooltip("애니메이션할 시작 크기, 목표 크기")] 
    public Vector2 toScale;
    [SerializeField, Tooltip("애니메이션할 시작 크기, 목표 크기")]
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

    // Object 풀에 반납
    void Destroy()
    {
        gameObject.SetActive(false);
    }
}
