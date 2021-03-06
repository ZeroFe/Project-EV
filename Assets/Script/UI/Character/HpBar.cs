using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

public class HpBar : MonoBehaviour
{
    public CharacterStatus targetStatus;

    [Header("Component")]
    [SerializeField] protected Image fillHp;
    [SerializeField] protected Image changedHp;

    [Header("Animation")]
    [SerializeField] protected float animationTime = 0.7f;

    private void Awake()
    {
        Debug.Assert(targetStatus);

        Debug.Assert(fillHp);
        Debug.Assert(changedHp);
    }

    public void OnEnable()
    {
        targetStatus.OnHpChanged += Draw;
    }

    public virtual void Draw(int current, int max)
    {
        Draw((float)current / max);
    }

    public virtual void Draw(float percent)
    {
        fillHp.fillAmount = percent;
        changedHp.DOFillAmount(percent, animationTime).
            SetEase(Ease.Linear);
    }

    public void End()
    {
        targetStatus.OnHpChanged -= Draw;
    }
}
