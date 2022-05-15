using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeEnhanceView : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectOutline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectOutline.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
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
