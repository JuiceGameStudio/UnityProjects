using UnityEngine;
using UnityEditor;
using CustomSystem.Debug;

[CustomEditor(typeof(MonoBehaviour), true)]
public class GlobalDebugSystemEditor : Editor
{

    public override void OnInspectorGUI()
    {
        var targetMonoBehaviour = target as MonoBehaviour;

        if (targetMonoBehaviour is IDebugSystem debugSystem)
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug System", EditorStyles.boldLabel);

            debugSystem.localDebugEnabled = EditorGUILayout.Toggle("Debug Local Enabled", debugSystem.localDebugEnabled);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
        else
        {
            base.OnInspectorGUI();
        }
    }
}
