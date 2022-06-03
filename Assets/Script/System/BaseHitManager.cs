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
            // 맞았을 때 기지 공격 당함 알림음
            audioSource.PlayOneShot(hitNotifySound);

            baseFillImage.color = normalBaseColor;
            var hitFlash = baseFillImage.DOColor(hitBaseColor, 0.5f).SetLoops(6);
            hitFlash.onComplete = () =>
            {
                baseFillImage.color = normalBaseColor;
                doHitAnimation = false;
            };
        }

        // 맞았을 때 잠시 체력 바 표시


        // 미니맵에 Base 위치에 해당하는 Indicator를 출력한다
    }

    private void OnEnterCrisis()
    {
        // 위험 상태 알림 소리
        audioSource.PlayOneShot(crisisNotifySound);

        // 위험 상태이면 계속 점멸해야함
        foreach (var light in crisisNotifyLights)
        {
            light.color = crisisLightColor;
        }
        
        StartCoroutine(nameof(IEFlashAnimation));
    }

    private void OnExitCrisis()
    {
        // 위험 상태 해제
        // 위험 상태 점멸 해제
        foreach (var light in crisisNotifyLights)
        {
            light.color = normalLightColor;
        }
        StopCoroutine(nameof(IEFlashAnimation));
    }

    IEnumerator IEFlashAnimation()
    {
        Vector2 lightIntensityVec = flashLightIntensity;
        // 초기 불빛 세기에 따라 첫 점멸 시간은 달라져야 함
        float currentTime = ((normalLightIntensity - flashLightIntensity.x) / flashLightIntensity.y) * flashTime;
        while (true)
        {
            for (; currentTime <= flashTime; currentTime += Time.deltaTime)
            {
                // 불빛 세기 변경
                foreach (var light in crisisNotifyLights)
                {
                    light.intensity = Mathf.Lerp(lightIntensityVec.x, lightIntensityVec.y, currentTime / flashTime);
                }
                yield return 0;
            }

            // 불빛 세기가 Fade In / Fade Out 식으로 바뀔 수 있도록 Flip-Flop 식으로 빛 세기를 바꿔준다
            lightIntensityVec = new Vector2(lightIntensityVec.y, lightIntensityVec.x);
            currentTime = 0;
        }
    }
}
