using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeEnhanceView : Singleton<UpgradeEnhanceView>
{
    // UI
    [SerializeField] private Image thumbnail;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;
    private Outline selectOutline;

    public Action<Enhance> SelectEnhanceHandler;
    private Enhance upgradeEnhance;

    private void Awake()
    {
        selectOutline = GetComponent<Outline>();
        selectOutline.enabled = false;
    }

    private void OnMouseEnter()
    {
        selectOutline.enabled = true;
    }

    private void OnMouseExit()
    {
        selectOutline.enabled = false;
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        // 상위 ui에게 알린다
        SelectEnhanceHandler.Invoke(upgradeEnhance);
    }

    public void DrawEnhance(Enhance enhance)
    {
        thumbnail.sprite = enhance.Icon;
        name.text = enhance.name;
        description.text = enhance.Description;
        upgradeEnhance = enhance;
    }
}
