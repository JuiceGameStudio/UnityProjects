using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class CustomToolbarPref : EditorWindow
{
    public static SceneAsset favStartScene;
    static SceneAsset previousfavStartScene;

    public static List<SceneAsset> favScenes = new List<SceneAsset>();
    static List<string> favScenePaths = new List<string>();
    static bool showFavoriteScenes = true;
    bool havePicked = false;
    static bool complexeHierarchy;
    public static bool reloadLastScene = true;

    public static void ShowWindow()
    {
        GetWindow<CustomToolbarPref>("Toolbar Preferences");
    }

    public static void Load()
    {
        LoadFavoriteScenes();
        LoadSavedScene();
        complexeHierarchy = EditorPrefs.GetBool("complexeHierarchy");
        reloadLastScene = EditorPrefs.GetBool("reloadLastScene");
    }

    void OnGUI()
    {
        favStartScene = (SceneAsset)EditorGUILayout.ObjectField("Start Scene", favStartScene, typeof(SceneAsset), false);

        if (favStartScene != previousfavStartScene)
        {
            previousfavStartScene = favStartScene;
            SaveSceneAsset();
        }

        EditorGUILayout.Space();

        showFavoriteScenes = EditorGUILayout.Foldout(showFavoriteScenes, "Favorite Scenes", true);
        if (showFavoriteScenes)
        {
            for (int i = 0; i < favScenes.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("/\\", "Move Up"), GUILayout.Width(20)) && i > 0)
                {
                    SwapScenes(i, i - 1);
                    SaveFavoriteScenes();
                }
                if (GUILayout.Button(new GUIContent("\\/", "Move Down"), GUILayout.Width(20)) && i < favScenes.Count - 1)
                {
                    SwapScenes(i, i + 1);
                    SaveFavoriteScenes();
                }
                favScenes[i] = (SceneAsset)EditorGUILayout.ObjectField(favScenes[i], typeof(SceneAsset), false);
                if (GUILayout.Button(new GUIContent("X", "Remove"), GUILayout.Width(20)))
                {
                    favScenes.RemoveAt(i);
                    favScenePaths.RemoveAt(i);
                    SaveFavoriteScenes();
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Favorite Scene"))
            {
                havePicked = true;
                EditorGUIUtility.ShowObjectPicker<SceneAsset>(null, false, "", EditorGUIUtility.GetControlID(FocusType.Passive));
            }

            if (havePicked && Event.current.commandName == "ObjectSelectorClosed")
            {
                string scenePath = AssetDatabase.GetAssetPath(EditorGUIUtility.GetObjectPickerObject());
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

                if (sceneAsset != null)
                {
                    favScenes.Add(sceneAsset);
                    favScenePaths.Add(scenePath);
                    SaveFavoriteScenes();
                }
                else
                {
                    Debug.LogWarning("Invalid scene asset selected.");
                }

                havePicked = false;
            }

            GUI.enabled = favScenes.Count > 0;

            var tex = EditorGUIUtility.IconContent("d_TreeEditor.Trash").image;
            if (GUILayout.Button(new GUIContent(null, tex, $"Delete all Favorite Scenes"), GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Warning", $"Are you sure you want to delete all Favorite Scenes ?", "Yes", "No"))
                {
                    favScenes.Clear();
                    favScenePaths.Clear();
                    SaveFavoriteScenes();
                }
            }

            GUI.enabled = true;

            GUILayout.EndHorizontal();
        }

        SaveFavoriteScenes();
        MyToolbarCustom.favScenes = favScenePaths;

        EditorGUILayout.Space();

        complexeHierarchy = GUILayout.Toggle(complexeHierarchy, "Toggle Complexe Hierarchy");
        HierarchyIconDisplay.SetComlplexeHierarchy(complexeHierarchy);

        reloadLastScene = GUILayout.Toggle(reloadLastScene, "Toggle Auto Reload Last Scene");
        EditorPrefs.SetBool("reloadLastScene", reloadLastScene);
    }

    void SaveSceneAsset()
    {
        if (favStartScene != null)
        {
            EditorPrefs.SetString("SavedScenePath", AssetDatabase.GetAssetPath(favStartScene));
        }
    }

    static void LoadSavedScene()
    {
        string scenePath = EditorPrefs.GetString("SavedScenePath", "");

        if (!string.IsNullOrEmpty(scenePath))
        {
            SceneAsset loadedSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            if (loadedSceneAsset != null)
            {
                favStartScene = loadedSceneAsset;
            }
            else
            {
                Debug.LogWarning("No saved scene asset found at path: " + scenePath);
            }
        }
        else
        {
            Debug.LogWarning("No saved scene asset path found in EditorPrefs.");
        }
    }

    void SaveFavoriteScenes()
    {
        for (int i = 0; i < favScenes.Count; i++)
        {
            if (favScenes[i] != null)
            {
                favScenePaths[i] = AssetDatabase.GetAssetPath(favScenes[i]);
            }
        }

        EditorPrefs.SetString("FavoriteScenes", string.Join(";", favScenePaths));
    }

    static void LoadFavoriteScenes()
    {
        favScenePaths = new List<string>(EditorPrefs.GetString("FavoriteScenes", "").Split(';'));

        favScenes.Clear();
        foreach (var path in favScenePaths)
        {
            if (!string.IsNullOrEmpty(path))
            {
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                if (sceneAsset != null)
                {
                    favScenes.Add(sceneAsset);
                }
            }
        }
    }

    void SwapScenes(int indexA, int indexB)
    {
        var tempScene = favScenes[indexA];
        favScenes[indexA] = favScenes[indexB];
        favScenes[indexB] = tempScene;

        var tempPath = favScenePaths[indexA];
        favScenePaths[indexA] = favScenePaths[indexB];
        favScenePaths[indexB] = tempPath;
    }
}
