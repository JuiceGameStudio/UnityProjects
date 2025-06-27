using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class AnimatorParameters : MonoBehaviour
{
#if UNITY_EDITOR
    [HideInInspector] public WarningReason warningReason = WarningReason.NONE;
    string scriptName = "";
    Animator animator = null;
    AnimatorParameter[] parameters;

    public enum WarningReason
    {
        NOTGENERATED,
        DIFFERENT,
        NONE
    }

    public class AnimatorParameter
    {
        public string name;
        public AnimatorControllerParameterType type;
    }

    public string GetScriptName()
    {
        return $"{animator.runtimeAnimatorController.name}Param";
    }

    public void StoreParameters(string path)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError("No Animator component found on this GameObject.");
            return;
        }

        scriptName = GetScriptName();

        var animatorController = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        if (animatorController != null)
        {
            var animatorParameters = animatorController.parameters;
            parameters = new AnimatorParameter[animatorParameters.Length];
            for (int i = 0; i < animatorParameters.Length; i++)
            {
                parameters[i] = new AnimatorParameter
                {
                    name = animatorParameters[i].name,
                    type = animatorParameters[i].type
                };
            }

            if (DoesScriptExist(scriptName, out string existingScriptPath))
            {
                if (CompareWithExistingScript(existingScriptPath))
                {
                    GenerateStaticScript(existingScriptPath, scriptName);
                }
            }
            else
            {
                GenerateStaticScript(path, scriptName);
            }
        }
        else
        {
            Debug.LogError("AnimatorController is null or invalid.", gameObject);
        }
    }

    public void CheckFiles()
    {
        if (TryGetComponent<Animator>(out Animator animator))
        {
            this.animator = animator;
        }

        if (this.animator == null)
        {
            Debug.LogError("No Animator component found on this GameObject.", gameObject);
            return;
        }

        if (this.animator.runtimeAnimatorController == null)
        {
            Debug.LogError("No controller component found on this GameObject.", gameObject);
            return;
        }

        if (scriptName == "")
        {
            scriptName = GetScriptName();
        }

        var animatorController = this.animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        if (animatorController != null)
        {
            var animatorParameters = animatorController.parameters;
            parameters = new AnimatorParameter[animatorParameters.Length];
            for (int i = 0; i < animatorParameters.Length; i++)
            {
                parameters[i] = new AnimatorParameter
                {
                    name = animatorParameters[i].name,
                    type = animatorParameters[i].type
                };
            }

            if (DoesScriptExist(scriptName, out string existingScriptPath))
            {
                if (CompareWithExistingScript(existingScriptPath))
                {
                    warningReason = WarningReason.DIFFERENT;
                }
                else
                {
                    warningReason = WarningReason.NONE;
                }
            }
            else
            {
                warningReason = WarningReason.NOTGENERATED;
            }
        }
        else
        {
            Debug.LogError("AnimatorController is null or invalid.", gameObject);
        }
    }

    void GenerateStaticScript(string path, string scriptName)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Invalid path provided for generating script.");
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"public static class {scriptName}");
        sb.AppendLine("{");

        AppendParametersByType(sb, "Bool", AnimatorControllerParameterType.Bool);
        AppendParametersByType(sb, "Float", AnimatorControllerParameterType.Float);
        AppendParametersByType(sb, "Int", AnimatorControllerParameterType.Int);
        AppendParametersByType(sb, "Trigger", AnimatorControllerParameterType.Trigger);

        sb.AppendLine("}");

        File.WriteAllText(path, sb.ToString());

        warningReason = WarningReason.NONE;
        AssetDatabase.Refresh();
        Debug.Log($"Script generated at: {path}");
    }

    void AppendParametersByType(StringBuilder sb, string typeName, AnimatorControllerParameterType parameterType)
    {
        sb.AppendLine($"    public class {typeName}s");
        sb.AppendLine("    {");

        foreach (var param in parameters)
        {
            if (param.type == parameterType)
            {
                sb.AppendLine($"        public const string {param.name} = \"{param.name}\";");
            }
        }

        sb.AppendLine("    }");
    }

    bool DoesScriptExist(string scriptName, out string existingScriptPath)
    {
        string assetsPath = Application.dataPath;
        string[] files = Directory.GetFiles(assetsPath, scriptName + ".cs", SearchOption.AllDirectories);

        if (files.Length > 0)
        {
            existingScriptPath = files[0];
            return true;
        }
        else
        {
            existingScriptPath = null;
            return false;
        }
    }

    bool CompareWithExistingScript(string existingScriptPath)
    {
        string[] scriptLines = File.ReadAllLines(existingScriptPath);
        HashSet<string> scriptParams = new HashSet<string>();

        foreach (string line in scriptLines)
        {
            string trimmedLine = line.Trim();
            if (trimmedLine.StartsWith("public const string"))
            {
                int startIndex = trimmedLine.IndexOf("string") + "string".Length;
                int endIndex = trimmedLine.IndexOf("=", startIndex);
                if (startIndex >= 0 && endIndex >= 0)
                {
                    string paramName = trimmedLine.Substring(startIndex, endIndex - startIndex).Trim();
                    scriptParams.Add(paramName);
                }
            }
        }

        HashSet<string> animatorParams = new HashSet<string>();
        foreach (var param in parameters)
        {
            animatorParams.Add(param.name);
        }

        bool missingInScript = !animatorParams.IsSubsetOf(scriptParams);
        bool extraInScript = !scriptParams.IsSubsetOf(animatorParams);

        return missingInScript || extraInScript;
    }
}

