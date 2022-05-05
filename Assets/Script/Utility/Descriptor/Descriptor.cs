using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class Descriptor : ISerializationCallbackReceiver
{
    // Descriptor 클래스를 가진 객체
    private DescriptableObject owner;
    // owner가 가진 IExplainable 변수들을 저장하는 Dictionary
    private Dictionary<string, DescriptableObject> ownerChildDescriptorDict = new Dictionary<string, DescriptableObject>();

    [Tooltip("설명 문구 생성을 위한 합성 서식 문자열\n" +
             "비어있으면 기존 자동생성 문구 출력")]
    [TextArea(3, 5)]
    public string text;

    [ReadOnlyTextArea(4, 10)]
    public string description;

    static readonly char[] delimeterChars = { '{', '}' };
    /// <summary>
    /// text를 기반으로 설명 문구를 만드는 함수
    /// </summary>
    public string Description
    {
        get
        {
            // 비어있는 경우는 기존 자동 설명 텍스트를 사용한다
            if (string.IsNullOrEmpty(text))
            {
                return owner.Explain();
            }

            // 비어있지 않다면 합성 서식 문자열을 만든다
            string list = "";

            string[] words = text.Split(delimeterChars);
            foreach (var word in words)
            {
                list += ownerChildDescriptorDict.TryGetValue(word, out var value)
                    ? value.descriptor.description
                    : word;
            }
            return list;
        }
    }

    public void InitDescriptor(object ownerObj)
    {
        if (ownerObj is not DescriptableObject)
        {
            Debug.LogError($"Error : {ownerObj.ToString()} is not DescriptableObject");
            return;
        }

        owner = ownerObj as DescriptableObject;
        var ownerType = owner.GetType();
        var fieldInfos = ownerType.GetFields();
        foreach (var fieldInfo in fieldInfos)
        {
            // Reference Type의 배열인 경우 처리
            var value = fieldInfo.GetValue(ownerObj);
            if (value == null)
            {
                continue;
            }
            var type = value.GetType();
            if (type.IsArray && type.GetElementType().IsAssignableToGenericType(typeof(ScriptableObjectReference<>)))
            {
                var objArr = (object[]) value;
                for (int i = 0; i < objArr.Length; i++)
                {
                    ScriptableObject so = (ScriptableObject)objArr[i].GetType().GetMethod("GetReference")?.Invoke(objArr[i], null);
                    if (so is DescriptableObject)
                    {
                        var elem = so as DescriptableObject;
                        ownerChildDescriptorDict.Add(fieldInfo.Name + $"[{i}]", elem);
                    }
                }
            }
        }

        // 이미 text가 채워져있는 경우에도 업데이트해야함
        description = Description;
    }

    private void UpdateChildDescriptor()
    {

    }

    // ISerializationCallbackReceiver 용으로 생성한 함수고 사용하지 않음
    public void OnBeforeSerialize() {}

    /// <summary>
    /// 
    /// 지연쓰기 방식 Update 사용
    /// </summary>
    public void OnAfterDeserialize()
    {
        Debug.Log("update!!!");
        description = Description;
    }
}