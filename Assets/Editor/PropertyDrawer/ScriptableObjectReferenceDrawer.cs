using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectReferenceDrawer
{
    public static void UpdatePopupStyle(ref GUIStyle popupStyle)
    {
        if (popupStyle == null)
        {
            Debug.Log($"Setting Popup Style!");
            // '...' 모양의 팝업 버튼 스타일 생성
            popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
        }
    }

    public static void UpdateTypeInfos<T>(ref string[] typeNames, ref Type[] types)
    {
        if (typeNames == null || types == null)
        {
            Debug.Log($"update {typeof(T).Name} types");
            List<string> typeNameList = new List<string>();
            List<Type> typeList = new List<Type>();
            typeNameList.Add("not created");
            foreach (var item in DerivedClassFinder.FindAllDerivedTypes<T>())
            {
                typeNameList.Add(item.Name);
                typeList.Add(item);
            }
            typeNames = typeNameList.ToArray();
            types = typeList.ToArray();
        }
    }

    public static void OnGUI<T>(Rect position, SerializedProperty property, GUIContent label, 
        GUIStyle popupStyle, string[] typeNames, Type[] types)
        where T : ScriptableObject
    {
        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Get properties
        SerializedProperty reference = property.FindPropertyRelative("reference");

        // Calculate rect for configuration button
        Rect buttonRect = new Rect(position);
        buttonRect.yMin += popupStyle.margin.top;
        buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
        position.xMin = buttonRect.xMax;

        // Store old indent level and set it to 0, the PrefixLabel takes care of it
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        int result = EditorGUI.Popup(buttonRect, 0, typeNames, popupStyle);

        if (result != 0)
        {
            Debug.Log($"Create SubItem {types[result - 1].Name}");
            reference.objectReferenceValue = SubAssetManager.AddSubAsset<T>(types[result-1]);
        }

        EditorGUI.ObjectField(position, reference, typeof(T), GUIContent.none);

        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}