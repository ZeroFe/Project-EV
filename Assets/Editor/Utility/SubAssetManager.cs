using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class SubAssetManager
{
    /// <summary>
    /// 선택한 Prefab에 T 타입의 아이템(derived type)을 넣는다
    /// ex) 아이템에 효과(UseEffect)를 넣음
    /// </summary>
    /// <typeparam name="T">넣을 아이템의 타입</typeparam>
    /// <param name="derivedType">T로부터 파생된 타입</param>
    /// <returns>T타입의 subAsset</returns>
    public static T AddSubAsset<T>(Type derivedType) where T : ScriptableObject
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);

        var newAsset = ScriptableObject.CreateInstance(derivedType);
        var subAssetName = Path.GetFileNameWithoutExtension(path) + "_" + derivedType.Name;
        newAsset.name = subAssetName;
        newAsset.hideFlags = HideFlags.HideInHierarchy;
        AssetDatabase.AddObjectToAsset(newAsset, path);
        AssetDatabase.SaveAssets();
        return (T)newAsset;
    }

    [MenuItem("Assets/Remove Obsoleted")]
    /// <summary>
    /// 해당 Asset 내에서 사용하지 않는 SubAsset을 모두 제거한다
    /// SubAsset이 SubAsset을 참조하는 경우는 사용하는 SubAsset으로 친다
    /// 단, 타 Asset이 해당 Asset의 SubAsset을 참조하는 경우는 제외한다
    /// </summary>
    public static void RemoveObsoletedSubAsset()
    {
        //var path = SelectedAssetPicker.GetLastSelectedAssetPath();
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        var assets = AssetDatabase.LoadAllAssetsAtPath(path);

        HashSet<UnityEngine.Object> hashSet = new HashSet<UnityEngine.Object>();
        foreach (var asset in assets)
        {
            var fieldInfos = asset.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var fieldInfo in fieldInfos)
            {
                var value = fieldInfo.GetValue(asset);
                if (value == null)
                {
                    continue;
                }
                var type = value.GetType();
                if (type.IsArray && type.GetElementType().IsAssignableToGenericType(typeof(ScriptableObjectReference<>)))
                {
                    foreach (var elem in (object[])value)
                    {
                        ScriptableObject so = (ScriptableObject) elem.GetType().GetMethod("GetReference")?.Invoke(elem, null);
                        if (!hashSet.Contains(so))
                        {
                            hashSet.Add(so);
                        }
                    }
                }
            }
        }

        foreach (var item in assets)
        {
            if (AssetDatabase.IsSubAsset(item))
            {
                if (!hashSet.Contains(item))
                {
                    AssetDatabase.RemoveObjectFromAsset(item);
                }
            }
        }
        AssetDatabase.SaveAssets();


        //if (  typeof(ScriptableObjectReference[]).IsInstanceOfType())
        //{
        //    foreach (var elem in (ScriptableObjectReference[])fieldInfo.GetValue(asset))
        //    {
        //        if (!hashSet.Contains(elem))
        //        {
        //            hashSet.Add(elem);
        //        }
        //    }
        //}
    }

    [MenuItem("Assets/Hide Sub Assets")]
    static void HideSubAssets()
    {
        var activeObj = Selection.activeObject;
        if (!activeObj)
        {
            Debug.LogWarning($"can't hide sub assets because object is not selected");
            return;
        }
        SetSubAssetVisible(AssetDatabase.GetAssetPath(activeObj), false);
    }

    [MenuItem("Assets/Reveal Sub Assets")]
    static void RevealSubAssets()
    {
        var activeObj = Selection.activeObject;
        if (!activeObj)
        {
            Debug.LogWarning($"can't reveeal sub assets because object is not selected");
            return;
        }
        SetSubAssetVisible(AssetDatabase.GetAssetPath(activeObj), true);
    }

    /// <summary>
    /// path에 있는 SubAsset들을 isVisible에 따라 hierarchy에서 보이거나 보이지 않게 한다
    /// </summary>
    /// <param name="path">Asset의 경로</param>
    /// <param name="isVisible">SubAsset의 HideFlags.HideInHierarchy 추가 / 해제</param>
    private static void SetSubAssetVisible(string path, bool isVisible)
    {
        var assets = AssetDatabase.LoadAllAssetsAtPath(path);
        foreach (var asset in assets)
        {
            if (!AssetDatabase.IsMainAsset(asset))
            {
                if (isVisible)
                {
                    asset.hideFlags = HideFlags.None;
                }
                else
                {
                    asset.hideFlags = HideFlags.HideInHierarchy;
                }
            }
        }
        AssetDatabase.ImportAsset(path);
    }
}

//internal class ScriptableObjectReference<T>
//{
//}