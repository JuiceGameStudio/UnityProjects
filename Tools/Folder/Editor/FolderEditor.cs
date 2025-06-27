using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Folder))]
public class FolderEditor : Editor
{
    [MenuItem("GameObject/Folder", false, 0)]
    static void CreateCustomGameObject()
    {
        GameObject go = new GameObject("Folder");
        go.AddComponent<Folder>();
    }

    public override void OnInspectorGUI()
    {
        Transform transform = (target as Folder).transform;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
