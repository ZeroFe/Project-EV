using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemCreator
{
    [MenuItem("Magimaker/Create Item")]
    static void CreateItem()
    {
        var asset = ScriptableObject.CreateInstance<EquipItem>();
        asset.ID = 789456;

        var path = "Assets/Resources/Item/NewItem.asset";
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.ImportAsset(path);
        ProjectWindowUtil.ShowCreatedAsset(asset);
    }
}
