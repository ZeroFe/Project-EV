using System.IO;
using UnityEngine;
using UnityEditor;
using System.Text;

public class ScriptTemplateMaker
{
    [MenuItem("Assets/To Script Template")]
    static void DoSomething()
    {
        // get selected file path
        var activeObj = Selection.activeObject;
        if (!activeObj)
        {
            Debug.LogWarning($"script not selected. can't create script template");
            return;
        }
        var assetPath = AssetDatabase.GetAssetPath(activeObj);
        // get class name and absoulute path
        var className = Path.GetFileNameWithoutExtension(assetPath);
        Debug.Log($"{className}");
        string dataPath = Application.dataPath;
        dataPath = dataPath.Remove(dataPath.LastIndexOf('/'));
        Debug.Log($"{dataPath}");
        var resourceFile = Path.Combine(@dataPath, assetPath);
        Debug.Log(resourceFile);
        
        // 해당 경로의 파일을 읽어 템플릿 형식의 텍스트로 변환한다
        var text = File.ReadAllText(resourceFile);

        text = text.Replace(className, "#SCRIPTNAME#");

        //UTF8 에 BOM 붙여서 저장
        var encoding = new UTF8Encoding(true, false);

        // 변환한 텍스트를 ScriptTemplates에 쓴다
        StringBuilder sb = new StringBuilder();
        sb.Append("Assets/ScriptTemplates/");
        sb.Append("64-C# Script-New");
        sb.Append(className);
        sb.Append("Script.cs.txt");
        var pathName = sb.ToString();
        File.WriteAllText(pathName, text, encoding);

        AssetDatabase.ImportAsset(pathName);
    }
}