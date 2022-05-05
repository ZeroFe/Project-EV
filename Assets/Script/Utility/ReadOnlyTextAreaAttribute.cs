using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// string을 readonly형 TextArea로 만들 때 사용하는 attribute
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class ReadOnlyTextAreaAttribute : PropertyAttribute
{
    public readonly int minLines;
    public readonly int maxLines;

    public ReadOnlyTextAreaAttribute(int min, int max)
    {
        minLines = min;
        maxLines = max;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyTextAreaAttribute), true)]
public class ReadOnlyTextAreaAttributeDrawer : PropertyDrawer
{
    private Vector2 scrollPos;

    // Necessary since some properties tend to collapse smaller than their content
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ReadOnlyTextAreaAttribute textAreaAttribute = (ReadOnlyTextAreaAttribute)attribute;
        float minLineHeight = textAreaAttribute.minLines * EditorGUIUtility.singleLineHeight;
        return EditorGUI.GetPropertyHeight(property, label, true) + minLineHeight;
    }

    // Draw a disabled property field
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ReadOnlyTextAreaAttribute textAreaAttribute = (ReadOnlyTextAreaAttribute)attribute;
        float minLineHeight = textAreaAttribute.minLines * EditorGUIUtility.singleLineHeight;
        float maxLineHeight = textAreaAttribute.maxLines * EditorGUIUtility.singleLineHeight;

        var val = property.stringValue;
        var descriptionLabelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(descriptionLabelRect, property.name);

        // Make Description Area Field
        var descriptionAreaRect = new Rect(position.x, position.y + descriptionLabelRect.height, position.width, minLineHeight);
        scrollPos = GUI.BeginScrollView(descriptionAreaRect, scrollPos, new Rect(position.x, 0, position.width - 20, maxLineHeight));
        // 세로 사이즈가 비정상적으로 크므로 수정 필요
        var descriptionContentRect = new Rect(position.x, 0, position.width - 14, maxLineHeight);
        GUI.Box(descriptionContentRect, val, EditorStyles.textArea);
        GUI.EndScrollView();
    }
}
#endif