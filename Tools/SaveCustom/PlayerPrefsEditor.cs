using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using CustomSystem.PlayerPrefsExtra;

public class PlayerPrefsEditor : EditorWindow
{
    private Vector2 scrollPosition;
    private List<string> keys;

    [MenuItem("Tools/PlayerPrefs Editor")]
    public static void ShowWindow()
    {
        GetWindow<PlayerPrefsEditor>("PlayerPrefs Editor");
    }

    private void OnEnable()
    {
        RefreshKeys();
    }

    private void RefreshKeys()
    {
        keys = GetAllKeys();
    }

    private void ResetAllToDefault()
    {
        foreach (string key in keys)
        {
            string type = GetKeyType(key);
            ResetValue(type, key);
        }
        RefreshKeys();
    }

    void ResetValue(string type, string key)
    {
        if (type == "Int")
        {
            PlayerPrefsExtra.SetInt(key, 0);
        }
        else if (type == "Float")
        {
            PlayerPrefsExtra.SetFloat(key, 0.0f);
        }
        else if (type == "String")
        {
            PlayerPrefsExtra.SetString(key, "");
        }
        else if (type == "Bool")
        {
            PlayerPrefsExtra.SetBool(key, false);
        }
        else if (type == "Vector2")
        {
            PlayerPrefsExtra.SetVector2(key, Vector2.zero);
        }
        else if (type == "Vector3")
        {
            PlayerPrefsExtra.SetVector3(key, Vector3.zero);
        }
        else if (type == "Vector4")
        {
            PlayerPrefsExtra.SetVector4(key, Vector4.zero);
        }
        else if (type == "Quaternion")
        {
            PlayerPrefsExtra.SetQuaternion(key, Quaternion.identity);
        }
        else if (type == "Color")
        {
            PlayerPrefsExtra.SetColor(key, default(Color));
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh", GUILayout.Width(150)))
        {
            RefreshKeys();
        }

        if (GUILayout.Button("Reset All to Default", GUILayout.Width(150)))
        {
            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to reset all PlayerPrefs values to their default?", "Yes", "No"))
            {
                ResetAllToDefault();
            }
        }

        if (GUILayout.Button("Delete All PlayerPrefs", GUILayout.Width(150)))
        {
            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete all PlayerPrefs?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                RefreshKeys();
            }
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

        GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(200));
        GUILayout.Label("Type", EditorStyles.boldLabel, GUILayout.Width(75));
        GUILayout.Label("Value", EditorStyles.boldLabel, GUILayout.Width(250));

        GUILayout.EndHorizontal();

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (string key in keys)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(key, GUILayout.Width(200));

            string type = GetKeyType(key);
            GUILayout.Label(type, GUILayout.Width(75));


            if (type == "Int")
            {
                int value = PlayerPrefsExtra.GetInt(key);
                value = EditorGUILayout.IntField(value, GUILayout.Width(250));
                PlayerPrefsExtra.SetInt(key, value);
            }
            else if (type == "Float")
            {
                float value = PlayerPrefsExtra.GetFloat(key);
                value = EditorGUILayout.FloatField(value, GUILayout.Width(250));
                PlayerPrefsExtra.SetFloat(key, value);
            }
            else if (type == "String")
            {
                string value = PlayerPrefsExtra.GetString(key);
                value = EditorGUILayout.TextField(value, GUILayout.Width(250));
                PlayerPrefsExtra.SetString(key, value);
            }
            else if (type == "Bool")
            {
                bool value = PlayerPrefsExtra.GetBool(key);
                value = EditorGUILayout.Toggle(value, GUILayout.Width(250));
                PlayerPrefsExtra.SetBool(key, value);
            }
            else if (type == "Vector2")
            {
                Vector2 value = PlayerPrefsExtra.GetVector2(key, Vector2.zero);
                value = EditorGUILayout.Vector2Field("", value, GUILayout.Width(250));
                PlayerPrefsExtra.SetVector2(key, value);
            }
            else if (type == "Vector3")
            {
                Vector3 value = PlayerPrefsExtra.GetVector3(key, Vector3.zero);
                value = EditorGUILayout.Vector3Field("", value, GUILayout.Width(250));
                PlayerPrefsExtra.SetVector3(key, value);
            }
            else if (type == "Vector4")
            {
                Vector4 value = PlayerPrefsExtra.GetVector4(key, Vector4.zero);
                value = EditorGUILayout.Vector4Field("", value, GUILayout.Width(250));
                PlayerPrefsExtra.SetVector4(key, value);
            }
            else if (type == "Quaternion")
            {
                Quaternion value = PlayerPrefsExtra.GetQuaternion(key, Quaternion.identity);
                value = Quaternion.Euler(EditorGUILayout.Vector3Field("", value.eulerAngles, GUILayout.Width(250)));
                PlayerPrefsExtra.SetQuaternion(key, value);
            }
            else if (type == "Color")
            {
                Color value = PlayerPrefsExtra.GetColor(key, Color.black);
                value = EditorGUILayout.ColorField(value, GUILayout.Width(250));
                PlayerPrefsExtra.SetColor(key, value);
            }


            var tex = EditorGUIUtility.IconContent("d_Refresh").image;
            if (GUILayout.Button(new GUIContent(null, tex, $"Reset PlayerPrefs to default"), GUILayout.Width(25)))
            {
                ResetValue(type, key);
            }

            tex = EditorGUIUtility.IconContent("d_TreeEditor.Trash").image;
            if (GUILayout.Button(new GUIContent(null, tex, $"Delete PlayerPrefs"), GUILayout.Width(25)))
            {
                if (EditorUtility.DisplayDialog("Warning", $"Are you sure you want to delete PlayerPrefs key '{key}'?", "Yes", "No"))
                {
                    PlayerPrefs.DeleteKey(key);
                    string keys = PlayerPrefs.GetString("PlayerPrefsKeys", "");
                    List<string> keyList = new List<string>(keys.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
                    if (keyList.Contains(key))
                    {
                        keyList.Remove(key);
                        PlayerPrefs.SetString("PlayerPrefsKeys", string.Join(";", keyList) + ";");
                    }
                    RefreshKeys();
                }
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }

    public List<string> GetAllKeys()
    {
        string keys = PlayerPrefs.GetString("PlayerPrefsKeys", "");
        return new List<string>(keys.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
    }

    public string GetKeyType(string key)
    {
        string data = PlayerPrefs.GetString(key);
        int index = data.IndexOf('|');
        if (index > -1)
        {
            return data.Substring(0, index);
        }
        return "";
    }
}
