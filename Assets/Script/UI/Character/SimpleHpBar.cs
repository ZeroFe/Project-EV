using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleHpBar : MonoBehaviour
{
    public CharacterStatus targetStatus;

    public Slider hpSlider;
    public TMP_Text hpText;

    private void Start()
    {
        Debug.Assert(targetStatus, "Error : target Status not setting");
        targetStatus.onHpChanged += DrawHp;
        DrawHp(targetStatus.CurrentHp, targetStatus.MaxHp);
    }

    public void DrawHp(int current, int max)
    {
        float hpPercent = (float)current / max;
        hpSlider.value = hpPercent;
        hpText.text = $"{current} / {max}";
    }
}
