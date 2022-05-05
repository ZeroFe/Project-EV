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
    /// <param name="pathName">유저가 선택한 패스 + 해당 파일 이름</param>
    /// <param name="resourceFile">베이스가 되는 스크립트 템플릿</param>
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        Debug.Log($"Create Object Path Name : {pathName}, Resource File : {resourceFile}");
        // 생성한 파일의 ID를 받아서 내가 원하는 Type으로 형변환
        var item = (EquipItem)EditorUtility.InstanceIDToObject(instanceId);

        var fileName = Path.GetFileNameWithoutExtension(pathName);
        // 여기서 pathName을 강제로 바꾸면 생성 경로를 강제지정할 수 있다
        pathName = "Assets/Item/" + fileName + ".asset";

        AssetDatabase.CreateAsset(item, pathName);
        AssetDatabase.ImportAsset(pathName);
        var asset = AssetDatabase.LoadAssetAtPath<EquipItem>(pathName);
        ProjectWindowUtil.ShowCreatedAsset(asset);
    }
}
