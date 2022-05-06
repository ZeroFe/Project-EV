using System.Linq;
using UnityEngine;
using UnityEditor;

public static class ScriptableObjectIdMake
{
    [MenuItem("Magimaker/Make Item ID")]
    static void MakeItemID()
    {
        var findAssets = AssetDatabase.FindAssets("t:Item", new string[] { "Assets/Item" });
        var availableItems = findAssets.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<Item>(path))
            .Where(b => b).ToArray();
        for (int i = 0; i < findAssets.Length; i++)
        {
            //availableItems[i].ID = findAssets[i].GetHashCode();
            EditorUtility.SetDirty(availableItems[i]);
        }
    }

    [MenuItem("Magimaker/Make Persist Infomation ID")]
    static void MakePersistInfoID()
    {
        var findAssets = AssetDatabase.FindAssets("t:PersistInfo", new string[] { "Assets/UseEffect/PersistInfo" });
        var availableItems = findAssets.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<PersistInfo>(path))
            .Where(b => b).ToArray();
        for (int i = 0; i < findAssets.Length; i++)
        {
            availableItems[i].ID = findAssets[i].GetHashCode();
            EditorUtility.SetDirty(availableItems[i]);
        }
    }
}
