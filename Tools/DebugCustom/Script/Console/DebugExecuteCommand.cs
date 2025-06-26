using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DebugExecuteCommand : MonoBehaviour
{
    private Dictionary<string, MethodInfo> commands = new Dictionary<string, MethodInfo>();
    private Dictionary<string, string[]> commandParameterNames = new Dictionary<string, string[]>();

    void Start()
    {
        RegisterCommands();
    }

    void RegisterCommands()
    {
        var methods = typeof(MonoBehaviour).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<ConsoleCommandAttribute>();
            if (attribute != null)
            {
                commands.Add(attribute.Command, method);

                // Retrieve parameter names from method
                var parameterInfos = method.GetParameters();
                var parameterNames = parameterInfos.Select(p => p.Name).ToArray();
                commandParameterNames.Add(attribute.Command, parameterNames);
            }
        }
    }

    public void ExecuteCommand(string input)
    {
        // Split the input into command and parameters
        string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            Debug.LogError("Invalid command format.");
            return;
        }

        string commandName = parts[0];
        string[] commandParameters = parts.Skip(1).ToArray();

        if (commands.TryGetValue(commandName, out var method))
        {
            var instance = FindObjectOfType<MonoBehaviour>(); // assuming commands are instance methods
            var parameterInfos = method.GetParameters();

            // Check if number of parameters matches
            if (parameterInfos.Length != commandParameters.Length)
            {
                Debug.LogError($"Incorrect number of parameters for command '{commandName}'. Expected {parameterInfos.Length}, got {commandParameters.Length}.");
                return;
            }

            // Convert parameters to correct types
            object[] parsedParameters = new object[commandParameters.Length];
            for (int i = 0; i < commandParameters.Length; i++)
            {
                Type paramType = parameterInfos[i].ParameterType;

                try
                {
                    parsedParameters[i] = Convert.ChangeType(commandParameters[i], paramType);
                }
                catch (FormatException)
                {
                    Debug.LogError($"Invalid format for parameter {i + 1} of command '{commandName}'. Expected {paramType.Name}.");
                    return;
                }
                catch (InvalidCastException)
                {
                    Debug.LogError($"Invalid type for parameter {i + 1} of command '{commandName}'. Expected {paramType.Name}.");
                    return;
                }
            }

            // Invoke the method with parsed parameters
            method.Invoke(instance, parsedParameters);
        }
        else
        {
            Debug.LogError($"Command not found: {commandName}");
        }
    }
}
