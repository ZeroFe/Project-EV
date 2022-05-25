using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WeaponView : MonoBehaviour
{
    public static WeaponView Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI weaponClipSize;
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private Image reloadFill;
    [SerializeField] private Slider reloadBar;

    private void Awake()
    {
        Instance = this;

        Debug.Assert(weaponClipSize, $"{weaponClipSize.name} not set");
        Debug.Assert(ammoCount, $"{ammoCount.name} not set");
        Debug.Assert(reloadFill, $"{reloadFill.name} not set");
        Debug.Assert(reloadBar, $"{reloadBar.name} not set");
    }

    public void UpdateClipInfo(int amount)
    {
        weaponClipSize.text = amount.ToString();
    }

    public void UpdateAmmoCount(int amount)
    {
        ammoCount.text = amount.ToString();
    }

    public void UpdateReloadBar(float percent)
    {
        //reloadBar.value = Mathf.Clamp01(percent);
        reloadFill.fillAmount = Mathf.Clamp01(percent);
    }
}
