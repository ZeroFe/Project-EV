using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public static readonly float ANIMATE_TIME = 0.025f;

    public CharacterStatus targetStatus;

    [Header("Component")]
    [SerializeField] private Image fillHp;
    [SerializeField] private Image changedHp;

    [Header("Animation")]
    [SerializeField] private float animationTime = 0.05f;
    [SerializeField] private float changedPerSecond = 0.2f;
    private float changedPerLoop;

    private void Awake()
    {
        Debug.Assert(targetStatus);

        Debug.Assert(fillHp);
        Debug.Assert(changedHp);
    }

    public void Start()
    {
        targetStatus.OnHpChanged += (int current, int max) => Changed((float) current / max);

        changedPerLoop = changedPerSecond * ANIMATE_TIME;
        StartCoroutine(HpChangeAnimate());
    }

    public void Changed(float percent)
    {
        fillHp.fillAmount = percent;
    }

    IEnumerator HpChangeAnimate()
    {
        while (true)
        {
            if (fillHp.fillAmount < changedHp.fillAmount)
            {
                changedHp.fillAmount = Mathf.Max(changedHp.fillAmount - changedPerLoop, fillHp.fillAmount);
            }
            yield return new WaitForSeconds(ANIMATE_TIME);
        }
    }
}
