using UnityEngine;
using System;

namespace CustomSystem.SaverLoader 
{
    using PlayerPrefsExtra;
    using System.Collections.Generic;

    public static class SaverLoader
    {
        public static event Action OnLoad;
        public static event Action OnSave;

        public static void SaveAll() 
        {
            OnSave?.Invoke();
        }

        public static void LoadAll()
        {
            OnLoad?.Invoke();
        }

        public static void Delete()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void Save(object obj)
        {
            var type = obj.GetType();
            var fields = type.GetFields();

            foreach (var field in fields)
            {
                var saveAttributes = field.GetCustomAttributes(typeof(SaveAttribute), true);
                if (saveAttributes.Length > 0)
                {
                    var key = (saveAttributes[0] as SaveAttribute).key;
                    var fieldType = field.FieldType;

                    switch (Type.GetTypeCode(fieldType))
                    {
                        case TypeCode.Int32:
                            PlayerPrefsExtra.SetInt(key, (int)field.GetValue(obj));
                            break;
                        case TypeCode.Single:
                            PlayerPrefsExtra.SetFloat(key, (float)field.GetValue(obj));
                            break;
                        case TypeCode.String:
                            PlayerPrefsExtra.SetString(key, (string)field.GetValue(obj));
                            break;
                        case TypeCode.Boolean:
                            PlayerPrefsExtra.SetBool(key, (bool)field.GetValue(obj));
                            break;
                        default:
                            if (fieldType == typeof(Vector2))
                            {
                                PlayerPrefsExtra.SetVector2(key, (Vector2)field.GetValue(obj));
                            }
                            else if (fieldType == typeof(Vector3))
                            {
                                PlayerPrefsExtra.SetVector3(key, (Vector3)field.GetValue(obj));
                            }
                            else if (fieldType == typeof(Vector4))
                            {
                                PlayerPrefsExtra.SetVector4(key, (Vector4)field.GetValue(obj));
                            }
                            else if (fieldType == typeof(Color))
                            {
                                PlayerPrefsExtra.SetColor(key, (Color)field.GetValue(obj));
                            }
                            else if (fieldType == typeof(Quaternion))
                            {
                                PlayerPrefsExtra.SetQuaternion(key, (Quaternion)field.GetValue(obj));
                            }
                            else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                var listType = typeof(List<>).MakeGenericType(fieldType.GetGenericArguments()[0]);
                                var list = Convert.ChangeType(field.GetValue(obj), listType);
                                //PlayerPrefsExtra.SetList(key, (List<object>)list);
                            }
                            else
                            {
                                //PlayerPrefsExtra.SetObject(key, field.GetValue(obj));
                            }
                            break;
                    }

                    PlayerPrefs.Save();
                }
            }
        }

        public static void Load(object obj)
        {
            var type = obj.GetType();
            var fields = type.GetFields();

            foreach (var field in fields)
            {
                var saveAttributes = field.GetCustomAttributes(typeof(SaveAttribute), true);
                if (saveAttributes.Length > 0)
                {
                    var key = (saveAttributes[0] as SaveAttribute).key;
                    var fieldType = field.FieldType;

                    if (PlayerPrefs.HasKey(key))
                    {
                        switch (Type.GetTypeCode(fieldType))
                        {
                            case TypeCode.Int32:
                                field.SetValue(obj, PlayerPrefsExtra.GetInt(key));
                                break;
                            case TypeCode.Single:
                                field.SetValue(obj, PlayerPrefsExtra.GetFloat(key));
                                break;
                            case TypeCode.String:
                                field.SetValue(obj, PlayerPrefsExtra.GetString(key));
                                break;
                            case TypeCode.Boolean:
                                field.SetValue(obj, PlayerPrefsExtra.GetBool(key));
                                break;
                            default:
                                if (fieldType == typeof(Vector2))
                                {
                                    field.SetValue(obj, PlayerPrefsExtra.GetVector2(key));
                                }
                                else if (fieldType == typeof(Vector3))
                                {
                                    field.SetValue(obj, PlayerPrefsExtra.GetVector3(key));
                                }
                                else if (fieldType == typeof(Vector4))
                                {
                                    field.SetValue(obj, PlayerPrefsExtra.GetVector4(key));
                                }
                                else if (fieldType == typeof(Color))
                                {
                                    field.SetValue(obj, PlayerPrefsExtra.GetColor(key));
                                }
                                else if (fieldType == typeof(Quaternion))
                                {
                                    field.SetValue(obj, PlayerPrefsExtra.GetQuaternion(key));
                                }
                                else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
                                {
                                    var listType = typeof(List<>).MakeGenericType(fieldType.GetGenericArguments()[0]);
                                    var defaultValue = Activator.CreateInstance(listType);
                                    //field.SetValue(obj, PlayerPrefsExtra.GetList<object>(key, (List<object>)defaultValue));
                                }
                                else
                                {
                                    var defaultValue = Activator.CreateInstance(fieldType);
                                    //field.SetValue(obj, PlayerPrefsExtra.GetObject(key, defaultValue));
                                }
                                break;
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning($"Key not found in PlayerPrefs: {key}");
                    }
                }
            }
        }
    }
}
