using UnityEditor;
using UnityEngine;
using CustomAttributes;

/// <summary>
/// Custom property drawer pour créer une liste des différents tag dans l'inspecteur
/// </summary>
[CustomPropertyDrawer(typeof(TagAttribute))]
public class TagPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}