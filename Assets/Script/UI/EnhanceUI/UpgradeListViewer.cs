using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeListViewer : Singleton<UpgradeListViewer>
{
    [SerializeField] private List<UpgradeEnhanceViewer> upgradeEnhanceViewers;

    public void SetUpgradeLists(List<Item> enhances)
    {
        int count = enhances.Count;
        Debug.Assert(count > upgradeEnhanceViewers.Count);

        for (int i = 0; i < count; i++)
        {
            upgradeEnhanceViewers[i].gameObject.SetActive(true);
            upgradeEnhanceViewers[i].DrawEnhance(enhances[i]);
        }
        for (int i = count; i < upgradeEnhanceViewers.Count; i++)
        {
            upgradeEnhanceViewers[i].gameObject.SetActive(false);
        }
    }

    public void SelectEnhance(Item enhance)
    {
        ItemManager.Instance.EndEnhance(enhance);
        gameObject.SetActive(false);
    }

    
}
