using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

public class DoCreateObjectAsset : EndNameEditAction
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="instanceId"></param>
    /// <param name="pathName">������ ������ �н� + �ش� ���� �̸�</param>
    /// <param name="resourceFile">���̽��� �Ǵ� ��ũ��Ʈ ���ø�</param>
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        Debug.Log($"Create Object Path Name : {pathName}, Resource File : {resourceFile}");
        // ������ ������ ID�� �޾Ƽ� ���� ���ϴ� Type���� ����ȯ
        var item = (EquipItem)EditorUtility.InstanceIDToObject(instanceId);

        var fileName = Path.GetFileNameWithoutExtension(pathName);
        // ���⼭ pathName�� ������ �ٲٸ� ���� ��θ� ���������� �� �ִ�
        pathName = "Assets/Item/" + fileName + ".asset";

        AssetDatabase.CreateAsset(item, pathName);
        AssetDatabase.ImportAsset(pathName);
        var asset = AssetDatabase.LoadAssetAtPath<EquipItem>(pathName);
        ProjectWindowUtil.ShowCreatedAsset(asset);
    }
}
