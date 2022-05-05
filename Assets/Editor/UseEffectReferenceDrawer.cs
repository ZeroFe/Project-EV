using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(UseEffectReference))]
public class UseEffectReferenceDrawer : PropertyDrawer
{
    private static string[] useEffectTypeNames;
    private static Type[] useEffectTypes;

    private static GUIStyle popupStyle;
    /*
    public static void UpdateTypeInfos()
    {
        Debug.Log($"update use effect types");
        List<string> typeNames = new List<string>();
        List<Type> types = new List<Type>();
        typeNames.Add("not created");
        types.Add(typeof(UseEffect));
        foreach (var item in DerivedClassFinder.FindAllDerivedTypes<UseEffect>())
        {
            typeNames.Add(item.Name);
            types.Add(item);
        }
        useEffectTypeNames = typeNames.ToArray();
        useEffectTypes = types.ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (popupStyle == null)
        {
            Debug.Log($"Setting Popup Style!");
            // '...' 모양의 팝업 버튼 스타일 생성
            popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
        }

        if (useEffectTypeNames == null || useEffectTypes == null)
        {
            Debug.Log($"update");
            UpdateTypeInfos();
            return;
        }

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

        int result = EditorGUI.Popup(buttonRect, 0, useEffectTypeNames, popupStyle);

        if (result != 0)
        {
            Debug.Log($"Create SubItem UseEffect");
            reference.objectReferenceValue = SubAssetManager.AddSubAsset<UseEffect>(useEffectTypes[result]);
        }

        EditorGUI.ObjectField(position, reference, typeof(UseEffect), GUIContent.none);

        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
    */

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ScriptableObjectReferenceDrawer.UpdatePopupStyle(ref popupStyle);
        //if (popupStyle == null)
        //{
        //    Debug.Log($"Setting Popup Style!");
        //    // '...' 모양의 팝업 버튼 스타일 생성
        //    popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
        //    popupStyle.imagePosition = ImagePosition.ImageOnly;
        //}
        ScriptableObjectReferenceDrawer.UpdateTypeInfos<UseEffect>(ref useEffectTypeNames, ref useEffectTypes);
        ScriptableObjectReferenceDrawer.OnGUI<UseEffect>(position, property, label, popupStyle, useEffectTypeNames, useEffectTypes);
    }
}