[CustomEditor(typeof(AnimatorParameters))]
public class AnimatorParametersEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AnimatorParameters myScript = (AnimatorParameters)target;

        DrawDefaultInspector();

        myScript.CheckFiles();

        if (myScript.warningReason != AnimatorParameters.WarningReason.NONE)
        {
            string warningHelpBox = "";
            string warningLog = "";
            switch (myScript.warningReason)
            {
                case AnimatorParameters.WarningReason.NOTGENERATED:
                    warningHelpBox = "Warning: Animator Parameters are not generated for this script";
                    warningLog = $"Warning : {myScript.gameObject.name} // Animator Parameters are not generated for this script";
                    break;

                case AnimatorParameters.WarningReason.DIFFERENT:
                    warningHelpBox = "Warning: Animator Parameters are not the same as the existing script";
                    warningLog = $"Warning : {myScript.gameObject.name} // Animator Parameters are not the same as the existing script";
                    break;
            }
            EditorGUILayout.HelpBox(warningHelpBox, MessageType.Warning);
            Debug.LogWarning(warningLog, myScript.gameObject);
        }

        if (GUILayout.Button("Generate static values"))
        {
            if (myScript.warningReason == AnimatorParameters.WarningReason.NOTGENERATED)
            {
                string path = EditorUtility.SaveFilePanel(
                    "Save Script As",
                    "Assets",
                    myScript.GetScriptName(),
                    "cs"
                );

                if (!string.IsNullOrEmpty(path))
                {
                    myScript.StoreParameters(path);
                }
                else
                {
                    Debug.LogError("Invalid path or script name.", myScript.gameObject);
                }
            }
            else
            {
                myScript.StoreParameters("");
            }
        }
    }

}

[InitializeOnLoad]
public static class AnimatorMonitor
{
    static AnimatorMonitor()
    {
        ObjectFactory.componentWasAdded += OnComponentAdded;
    }

    private static void OnComponentAdded(Component component)
    {
        if (component is Animator animator)
        {
            GameObject gameObject = animator.gameObject;

            if (gameObject.GetComponent<AnimatorParameters>() == null)
            {
                gameObject.AddComponent<AnimatorParameters>();
            }
        }
    }
#endif
}
