using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HpBarTextExtend : HpBar
{
    [Header("Text")]
    public TextMeshProUGUI hpText;

    public override void Draw(int current, int max)
    {
        base.Draw(current, max);
        hpText.text = $"{current} / {max}";
    }
}
