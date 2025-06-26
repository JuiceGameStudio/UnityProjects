using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SaveAttribute))]
public class SaveAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        EditorGUI.PropertyField(position, property, GUIContent.none);

        EditorGUI.EndProperty();
    }
}
