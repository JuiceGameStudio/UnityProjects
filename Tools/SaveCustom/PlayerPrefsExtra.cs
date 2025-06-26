using UnityEngine;
using System.Collections.Generic;

namespace CustomSystem.PlayerPrefsExtra
{
    public static class PlayerPrefsExtra
    {
        private static void AddKey(string key)
        {
            string keys = PlayerPrefs.GetString("PlayerPrefsKeys", "");
            if (!keys.Contains(key))
            {
                keys += key + ";";
                PlayerPrefs.SetString("PlayerPrefsKeys", keys);
            }
        }

        #region int 

        public static void SetInt(string key, int value)
        {
            string data = $"Int|{value}";
            PlayerPrefs.SetString(key, data);
            AddKey(key);
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            string data = PlayerPrefs.GetString(key, null);
            if (data != null && data.StartsWith("Int|"))
            {
                return int.Parse(data.Substring(4));
            }
            return defaultValue;
        }

        #endregion

        #region float 

        public static void SetFloat(string key, float value)
        {
            string data = $"Float|{value}";
            PlayerPrefs.SetString(key, data);
            AddKey(key);
        }

        public static float GetFloat(string key, float defaultValue = 0f)
        {
            string data = PlayerPrefs.GetString(key, null);
            if (data != null && data.StartsWith("Float|"))
            {
                return float.Parse(data.Substring(6));
            }
            return defaultValue;
        }

        #endregion

        #region string 

        public static void SetString(string key, string value)
        {
            string data = $"String|{value}";
            PlayerPrefs.SetString(key, data);
            AddKey(key);
        }

        public static string GetString(string key, string defaultValue = "")
        {
            string data = PlayerPrefs.GetString(key, null);
            if (data != null && data.StartsWith("String|"))
            {
                return data.Substring(7);
            }
            return defaultValue;
        }

        #endregion

        #region Bool 

        public static void SetBool(string key, bool value)
        {
            string data = $"Bool|{(value ? "true" : "false")}";
            PlayerPrefs.SetString(key, data);
            AddKey(key);
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            string data = PlayerPrefs.GetString(key, null);
            if (data != null && data.StartsWith("Bool|"))
            {
                return data.Substring(5) == "true";
            }
            return defaultValue;
        }

        #endregion

        #region Vector 2 

        public static void SetVector2(string key, Vector2 value)
        {
            string data = $"Vector2|{value.x};{value.y}";
            PlayerPrefs.SetString(key, data);
            AddKey(key);
        }

        public static Vector2 GetVector2(string key, Vector2 defaultValue = default(Vector2))
        {
            string data = PlayerPrefs.GetString(key, null);
            if (data != null && data.StartsWith("Vector2|"))
            {
                string[] parts = data.Substring(8).Split(';');
                if (parts.Length == 2 &&
                    float.TryParse(parts[0], out float x) &&
                    float.TryParse(parts[1], out float y))
                {
                    return new Vector2(x, y);
                }
            }

            return defaultValue;
        }

        #endregion

        #region Vector 3 

        public static void SetVector3(string key, Vector3 value)
        {
            string data = $"Vector3|{value.x};{value.y};{value.z}";
            PlayerPrefs.SetString(key, data);
            AddKey(key);
        }

        public static Vector3 GetVector3(string key, Vector3 defaultValue = default(Vector3))
        {
            string data = PlayerPrefs.GetString(key, null);
            if (data != null && data.StartsWith("Vector3|"))
            {
                string[] parts = data.Substring(8).Split(';');
                if (parts.Length == 3 &&
                    float.TryParse(parts[0], out float x) &&
                    float.TryParse(parts[1], out float y) &&
                    float.TryParse(parts[2], out float z))
                {
                    return new Vector3(x, y, z);
                }
            }
            return defaultValue;
        }

        #endregion

        #region Vector 4 

        public static void SetVector4(string key, Vector4 value)
        {
            string data = $"Vector4|{value.x};{value.y};{value.z};{value.w}";
            PlayerPrefs.SetString(key, data);
            AddKey(key);
        }

        public static Vector4 GetVector4(string key, Vector4 defaultValue = default(Vector4))
        {
            string data = PlayerPrefs.GetString(key, null);
            if (data != null && data.StartsWith("Vector4|"))
            {
                string[] parts = data.Substring(8).Split(';');
                if (parts.Length == 4 &&
                    float.TryParse(parts[0], out float x) &&
                    float.TryParse(parts[1], out float y) &&
                    float.TryParse(parts[2], out float z) &&
                    float.TryParse(parts[3], out float w))
                {
                    return new Vector4(x, y, z, w);
                }
            }
            return defaultValue;
        }

        #endregion

        #region Color 

        public static void SetColor(string key, Color value)
        {
            string data = $"Color|{value.r};{value.g};{value.b};{value.a}";
            PlayerPrefs.SetString(key, data);
            AddKey(key);
        }

        public static Color GetColor(string key, Color defaultValue = default(Color))
        {
            string data = PlayerPrefs.GetString(key, null);
            if (data != null && data.StartsWith("Color|"))
            {
                string[] parts = data.Substring(6).Split(';');
                if (parts.Length == 4 &&
                    float.TryParse(parts[0], out float r) &&
                    float.TryParse(parts[1], out float g) &&
                    float.TryParse(parts[2], out float b) &&
                    float.TryParse(parts[3], out float a))
                {
                    return new Color(r, g, b, a);
                }
            }
            return defaultValue;
        }

        #endregion

        #region Quaternion 

        public static void SetQuaternion(string key, Quaternion value)
        {
            string data = $"Quaternion|{value.x};{value.y};{value.z};{value.w}";
            PlayerPrefs.SetString(key, data);
            AddKey(key);
        }

        public static Quaternion GetQuaternion(string key, Quaternion defaultValue = default(Quaternion))
        {
            string data = PlayerPrefs.GetString(key, null);
            if (data != null && data.StartsWith("Quaternion|"))
            {
                string[] parts = data.Substring(11).Split(';');
                if (parts.Length == 4 &&
                    float.TryParse(parts[0], out float x) &&
                    float.TryParse(parts[1], out float y) &&
                    float.TryParse(parts[2], out float z) &&
                    float.TryParse(parts[3], out float w))
                {
                    return new Quaternion(x, y, z, w);
                }
            }
            return defaultValue;
        }

        #endregion

        /* Work in Progress ------------------------------------------------------------------------------------
        #region List <T>

        public class ListWrapper<T>
        {
            public List<T> list = new List<T>();
        }

        public static List<T> GetList<T>(string key)
        {
            return Get<ListWrapper<T>>(key, new ListWrapper<T>()).list;
        }

        public static List<T> GetList<T>(string key, List<T> defaultValue)
        {
            return Get<ListWrapper<T>>(key, new ListWrapper<T> { list = defaultValue }).list;
        }

        public static void SetList<T>(string key, List<T> value)
        {
            Set(key, new ListWrapper<T> { list = value });
        }

        #endregion

        #region Object 

        public static T GetObject<T>(string key)
        {
            return Get<T>(key, default(T));
        }

        public static T GetObject<T>(string key, T defaultValue)
        {
            return Get<T>(key, defaultValue);
        }

        public static void SetObject<T>(string key, T value)
        {
            Set(key, value);
        }

        #endregion

        //Generic template ---------------------------------------------------------------------------------------

        static T Get<T>(string key, T defaultValue)
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key, JsonUtility.ToJson(defaultValue)));
        }

        static void Set<T>(string key, T value)
        {
            PlayerPrefs.SetString(key, JsonUtility.ToJson(value));
        }
        */
    }
}