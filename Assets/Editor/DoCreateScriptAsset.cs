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
    /// <param name="pathName">������ ������ �н� + �ش� ���� �̸�</param>
    /// <param name="resourceFile">���̽��� �Ǵ� ��ũ��Ʈ ���ø�</param>
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        Debug.Log($"Path Name : {pathName}, Resource File : {resourceFile}");
        var text = File.ReadAllText(resourceFile);

        var className = Path.GetFileNameWithoutExtension(pathName);

        //�ݰ� �����̽��� ����
        className = className.Replace(" ", "");

        //�ٸ� �Ķ���Ϳ� ���ؼ� �˰� �ʹٸ�
        //15�塸ScriptTemplates�� �� �������ּ���
        text = text.Replace("#SCRIPTNAME#", className);

        //UTF8 �� BOM �ٿ��� ����
        var encoding = new UTF8Encoding(true, false);

        // ���⼭ pathName�� ������ �ٲٸ� ���� ��θ� ���������� �� �ִ�
        //pathName = "Assets/Script/DoCreate/" + className + ".cs";
        File.WriteAllText(pathName, text, encoding);

        AssetDatabase.ImportAsset(pathName);
        var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
        ProjectWindowUtil.ShowCreatedAsset(asset);
    }
}
