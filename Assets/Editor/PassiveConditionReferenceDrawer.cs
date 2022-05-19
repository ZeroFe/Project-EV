using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PassiveConditionReference))]
public class PassiveConditionReferenceDrawer : PropertyDrawer
{
    private static string[] typeNames;
    private static Type[] types;

    private static GUIStyle popupStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ScriptableObjectReferenceDrawer.UpdatePopupStyle(ref popupStyle);
        ScriptableObjectReferenceDrawer.UpdateTypeInfos<PassiveCondition>(ref typeNames, ref types);
        ScriptableObjectReferenceDrawer.OnGUI<PassiveCondition>(position, property, label, popupStyle, typeNames, types);
    }
}