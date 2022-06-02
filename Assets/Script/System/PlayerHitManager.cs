using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHitManager : MonoBehaviour
{
    private Image bloodImage;

    public PlayerStatus status;

    [Header("UI Setting")]
    [SerializeField, Tooltip("맞았을 때 알파 값 설정 정도 : 초기 / 맞았을 때 최대치")]
    private Vector2 hitOverlayAlpha = new Vector2(0.0f, 0.3f);
    [SerializeField, Tooltip("낮은 체력일 때 알파 값 설정 정도 : 초기 / 맞았을 때 최대치")]
    private Vector2 crisisAlpha = new Vector2(0.2f, 0.7f);
    [SerializeField, Tooltip("Fade In 시간 / Fade Out 시간")] 
    private Vector2 animationTime = new Vector2(0.1f, 0.2f);

    [Header("Sound")]
    [SerializeField] private AudioClip crisisHeartbeatSound;

    private AudioSource audioSource;

    private Vector2 currentAlpha;
    private Sequence sequence;

    private void Awake()
    {
        bloodImage = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();

        Debug.Assert(status, "Error : Player Status not set");
        Debug.Assert(crisisHeartbeatSound, "Error : Crisis Heartbeat Sound not set");
    }

    private void Start()
    {
        // 초기 플레이어가 맞았을 땐 
        currentAlpha = hitOverlayAlpha;

        status.OnDamaged += (amount) => OnPlayerHit();
        status.onEnterCrisis += OnEnterCrisis;
        status.onExitCrisis += OnExitCrisis;
    }

    public void OnPlayerHit()
    {
        sequence = DOTween.Sequence();
        sequence.Append(bloodImage.DOFade(currentAlpha.y, animationTime.x));
        sequence.Append(bloodImage.DOFade(currentAlpha.x, animationTime.y));
    }

    private void OnEnterCrisis()
    {
        currentAlpha = crisisAlpha;
        audioSource.PlayOneShot(crisisHeartbeatSound);
    }

    private void OnExitCrisis()
    {
        currentAlpha = hitOverlayAlpha;
    }
}
