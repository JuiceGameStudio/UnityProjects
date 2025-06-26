using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoollingSystem))]
public class PoolingSystemCustomEditor : Editor
{
    private PoollingSystem poollingSystem; // Référence à PoollingSystem
    public static bool doOnce = false;

    // Méthode appelée lors de l'activation de l'éditeur
    private void OnEnable()
    {
        poollingSystem = target as PoollingSystem; // Lien avec l'objet cible
    }

    // Méthode d'édition de l'inspecteur
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Mise à jour de la liste des ScriptablePoolling dans le PoollingSystem
        SerializedProperty scriptablePoollingList = serializedObject.FindProperty("ScriptObject");

        // Recherche dynamique des ScriptablePoolling disponibles dans le projet
        ScriptablePoolling[] allPoolling = PoollingSystemEditor.GetAllInstances<ScriptablePoolling>();

        // Ajout de nouveaux éléments à la liste
        for (int i = 0; i < allPoolling.Length; i++)
        {
            // Vérifie si l'élément existe déjà dans la liste
            bool exists = false;
            for (int j = 0; j < scriptablePoollingList.arraySize; j++)
            {
                if (scriptablePoollingList.GetArrayElementAtIndex(j).objectReferenceValue == allPoolling[i])
                {
                    exists = true;
                    break;
                }
            }

            // Si l'élément n'existe pas, on l'ajoute à la liste
            if (!exists)
            {
                scriptablePoollingList.arraySize++;
                scriptablePoollingList.GetArrayElementAtIndex(scriptablePoollingList.arraySize - 1).objectReferenceValue = allPoolling[i];
            }
        }

        if (doOnce)
        {
            doOnce = false;
            poollingSystem.CreateEatchPoolling();
        }

        // Applique les modifications au SerializedObject
        serializedObject.ApplyModifiedProperties();
    }
}
