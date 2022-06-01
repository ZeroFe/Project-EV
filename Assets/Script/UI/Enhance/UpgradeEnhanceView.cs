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

    [SerializeField] private GameObject selectFrame;
    [SerializeField] private GameObject selectTopStar;
    [SerializeField] private GameObject selectBottomStar;

    [SerializeField] private AudioClip pointEnterSound;
    [SerializeField] private AudioSource audioSource;

    public Action<Enhance> SelectEnhanceHandler;
    private Enhance upgradeEnhance;

    private void Awake()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Point Enter");
        audioSource.PlayOneShot(pointEnterSound);
        selectFrame.SetActive(true);
        selectTopStar.SetActive(true);
        selectBottomStar.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Point Exit");
        selectFrame.SetActive(false);
        selectTopStar.SetActive(false);
        selectBottomStar.SetActive(false);
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
        name.text = enhance.EnhanceName;
        description.text = enhance.Description;
        upgradeEnhance = enhance;
    }
}
