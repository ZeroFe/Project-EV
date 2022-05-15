using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponView : Singleton<WeaponView>
{
    public static WeaponView Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI weaponClipSize;
    [SerializeField] private TextMeshProUGUI ammoCount;
    
    void OnEnable()
    {
        Instance = this;
    }

    public void UpdateClipInfo(Weapon weapon)
    {
        weaponClipSize.text = weapon.clipSize.ToString();
    }

    public void UpdateAmmoCount(int amount)
    {
        ammoCount.text = amount.ToString();
    }
}
