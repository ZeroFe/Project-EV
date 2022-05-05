using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

public class DoCreateScriptAsset : EndNameEditAction
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceId"></param>
    /// <param name="pathName">유저가 선택한 패스 + 해당 파일 이름</param>
    /// <param name="resourceFile">베이스가 되는 스크립트 템플릿</param>
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        Debug.Log($"Path Name : {pathName}, Resource File : {resourceFile}");
        var text = File.ReadAllText(resourceFile);

        var className = Path.GetFileNameWithoutExtension(pathName);

        //반각 스페이스를 제외
        className = className.Replace(" ", "");

        //다른 파라메터에 대해서 알고 싶다면
        //15장「ScriptTemplates」 을 참조해주세요
        text = text.Replace("#SCRIPTNAME#", className);

        //UTF8 에 BOM 붙여서 저장
        var encoding = new UTF8Encoding(true, false);

        // 여기서 pathName을 강제로 바꾸면 생성 경로를 강제지정할 수 있다
        //pathName = "Assets/Script/DoCreate/" + className + ".cs";
        File.WriteAllText(pathName, text, encoding);

        AssetDatabase.ImportAsset(pathName);
        var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
        ProjectWindowUtil.ShowCreatedAsset(asset);
    }
}
