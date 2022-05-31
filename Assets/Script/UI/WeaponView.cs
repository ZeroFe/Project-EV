using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class WeaponView : MonoBehaviour
{
    public static WeaponView Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI weaponClipSize;
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private Image reloadFill;
    [SerializeField] private Image crosshair;

    [Header("Update Ammo Count")] 
    [SerializeField] private Vector2 ammoCountFontSizeRange = new Vector2(48, 60);
    // 주의 : Color 초기 세팅 시 Alpha값이 0이므로 암살당할 수 있음
    [SerializeField] private Color normalAmmoColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    [SerializeField] private Color highlightAmmoColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    [SerializeField] private Color reloadAmmoColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    [SerializeField, Tooltip("숫자 커지는 시간 / 최대로 유지할 시간 / 다시 작아지는 시간")] 
    private Vector3 ammoCountAnimDuration = new Vector3(0.2f, 0.05f, 0.1f);
    private Coroutine updateAmmoCountCoroutine = null;

    private int clipSize = 0;

    private void Awake()
    {
        Instance = this;

        Debug.Assert(weaponClipSize, $"{weaponClipSize.name} not set");
        Debug.Assert(ammoCount, $"{ammoCount.name} not set");
        Debug.Assert(reloadFill, $"{reloadFill.name} not set");
    }

    public void UpdateClipInfo(int amount)
    {
        weaponClipSize.text = amount.ToString();
        clipSize = amount;
    }

    #region Update Ammo

    public void UpdateAmmo(int amount)
    {
        ammoCount.text = amount.ToString();
    }

    public void UpdateAmmoCount(int amount)
    {
        if (updateAmmoCountCoroutine != null)
        {
            StopCoroutine(updateAmmoCountCoroutine);
        }
        updateAmmoCountCoroutine = StartCoroutine(DoUpdateAmmoCount(amount));
    }

    IEnumerator DoUpdateAmmoCount(int amount)
    {
        // 초기화 : 애니메이션이 연속 발사로 중단될 수 있으므로 초기화부터 시켜야 한다
        ammoCount.fontSize = ammoCountFontSizeRange.x;
        ammoCount.color = normalAmmoColor;

        // 
        for (float t = 0.0f; t < ammoCountAnimDuration.x; t += Time.deltaTime)
        {
            yield return 0;
            ammoCount.fontSize = Mathf.Lerp(ammoCountFontSizeRange.x, ammoCountFontSizeRange.y,
                t / ammoCountAnimDuration.x);
        }

        // 최대로 커진 경우 Ammo를 빛나게 하고, 
        ammoCount.fontSize = ammoCountFontSizeRange.y;
        ammoCount.color = highlightAmmoColor;
        ammoCount.text = amount.ToString();
        yield return new WaitForSeconds(ammoCountAnimDuration.y);

        // 다시 작아지면서 원 상태로 돌려야 한다
        for (float t = 0.0f; t < ammoCountAnimDuration.z; t += Time.deltaTime)
        {
            yield return 0;
            ammoCount.fontSize = Mathf.Lerp(ammoCountFontSizeRange.y, ammoCountFontSizeRange.x,
                t / ammoCountAnimDuration.z);
        }
        ammoCount.color = normalAmmoColor;
    }

    #endregion


    public void UpdateReloadBar(float percent)
    {
        reloadFill.fillAmount = Mathf.Clamp01(percent);
    }
}
