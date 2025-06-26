#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using CustomSystem.Pooling;

namespace CustomSystem.GenerateEnum
{
    /// <summary>
    /// Script pour générer un enum en fonction des noms renseigner dans la liste de poolings
    /// </summary>
    public class CreateCustomEnum
    {
        static string defaultFilePath = "Assets/Plugins/PoolingSystem/Script/PoollingEntityType.cs";
        static string scriptEnumName = typeof(PoollingEntityType).Name;

        /// <summary>
        /// Génère un enum en fonction des différants noms renseigner dans la listes de pooling
        /// </summary>
        /// <param name="poolling"></param>
        public static void GenerateEnum(ScriptablePoolling[] poolling)
        {
            string filePathAndName = $"{FindScriptLocalization()}";
            List<string> enumEntries = new List<string>();

            for (int i = 0; i < poolling.Length; i++)
            {
                enumEntries.Add(poolling[i].customEntityType);
            }

            enumEntries = NewNameAfterRestriction(enumEntries);

            for (int i = 0; i < enumEntries.Count; i++)
            {
                poolling[i].customEntityType = enumEntries[i];
            }

            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                streamWriter.WriteLine("namespace CustomSystem.Pooling");
                streamWriter.WriteLine("{");
                streamWriter.WriteLine("    /// <summary>");
                streamWriter.WriteLine("    /// Enum des différents entité pouvant être pool");
                streamWriter.WriteLine("    /// </summary>");
                streamWriter.WriteLine($"    public enum {scriptEnumName}");
                streamWriter.WriteLine("    {");
                streamWriter.WriteLine($"        DEFAULT,");
                for (int i = 0; i < enumEntries.Count; i++)
                {
                    if (i < enumEntries.Count - 1)
                    {
                        streamWriter.WriteLine($"        {enumEntries[i]},");
                    }
                    else
                    {
                        streamWriter.WriteLine($"        {enumEntries[i]}");
                    }
                }
                streamWriter.WriteLine("    }");
                streamWriter.WriteLine("}");
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Recherche l'emplacement de l'enum custom
        /// </summary>
        /// <returns>L'emplacement de l'enum custom (si non trouvé retourne le chemin par défaut)</returns>
        static string FindScriptLocalization()
        {
            string[] scriptPaths = AssetDatabase.FindAssets($"t:script {scriptEnumName}");

            if (scriptPaths.Length > 0)
            {
                return AssetDatabase.GUIDToAssetPath(scriptPaths[0]);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"{typeof(CreateCustomEnum).Name} : The script {typeof(PoollingEntityType).Name} not exist, create a new one at the default path");
                return defaultFilePath;
            }
        }

        /// <summary>
        /// Change les noms des différents enum générer pour correspondre aux normes (sans espace, majuscule, différents les uns des autres)
        /// </summary>
        /// <param name="enumNames">La liste des enums à appliquer des restrictions</param>
        /// <returns>La nouvelle liste de noms d'enum</returns>
        static List<string> NewNameAfterRestriction(List<string> enumNames)
        {
            List<string> newNames = new List<string>();
            HashSet<string> uniqueNames = new HashSet<string>(); // Utilisation d'un HashSet pour vérifier les doublons rapidement

            for (int i = 0; i < enumNames.Count; i++)
            {
                if (!string.IsNullOrEmpty(enumNames[i]))
                {
                    // Retrait des espaces et conversion en majuscules
                    enumNames[i] = enumNames[i].Replace(" ", "");
                    enumNames[i] = enumNames[i].ToUpper();
                }
                else
                {
                    // Si le nom est vide ou nul, on le remplace par un nom par défaut
                    enumNames[i] = $"{i}DEFAULT";
                    UnityEngine.Debug.LogWarning($"{typeof(CreateCustomEnum).Name} : Un CustomEntityType est vide ou nul, il a été remplacé par {enumNames[i]}");
                }

                string baseName = enumNames[i]; // On garde le nom de base pour les doublons
                int counter = 1;

                // Tant que le nom est déjà pris, on ajoute un suffixe unique
                while (uniqueNames.Contains(enumNames[i]))
                {
                    enumNames[i] = $"{baseName}_{counter}"; // Ajoute un suffixe unique
                    counter++; // Incrémente le suffixe
                }

                // Ajouter le nom modifié (unique) au dictionnaire
                uniqueNames.Add(enumNames[i]);

                // Ajouter à la liste finale
                newNames.Add(enumNames[i]);
            }

            return newNames;
        }

    }
}
#endif