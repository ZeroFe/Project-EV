using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWindow : Singleton<UpgradeWindow>
{
    [SerializeField] private List<UpgradeEnhanceView> upgradeEnhanceViews;

    public void Awake()
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
        Debug.Assert(count > upgradeEnhanceViews.Count);

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
        // 다음 라운드 시작
        RoundSystem.Instance.NextRound();

        WindowSystem.Instance.CloseWindow(false);
    }
}
