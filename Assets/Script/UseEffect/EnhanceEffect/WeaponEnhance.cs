using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponEnhance : EnhanceEffect
{
    // 발사 관련
    [SerializeField, Tooltip("발사 속도 증감 퍼센트")]
    private float attackSpeedMultiplier = 1.0f;

    [FormerlySerializedAs("reloadSpeedMultiplier")] [SerializeField, Tooltip("재장전 속도 증감 퍼센트")]
    private float reloadTimeMultiplier = 1.0f;

    [SerializeField, Tooltip("탄창 크기 증감량")]
    private int clipSizeAdder = 0;
    [SerializeField, Tooltip("탄창 크기 증감 퍼센트")]
    private float clipSizeMultiplier = 1.0f;

    [Header("Advanced Option")]
    [SerializeField, Tooltip("")]
    private float spreadAngle = 0.0f;
    [SerializeField, Tooltip("")] 
    private int projectilePerShot = 1;

    public override void ApplyEnhance(GameObject target)
    {
        Weapon weapon = target.GetComponent<PlayerCtrl>().PlayerWeapon;

        weapon.fireRate = weapon.fireRate * attackSpeedMultiplier;
        weapon.reloadTime = weapon.reloadTime * reloadTimeMultiplier;

        weapon.ClipSize += clipSizeAdder;
        weapon.ClipSize = (int)((float)weapon.clipSize * clipSizeMultiplier);

        weapon.advancedSettings.spreadAngle = spreadAngle;
        weapon.advancedSettings.projectilePerShot = projectilePerShot;
    }
}
