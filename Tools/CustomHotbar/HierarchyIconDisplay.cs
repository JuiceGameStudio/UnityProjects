using UnityEngine;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif
public static class HierarchyIconDisplay
{
#if UNITY_EDITOR
    static bool hierarchyHasFocus = false;
    static EditorWindow hierarchyEditoWindow;
    static bool complexeHierarchy = true;

    static HierarchyIconDisplay()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        EditorApplication.update += OnEditorUpdate;
        complexeHierarchy = EditorPrefs.GetBool("complexeHierarchy");
    }

    public static void SetComlplexeHierarchy(bool state)
    {
        complexeHierarchy = state;
        EditorPrefs.SetBool("complexeHierarchy", state);
    }

    static void OnEditorUpdate()
    {
        if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.maximized)
        {
            return; // Exit if any window is maximized
        }

        if (hierarchyEditoWindow == null)
        {
            hierarchyEditoWindow = EditorWindow.GetWindow(Type.GetType("UnityEditor.SceneHierarchyWindow,UnityEditor"));
        }
        hierarchyHasFocus = EditorWindow.focusedWindow != null && EditorWindow.focusedWindow == hierarchyEditoWindow;
    }

    static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null) return;

        if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj) != null) return;

        Component[] components = obj.GetComponents<Component>();
        if (components == null || components.Length == 0) return;

        Component component = components.Length > 1 ? components[1] : components[0];

        Type type = component.GetType();

        GUIContent content = EditorGUIUtility.ObjectContent(complexeHierarchy ? component : null, type);
        content.text = null;
        content.tooltip = type.Name;

        if (content.image == null) return;

        bool isSelected = Selection.instanceIDs.Contains(instanceID);
        bool isHovering = selectionRect.Contains(Event.current.mousePosition);

        Color color = UnityEditorBackgroundColor.Get(isSelected, isHovering, hierarchyHasFocus);
        Rect backgroundRect = selectionRect;
        backgroundRect.width = 18.5f;
        EditorGUI.DrawRect(backgroundRect, color);

        EditorGUI.LabelField(selectionRect, content);
    }
#endif
}

public static class UnityEditorBackgroundColor
{
#if UNITY_EDITOR
    static readonly Color k_defaultColor = new Color(0.7843f, 0.7843f, 0.7843f);
    static readonly Color k_defaultProColor = new Color(0.2196f, 0.2196f, 0.2196f);

    static readonly Color k_selectedColor = new Color(0.22745f, 0.447f, 0.6902f);
    static readonly Color k_selectedProColor = new Color(0.1725f, 0.3647f, 0.5294f);

    static readonly Color k_selectedUnFocusedColor = new Color(0.68f, 0.68f, 0.68f);
    static readonly Color k_selectedUnFocusedProColor = new Color(0.3f, 0.3f, 0.3f);

    static readonly Color k_hoverColor = new Color(0.698f, 0.698f, 0.698f);
    static readonly Color k_hoverProColor = new Color(0.2706f, 0.2706f, 0.2706f);

    public static Color Get(bool isSelected, bool isHovering, bool isWindowFocused)
    {
        if (isSelected)
        {
            if (isWindowFocused)
            {
                return EditorGUIUtility.isProSkin ? k_selectedProColor : k_selectedColor;
            }
            else
            {
                return EditorGUIUtility.isProSkin ? k_selectedUnFocusedProColor : k_selectedUnFocusedColor;
            }
        }
        else if (isHovering)
        {
            return EditorGUIUtility.isProSkin ? k_hoverProColor : k_hoverColor;
        }
        else
        {
            return EditorGUIUtility.isProSkin ? k_defaultProColor : k_defaultColor;
        }
    }
#endif
}
