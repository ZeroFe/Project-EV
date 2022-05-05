using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Magimaker에서 사용하는 Script Templete를 
/// </summary>
public class ScriptFormatCreater
{
    [MenuItem("Assets/Create/MagiMaker Script/MonoBehavior Script", false, 10)]
    static void CreateExampleAssets()
    {
        //CreateCsScriptByTemplete("64-C# Script-NewMagimakerScript.cs.txt", "NewBehaviourScript");
        CreateCsScriptByTemplete2("64-C# Script-NewMagimakerScript.cs.txt", "NewBehaviourScript");
    }

    [MenuItem("Assets/Create/MagiMaker Script/Normal Script", false, 9)]
    static void CreateNormalScript()
    {
        CreateCsScriptByTemplete2("66-C# Script-NewUtilityScript.cs.txt", "NewNormalScript");
    }

    static void CreateEditorScript()
    {

    }

    static void CreateCsScriptByTemplete(string templeteName, string baseScriptName)
    {
        string contentsPath = EditorApplication.applicationContentsPath;
        Debug.Log(contentsPath);
        var resourceFile = Path.Combine(contentsPath,
            "Resources/ScriptTemplates/", templeteName);
        Debug.Log(resourceFile);
        //cs 는 "cs Script Icon"
        //js 는 "js Script Icon"
        Texture2D csIcon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
        var endNameEditAction = ScriptableObject.CreateInstance<DoCreateScriptAsset>();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endNameEditAction,
                                    baseScriptName + ".cs", csIcon, resourceFile);
    }

    /// <summary>
    /// 프로젝트에 있는 스크립트 템플릿으로부터 스크립트를 만든다
    /// </summary>
    /// <param name="templeteName"></param>
    /// <param name="baseScriptName"></param>
    static void CreateCsScriptByTemplete2(string templeteName, string baseScriptName)
    {
        //string dataPath = Application.dataPath.Replace(@"\", "/");
        string dataPath = Application.dataPath;
        var resourceFile = Path.Combine(@dataPath, "ScriptTemplates/", templeteName);
        Debug.Log(resourceFile);
        //cs 는 "cs Script Icon"
        //js 는 "js Script Icon"
        Texture2D csIcon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
        var endNameEditAction = ScriptableObject.CreateInstance<DoCreateScriptAsset>();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endNameEditAction,
                                    baseScriptName + ".cs", csIcon, resourceFile);
    }
}
