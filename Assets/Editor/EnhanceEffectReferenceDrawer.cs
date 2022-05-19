using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EnhanceEffectReference))]
public class EnhanceEffectReferenceDrawer : PropertyDrawer
{
    private static string[] typeNames;
    private static Type[] types;

    private static GUIStyle popupStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ScriptableObjectReferenceDrawer.UpdatePopupStyle(ref popupStyle);
        ScriptableObjectReferenceDrawer.UpdateTypeInfos<EnhanceEffect>(ref typeNames, ref types);
        ScriptableObjectReferenceDrawer.OnGUI<EnhanceEffect>(position, property, label, popupStyle, typeNames, types);
    }
}