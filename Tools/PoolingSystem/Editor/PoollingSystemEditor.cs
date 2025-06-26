using CustomSystem.GenerateEnum;
using CustomSystem.Pooling;
using System;
using UnityEditor;
using UnityEngine;

public class PoollingSystemEditor : EditorWindow
{
    SerializedObject serializedObject;
    SerializedProperty serializedProperty;

    ScriptablePoolling[] ScriptableObject;
    string selectedProperty;
    string repertory = "Assets/PoollingAssets/";
    string defaultName = "NewPool";

    bool haveChange = false;
    bool showParam;

    [MenuItem("Tools/Poolling System Creator")]
    static void ShowWindow()
    {
        GetWindow<PoollingSystemEditor>("Poolling System Creator");
    }

    private void OnGUI()
    {
        ScriptableObject = GetAllInstances<ScriptablePoolling>();

        GUI.enabled = haveChange;
        if (GUILayout.Button("Generated"))
        {
            Generated();
            haveChange = false;
        }
        GUI.enabled = true;

        showParam = EditorGUILayout.Foldout(showParam, "Parameters");

        if (showParam)
        {
            repertory = EditorGUILayout.TextField("Execution Path:", repertory);
            defaultName = EditorGUILayout.TextField("Default Generated Name:", defaultName);
        }

        EditorGUILayout.BeginHorizontal();


        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        DrawSliderBar(ScriptableObject);

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        EditorGUI.BeginChangeCheck();

        if (selectedProperty != null)
        {
            // Afficher les propriétés de l'objet sélectionné
            for (int i = 0; i < ScriptableObject.Length; i++)
            {
                if (ScriptableObject[i] != null && ScriptableObject[i].scriptableName == selectedProperty)
                {
                    serializedObject = new SerializedObject(ScriptableObject[i]);
                    serializedProperty = serializedObject.GetIterator();
                    serializedProperty.NextVisible(true);
                    DrawProperties(serializedProperty);
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("Select an item from the list or create a new one");
        }

        if (EditorGUI.EndChangeCheck())
        {
            haveChange = true;
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        Apply();
    }

    /// <summary>
    /// Cherche le type d'entit� (Enum) correspondant au string
    /// </summary>
    /// <param name="enumName">Le type d'entit� voulue</param>
    /// <returns>Le type  d'entit� (Enum) correspondant au string (retourne DEFAULT si non trouv�)</returns>
    PoollingEntityType GetEnumType(string enumName)
    {
        for (int i = 0; i < Enum.GetValues(typeof(PoollingEntityType)).Length; i++)
        {
            if (((PoollingEntityType)i).ToString() == enumName)
            {
                return (PoollingEntityType)i;
            }
        }
        Debug.LogWarning($"{typeof(PoollingSystem).Name} : The type {enumName} not exist, the type DEFAULT is attribuate");
        return PoollingEntityType.DEFAULT;
    }

    public static T[] GetAllInstances<T>() where T : ScriptablePoolling
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;
    }

    protected void DrawProperties(SerializedProperty p)
    {
        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);
        }
    }

    protected void DrawSliderBar(ScriptablePoolling[] prop)
    {
        for (int i = 0; i < prop.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(prop[i].scriptableName, GUILayout.ExpandWidth(true)))
            {
                selectedProperty = prop[i].scriptableName;
            }

            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                DeleteScriptableObject(prop[i]);
                serializedObject = null;
            }

            EditorGUILayout.EndHorizontal();
        }

        GUIStyle boldButtonStyle = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold };

        if (GUILayout.Button("Create new one", boldButtonStyle))
        {
            CreateInstance();
        }
    }

    void DeleteScriptableObject(ScriptablePoolling obj)
    {
        string assetPath = AssetDatabase.GetAssetPath(obj);

        if (!string.IsNullOrEmpty(assetPath))
        {
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            ScriptableObject = GetAllInstances<ScriptablePoolling>();
        }
        else
        {
            Debug.LogError("Impossible de supprimer l'asset, chemin introuvable.");
        }
    }

    void Generated()
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(ScriptablePoolling).Name);
        ScriptablePoolling[] a = new ScriptablePoolling[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<ScriptablePoolling>(path);
            AssetDatabase.RenameAsset(path, a[i].scriptableName);
            a[i].entityType = GetEnumType(a[i].customEntityType);
        }
        CreateCustomEnum.GenerateEnum(ScriptableObject);
#if UNITY_EDITOR
        PoolingSystemCustomEditor.doOnce = true;
#endif
    }

    void CreateInstance()
    {
        ScriptablePoolling newPool = CreateInstance<ScriptablePoolling>();

        ScriptableObject = GetAllInstances<ScriptablePoolling>();

        if (string.IsNullOrEmpty(newPool.scriptableName))
        {
            newPool.scriptableName = defaultName;
        }

        string assetPath = $"{repertory}{newPool.scriptableName}.asset";
        int counter = 1;

        // Vérifier l'existence du nom, ajouter un suffixe si nécessaire
        while (AssetDatabase.LoadAssetAtPath<ScriptablePoolling>(assetPath) != null)
        {
            newPool.scriptableName = $"{defaultName}{counter}";
            assetPath = $"{repertory}{newPool.scriptableName}.asset";
            counter++;
        }

        AssetDatabase.CreateAsset(newPool, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void Apply()
    {
        if (serializedObject != null)
        {
            serializedObject.ApplyModifiedProperties();
            selectedProperty = serializedObject.FindProperty("scriptableName").stringValue;
        }
    }
}
