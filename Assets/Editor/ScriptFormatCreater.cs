using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Magimaker���� ����ϴ� Script Templete�� 
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
        //cs �� "cs Script Icon"
        //js �� "js Script Icon"
        Texture2D csIcon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
        var endNameEditAction = ScriptableObject.CreateInstance<DoCreateScriptAsset>();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endNameEditAction,
                                    baseScriptName + ".cs", csIcon, resourceFile);
    }

    /// <summary>
    /// ������Ʈ�� �ִ� ��ũ��Ʈ ���ø����κ��� ��ũ��Ʈ�� �����
    /// </summary>
    /// <param name="templeteName"></param>
    /// <param name="baseScriptName"></param>
    static void CreateCsScriptByTemplete2(string templeteName, string baseScriptName)
    {
        //string dataPath = Application.dataPath.Replace(@"\", "/");
        string dataPath = Application.dataPath;
        var resourceFile = Path.Combine(@dataPath, "ScriptTemplates/", templeteName);
        Debug.Log(resourceFile);
        //cs �� "cs Script Icon"
        //js �� "js Script Icon"
        Texture2D csIcon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
        var endNameEditAction = ScriptableObject.CreateInstance<DoCreateScriptAsset>();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endNameEditAction,
                                    baseScriptName + ".cs", csIcon, resourceFile);
    }
}
