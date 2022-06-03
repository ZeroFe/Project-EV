using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DisallowMultipleComponent]
public class UpgradeWindow : MonoBehaviour
{
    public static UpgradeWindow Instance { get; private set; }

    [SerializeField] private List<UpgradeEnhanceView> upgradeEnhanceViews;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private AudioClip upgradeStartSound;

    private CanvasGroup group;
    private AudioSource audioSource;

    private bool isInited = false;

    public void Awake()
    {
        Debug.Log("Upgrade Window Init");
        Instance = this;

        group = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // 초기 세팅 시 애니메이션 방지용
        if (!isInited)
        {
            isInited = true;
            return;
        }

        group.alpha = 0;
        DOTween.defaultTimeScaleIndependent = true;
        DOTween.timeScale = 1.0f;
        group.DOFade(1.0f, fadeDuration);
        audioSource.PlayOneShot(upgradeStartSound);
    }

    public void Start()
    {
        for (int i = 0; i < upgradeEnhanceViews.Count; i++)
        {
            upgradeEnhanceViews[i].SelectEnhanceHandler = SelectEnhance;
        }
    }

    public void SetUpgradeWindow(List<Enhance> enhances)
    {
        foreach (var enhance in enhances)
        {
            Debug.Log(enhance.name);
        }
        SetUpgradeViews(enhances);
    }

    private void SetUpgradeViews(List<Enhance> enhances)
    {
        int count = enhances.Count;
        Debug.Assert(count <= upgradeEnhanceViews.Count);

        for (int i = 0; i < count; i++)
        {
            upgradeEnhanceViews[i].gameObject.SetActive(true);
            upgradeEnhanceViews[i].DrawEnhance(enhances[i]);
        }
        for (int i = count; i < upgradeEnhanceViews.Count; i++)
        {
            upgradeEnhanceViews[i].gameObject.SetActive(false);
        }
    }

    private void SelectEnhance(Enhance enhance)
    {
        EnhanceSystem.Instance.ApplyEnhance(enhance);

        // Animation
        Sequence s = DOTween.Sequence();
        s.Append(group.DOFade(0.0f, fadeDuration));

        s.onComplete += () =>
        {
            // 다음 라운드 시작
            RoundSystem.Instance.NextRound();
            WindowSystem.Instance.CloseWindow(false);
        };
    }
}
