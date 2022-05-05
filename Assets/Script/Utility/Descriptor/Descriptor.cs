using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class Descriptor : ISerializationCallbackReceiver
{
    // Descriptor Ŭ������ ���� ��ü
    private DescriptableObject owner;
    // owner�� ���� IExplainable �������� �����ϴ� Dictionary
    private Dictionary<string, DescriptableObject> ownerChildDescriptorDict = new Dictionary<string, DescriptableObject>();

    [Tooltip("���� ���� ������ ���� �ռ� ���� ���ڿ�\n" +
             "��������� ���� �ڵ����� ���� ���")]
    [TextArea(3, 5)]
    public string text;

    [ReadOnlyTextArea(4, 10)]
    public string description;

    static readonly char[] delimeterChars = { '{', '}' };
    /// <summary>
    /// text�� ������� ���� ������ ����� �Լ�
    /// </summary>
    public string Description
    {
        get
        {
            // ����ִ� ���� ���� �ڵ� ���� �ؽ�Ʈ�� ����Ѵ�
            if (string.IsNullOrEmpty(text))
            {
                return owner.Explain();
            }

            // ������� �ʴٸ� �ռ� ���� ���ڿ��� �����
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
            // Reference Type�� �迭�� ��� ó��
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

        // �̹� text�� ä�����ִ� ��쿡�� ������Ʈ�ؾ���
        description = Description;
    }

    private void UpdateChildDescriptor()
    {

    }

    // ISerializationCallbackReceiver ������ ������ �Լ��� ������� ����
    public void OnBeforeSerialize() {}

    /// <summary>
    /// 
    /// �������� ��� Update ���
    /// </summary>
    public void OnAfterDeserialize()
    {
        Debug.Log("update!!!");
        description = Description;
    }
}