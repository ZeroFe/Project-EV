using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpBar : MonoBehaviour
{
    public Slider hpSlider;
    public TMP_Text hpText;

    public void DrawHp(int current, int max)
    {
        float hpPercent = (float)current / max;
        hpSlider.value = hpPercent;
        hpText.text = $"{current} / {max}";
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
