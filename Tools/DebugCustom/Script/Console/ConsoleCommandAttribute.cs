using System;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
sealed class ConsoleCommandAttribute : Attribute
{
    public string Command { get; }
    public Type[] ParameterTypes { get; }
    public string[] ParameterNames { get; }

    public ConsoleCommandAttribute(string command)
    {
        Command = command;
    }
}
