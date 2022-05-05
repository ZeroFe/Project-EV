using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 단일 객체만 필요한 EnumScriptableObject를 자동으로 생성하는 클래스
/// 
/// </summary>
public class EnumScriptableObjectAutoCreator
{
    private static readonly string ESO_PATH = "Assets/EnumScriptableObject";

    [InitializeOnLoadMethod]
    public static void SyncESO()
    {
        Debug.Log("sync eso");

        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/EnumScriptableObject");
        if (di.Exists)
        {
            //Debug.Log("EnumScriptableObject Folder exists");
        }
        else
        {
            Debug.Log("EnumScriptableObject Folder not exists");
            // 없다면 폴더 새로 생성
            AssetDatabase.CreateFolder("Assets", "EnumScriptableObject");
        }
        

        var folders = AssetDatabase.GetSubFolders("Assets/EnumScriptableObject");
        for (int i = 0; i < folders.Length; i++)
        {
            folders[i] = Path.GetFileNameWithoutExtension(folders[i]);
        }

        var esoList = DerivedClassFinder.FindAllChildrenTypes<EnumScriptableObject>();
        foreach (var eso in esoList)
        {
            // 폴더 있는지 검사
            if (!folders.Contains(eso.Name))
            {
                Debug.Log("Create Folder");
                // 없다면 폴더 새로 생성
                AssetDatabase.CreateFolder(ESO_PATH, eso.Name);
            }
            // 해당 폴더 경로 확인
            string path = ESO_PATH + "/" + eso.Name;
            // 해당 클래스에 새로 생긴 eso가 있는지 검사
            // dictionary 사용 : 기존 eso 제외
            var findAssets = AssetDatabase.FindAssets($"t:{eso.Name}", new string[] { ESO_PATH + "/" + eso.Name });
            var remainedDESOs = findAssets.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath(path, eso))
                .Where(b => b).ToArray();
            Dictionary<Type, object> DESODict = new Dictionary<Type, object>();
            foreach (var dEso in remainedDESOs)
            {
                DESODict.Add(dEso.GetType(), dEso);
            }
            // 그 외의 ESO 있는지 파악 -> 없다면 새로 생성
            var DeriveTypes = DerivedClassFinder.FindAllDerivedTypes(eso);
            foreach (var deriveType in DeriveTypes)
            {
                if (!DESODict.ContainsKey(deriveType))
                {
                    Debug.Log($"{deriveType.Name} is not created");
                    CreateESO(deriveType, path);
                }
            }
        }
    }

    private static void CreateESO(Type derivedESOType, string path)
    {
        var asset = ScriptableObject.CreateInstance(derivedESOType);
        path += "/" + derivedESOType.Name + ".asset";
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.ImportAsset(path);
    }
}