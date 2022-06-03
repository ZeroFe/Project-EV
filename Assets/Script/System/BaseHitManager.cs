using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BaseHitManager : MonoBehaviour
{
    public static BaseHitManager Instance { get; private set; }

    private PlayerStatus baseStatus;

    [SerializeField] 
    private Light[] crisisNotifyLights;
    [SerializeField] private Color normalLightColor = Color.white;
    [SerializeField] private float normalLightIntensity = 5.0f; 

    [SerializeField] 
    private AudioClip hitNotifySound;
    [SerializeField]
    private AudioClip crisisNotifySound;
    private AudioSource audioSource;

    [SerializeField] 
    private HpBarTextExtend baseHpBar;

    [Header("Hit Flash Animation")] 
    [SerializeField]
    private Image baseFillImage;
    [SerializeField] private Color normalBaseColor;
    [SerializeField] private Color hitBaseColor;
    private bool doHitAnimation = false;

    [Header("Crisis Flash Animation")]
    [SerializeField] 
    private float flashTime = 0.7f;
    [SerializeField] private Color crisisLightColor = Color.red;
    [SerializeField] 
    private Vector2 flashLightIntensity = new Vector2(2, 8);
    private Coroutine flashAnimationCoroutine = null;

    private void Awake()
    {
        Instance = this;

        baseStatus = GetComponent<PlayerStatus>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        baseStatus.OnDamaged += (amount) => OnBaseHit();
        baseStatus.onEnterCrisis += OnEnterCrisis;
        baseStatus.onExitCrisis += OnExitCrisis;

        foreach (var light in crisisNotifyLights)
        {
            light.color = normalLightColor;
            light.intensity = normalLightIntensity;
        }
    }

    private void OnBaseHit()
    {
        if (!doHitAnimation)
        {
            doHitAnimation = true;
            // �¾��� �� ���� ���� ���� �˸���
            audioSource.PlayOneShot(hitNotifySound);

            baseFillImage.color = normalBaseColor;
            var hitFlash = baseFillImage.DOColor(hitBaseColor, 0.5f).SetLoops(6);
            hitFlash.onComplete = () =>
            {
                baseFillImage.color = normalBaseColor;
                doHitAnimation = false;
            };
        }

        // �¾��� �� ��� ü�� �� ǥ��


        // �̴ϸʿ� Base ��ġ�� �ش��ϴ� Indicator�� ����Ѵ�
    }

    private void OnEnterCrisis()
    {
        // ���� ���� �˸� �Ҹ�
        audioSource.PlayOneShot(crisisNotifySound);

        // ���� �����̸� ��� �����ؾ���
        foreach (var light in crisisNotifyLights)
        {
            light.color = crisisLightColor;
        }
        
        StartCoroutine(nameof(IEFlashAnimation));
    }

    private void OnExitCrisis()
    {
        // ���� ���� ����
        // ���� ���� ���� ����
        foreach (var light in crisisNotifyLights)
        {
            light.color = normalLightColor;
        }
        StopCoroutine(nameof(IEFlashAnimation));
    }

    IEnumerator IEFlashAnimation()
    {
        Vector2 lightIntensityVec = flashLightIntensity;
        // �ʱ� �Һ� ���⿡ ���� ù ���� �ð��� �޶����� ��
        float currentTime = ((normalLightIntensity - flashLightIntensity.x) / flashLightIntensity.y) * flashTime;
        while (true)
        {
            for (; currentTime <= flashTime; currentTime += Time.deltaTime)
            {
                // �Һ� ���� ����
                foreach (var light in crisisNotifyLights)
                {
                    light.intensity = Mathf.Lerp(lightIntensityVec.x, lightIntensityVec.y, currentTime / flashTime);
                }
                yield return 0;
            }

            // �Һ� ���Ⱑ Fade In / Fade Out ������ �ٲ� �� �ֵ��� Flip-Flop ������ �� ���⸦ �ٲ��ش�
            lightIntensityVec = new Vector2(lightIntensityVec.y, lightIntensityVec.x);
            currentTime = 0;
        }
    }
}
