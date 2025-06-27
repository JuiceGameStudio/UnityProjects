using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityToolbarExtender;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

static class ToolbarStyles
{
#if UNITY_EDITOR
    public static readonly GUIStyle commandButtonStyle;
    public static readonly GUIStyle textLabelStyle;
    public static readonly GUIStyle flexTextLabelStyle;

    static ToolbarStyles()
    {
        commandButtonStyle = new GUIStyle("Command")
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Normal,
            fixedWidth = 200,
        };

        textLabelStyle = new GUIStyle("Command")
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Normal,
            fixedWidth = 80,
        };

        flexTextLabelStyle = new GUIStyle("Command")
        {
            fontSize = 12,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Normal,
            padding = new RectOffset(2, 0, 0, 0),
        };
    }
}

/// <summary>
/// Script gérant différentes fonction pour aider dans le développement
/// </summary>
[InitializeOnLoad]
public class MyToolbarCustom
{
    static bool enabledDomaineReload = true;

    static List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>();
    public static List<string> favScenes = new List<string>();

    static Process process;
    static string lastScene = "null";

    static string LastScene
    {
        get { return lastScene; }
        set
        {
            lastScene = value;
            EditorPrefs.SetString("lastScene", value);
        }
    }

    static bool EnabledDomaineReload
    {
        get { return enabledDomaineReload; }
        set
        {
            EditorSettings.enterPlayModeOptionsEnabled = !value;
            enabledDomaineReload = value;
            EditorPrefs.SetBool("enabledDomaineReload", value);
        }
    }

    static MyToolbarCustom()
    {
        CustomToolbarPref.Load();
        GetDefaultState();
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUILeft);
        ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUIRight);
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    static void GetDefaultState()
    {
        EditorSettings.enterPlayModeOptions |= EnterPlayModeOptions.DisableDomainReload;
        EditorSettings.enterPlayModeOptions |= EnterPlayModeOptions.DisableSceneReload;
        enabledDomaineReload = EditorPrefs.GetBool("enabledDomaineReload", false);
        lastScene = EditorPrefs.GetString("lastScene", default);

        GetAllActiveScenes();
    }

    private static void GetAllActiveScenes()
    {
        buildScenes.Clear();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            buildScenes.Add(scene);
        }
        favScenes.Clear();
        favScenes = CustomToolbarPref.favScenes.Select(scene => AssetDatabase.GetAssetPath(scene)).ToList();

    }

    static void OpenScene(string scenePath)
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(scenePath);
    }

    static void OnToolbarGUILeft()
    {
        var tex = EditorGUIUtility.IconContent("Preset Icon").image;
        if (GUILayout.Button(new GUIContent(null, tex, "Toolbar preferences"), "Command"))
        {
            CustomToolbarPref.ShowWindow();
        }

        string activeSceneName = EditorSceneManager.GetActiveScene().name;

        if (GUILayout.Button(new GUIContent(activeSceneName, null, "Select a scene to load"), ToolbarStyles.commandButtonStyle))
        {
            GenericMenu menu = new GenericMenu();

            foreach (string scene in favScenes)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene);
                menu.AddItem(new GUIContent("Favorite Scenes/" + sceneName), false, () => OpenScene(scene));
            }

            foreach (var scene in buildScenes)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path) + (scene.enabled ? "" : " (Disabled)");
                menu.AddItem(new GUIContent("Build Scenes/" + sceneName), false, () => OpenScene(scene.path));
            }

            menu.ShowAsContext();
        }

        GUI.changed = false;

        tex = EditorGUIUtility.IconContent("P4_Updating").image;
        GUILayout.Toggle(enabledDomaineReload, new GUIContent(null, tex, "Enabled/Disabled Domaine reload"), "Command");

        if (GUI.changed)
        {
            EnabledDomaineReload = !EnabledDomaineReload;
        }

        GUI.changed = false;

        if (Application.isPlaying)
        {
            GUI.enabled = false;
        }

        if (Application.isPlaying)
        {
            GUI.enabled = true;
        }

        GUILayout.FlexibleSpace();
    }

    static void OnToolbarGUIRight()
    {
        var tex = EditorGUIUtility.IconContent("SceneAsset On Icon").image;
        string text = CustomToolbarPref.favStartScene != null ? CustomToolbarPref.favStartScene.name : "default";
        if (GUILayout.Button(new GUIContent(null, tex, $"Go and Start from {text}"), "Command"))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                LastScene = EditorSceneManager.GetActiveScene().path;
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(CustomToolbarPref.favStartScene));
                EditorApplication.isPlaying = true;
            }
        }

        GUI.changed = false;
    }

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            if (LastScene != "null" && CustomToolbarPref.reloadLastScene)
            {
                EditorSceneManager.OpenScene(LastScene);
                LastScene = "null";
            }
        }
    }
#endif
}