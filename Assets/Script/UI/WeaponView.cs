using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponView : MonoBehaviour
{
    public static WeaponView Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI weaponClipSize;
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private Slider reloadBar;

    private void Awake()
    {
        Instance = this;

        Debug.Assert(weaponClipSize, $"{weaponClipSize.name} not set");
        Debug.Assert(ammoCount, $"{ammoCount.name} not set");
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
        reloadBar.value = Mathf.Clamp01(percent);
    }
}
