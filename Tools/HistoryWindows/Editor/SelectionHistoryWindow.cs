using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SelectionHistoryWindow : EditorWindow
{
    List<Object> selectionHistory = new List<Object>();
    List<string> favoriteGuids = new List<string>();
    int maxHistorySize = 20;
    bool trackHierarchyObjects = true;
    bool trackProjectAssets = true;
    bool showingHistory = true;
    bool showParemeters;

    const string FavoritesKey = "SelectionHistoryWindow.Favorites";

    [MenuItem("Tools/Selection History")]
    public static void ShowWindow()
    {
        GetWindow<SelectionHistoryWindow>("Selection History");
    }

    void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChanged;
        LoadFavorites();
    }

    void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
        SaveFavorites();
    }

    void OnSelectionChanged()
    {
        if (Selection.activeObject != null)
        {
            bool shouldTrack = false;
            if (trackHierarchyObjects && Selection.activeObject is GameObject)
            {
                shouldTrack = true;
            }
            else if (trackProjectAssets && !(Selection.activeObject is GameObject))
            {
                shouldTrack = true;
            }

            if (shouldTrack && !selectionHistory.Contains(Selection.activeObject))
            {
                selectionHistory.Insert(0, Selection.activeObject);

                if (selectionHistory.Count > maxHistorySize)
                {
                    selectionHistory.RemoveAt(selectionHistory.Count - 1);
                }

                Repaint();
            }
        }
    }

    void LoadFavorites()
    {
        favoriteGuids.Clear();
        string favoritesData = EditorPrefs.GetString(FavoritesKey, "");
        if (!string.IsNullOrEmpty(favoritesData))
        {
            favoriteGuids.AddRange(favoritesData.Split(';'));
        }
    }

    void SaveFavorites()
    {
        string favoritesData = string.Join(";", favoriteGuids.ToArray());
        EditorPrefs.SetString(FavoritesKey, favoritesData);
    }

    void OnGUI()
    {
        showParemeters = EditorGUILayout.Foldout(showParemeters, "Paremeters", true);
        if (showParemeters)
        {
            maxHistorySize = EditorGUILayout.IntField("Max History Size", maxHistorySize);
            trackHierarchyObjects = EditorGUILayout.Toggle("Track Hierarchy Objects", trackHierarchyObjects);
            trackProjectAssets = EditorGUILayout.Toggle("Track Project Assets", trackProjectAssets);

            GUILayout.Space(10);
            GUILayout.Label("Tips :", EditorStyles.miniLabel);
            GUILayout.Label("Left click for drag & drop", EditorStyles.miniLabel);
            GUILayout.Label("Right click for hightlight", EditorStyles.miniLabel);
        }

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Show History"))
        {
            showingHistory = true;
        }
        if (GUILayout.Button("Show Favorites"))
        {
            showingHistory = false;
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (showingHistory)
        {
            if (GUILayout.Button("Clear History"))
            {
                selectionHistory.Clear();
            }

            GUILayout.Space(10);

            DisplaySelectionList(selectionHistory);
        }
        else
        {
            if (GUILayout.Button("Clear Favorite"))
            {
                favoriteGuids.Clear();
                SaveFavorites();
            }

            GUILayout.Space(10);

            DisplayFavoriteList();

            GUILayout.Label("Drag objects here to add to Favorite", EditorStyles.helpBox);
            Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandHeight(true));

            Event evt = Event.current;
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            string path = AssetDatabase.GetAssetPath(draggedObject);
                            string guid = AssetDatabase.AssetPathToGUID(path);

                            if (!favoriteGuids.Contains(guid))
                            {
                                favoriteGuids.Add(guid);
                            }
                        }

                        SaveFavorites();
                    }
                    Event.current.Use();
                    break;
            }


        }
    }

    private void DisplaySelectionList(List<Object> list)
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.alignment = TextAnchor.MiddleLeft;

        GUIContent starContent = EditorGUIUtility.IconContent("Favorite");

        List<Object> filteredList = new List<Object>();

        foreach (Object obj in list)
        {
            if (!IsObjectMissing(obj))
            {
                filteredList.Add(obj);
            }
        }

        selectionHistory = filteredList;

        foreach (Object obj in selectionHistory)
        {
            EditorGUILayout.BeginHorizontal();

            GUIContent iconContent = EditorGUIUtility.ObjectContent(obj, obj.GetType());
            GUIContent labelContent = new GUIContent(iconContent);

            string guid = GetObjectGuid(obj);
            bool isFavorite = favoriteGuids.Contains(guid);

            EditorGUILayout.LabelField(labelContent, labelStyle, GUILayout.Height(20));

            Rect labelRect = GUILayoutUtility.GetLastRect();
            Rect fullRect = new Rect(labelRect.x, labelRect.y, labelRect.width, labelRect.height);

            HandleDragAndDrop(fullRect, obj);

            float windowWidth = EditorGUIUtility.currentViewWidth;
            float margin = 10f;
            float iconSize = 16f; 
            float starX = windowWidth - margin - iconSize * 2;
            float typeIconX = windowWidth - margin - iconSize; 

            if (isFavorite)
            {
                Rect starRect = new Rect(starX, fullRect.y, iconSize, iconSize);
                GUI.Label(starRect, starContent);
            }

            Texture2D typeIcon = GetObjectTypeIcon(obj);
            if (typeIcon != null)
            {
                Rect iconRect = new Rect(typeIconX, fullRect.y, iconSize, iconSize);
                GUI.Label(iconRect, typeIcon);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void DisplayFavoriteList()
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.alignment = TextAnchor.MiddleLeft;

        List<string> filteredGuids = new List<string>();

        for (int i = favoriteGuids.Count - 1; i >= 0; i--)
        {
            string guid = favoriteGuids[i];
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);

            if (!IsObjectMissing(obj))
            {
                filteredGuids.Add(guid);
            }
        }

        favoriteGuids = filteredGuids;

        foreach (string guid in favoriteGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);

            if (obj != null)
            {
                EditorGUILayout.BeginHorizontal();

                GUIContent iconContent = EditorGUIUtility.ObjectContent(obj, obj.GetType());
                GUIContent labelContent = new GUIContent(iconContent);

                EditorGUILayout.LabelField(labelContent, labelStyle, GUILayout.Height(20));

                Rect labelRect = GUILayoutUtility.GetLastRect();
                Rect fullRect = new Rect(labelRect.x, labelRect.y, labelRect.width, labelRect.height);

                HandleDragAndDrop(fullRect, obj);

                float windowWidth = EditorGUIUtility.currentViewWidth;
                float margin = 10f; 
                float buttonWidth = 25f;
                float buttonX = windowWidth - margin - buttonWidth;

                var tex = EditorGUIUtility.IconContent("d_TreeEditor.Trash").image;
                Rect buttonRect = new Rect(buttonX, fullRect.y, buttonWidth, 20);
                if (GUI.Button(buttonRect, new GUIContent(null, tex, "Delete this Favorite")))
                {
                    favoriteGuids.Remove(guid);
                    SaveFavorites();
                    Repaint();
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }

    void HandleDragAndDrop(Rect rect, Object obj)
    {
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.MouseDown:
                if (evt.button == 1 && rect.Contains(evt.mousePosition))
                {
                    Selection.activeObject = obj;
                    EditorGUIUtility.PingObject(obj);
                    evt.Use();
                }
                break;
            case EventType.MouseDrag:
                if (rect.Contains(evt.mousePosition))
                {
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new Object[] { obj };
                    DragAndDrop.StartDrag(obj.name);
                    evt.Use();
                }
                break;
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (rect.Contains(evt.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        evt.Use();
                    }
                }
                break;
        }
    }

    string GetObjectGuid(Object obj)
    {
        if (obj is GameObject)
        {
            return obj.GetInstanceID().ToString();
        }
        else
        {
            return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
        }
    }

    Texture2D GetObjectTypeIcon(Object obj)
    {
        if (obj is GameObject)
        {
            return EditorGUIUtility.IconContent("UnityEditor.SceneHierarchyWindow").image as Texture2D;
        }
        else
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(assetPath))
            {
                return EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;
            }
        }

        return null;
    }

    bool IsObjectMissing(Object obj)
    {
        if (obj == null)
        {
            return true;
        }
        else if (obj is GameObject && obj as GameObject == null)
        {
            // Check if the GameObject is missing
            return true;
        }
        return false;
    }
}
