using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

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
    [SerializeField] private AudioClip upgradeSelectSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Select Animation")] 
    [SerializeField]
    private Transform selectLightImageTr;
    [SerializeField] private Vector2 selectAnimPosX = new Vector2(-50.0f, 600.0f);
    [SerializeField] private float animationTime = 0.3f;

    public Action<Enhance> SelectEnhanceHandler;
    private Enhance upgradeEnhance;

    private void Awake()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(pointEnterSound);
        selectFrame.SetActive(true);
        selectTopStar.SetActive(true);
        selectBottomStar.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectFrame.SetActive(false);
        selectTopStar.SetActive(false);
        selectBottomStar.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse Down");
        audioSource.PlayOneShot(upgradeSelectSound);

        // 애니메이션
        Sequence s = DOTween.Sequence();
        selectLightImageTr.position =
            new Vector3(selectAnimPosX.x, selectLightImageTr.position.y, selectLightImageTr.position.z);
        s.Append(selectLightImageTr.transform.DOMoveX(selectAnimPosX.y, animationTime));

        s.onComplete = () =>
        {
            // 상위 ui에게 알린다
            SelectEnhanceHandler.Invoke(upgradeEnhance);
        };
    }

    public void DrawEnhance(Enhance enhance)
    {
        thumbnail.sprite = enhance.Icon;
        name.text = enhance.EnhanceName;
        description.text = enhance.Description;
        upgradeEnhance = enhance;
    }
}
