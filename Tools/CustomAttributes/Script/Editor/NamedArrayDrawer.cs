using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NamedArrayAttribute))]
public class NamedArrayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        NamedArrayAttribute namedArray = attribute as NamedArrayAttribute;
        int index = int.Parse(property.propertyPath.Split('[', ']')[1]);
        label.text = namedArray.names[index];
        EditorGUI.PropertyField(position, property, label, true);
    }
}
