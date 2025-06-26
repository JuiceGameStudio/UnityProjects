using System;
using UnityEngine;

namespace CustomAttributes
{
    /// <summary>
    /// Custom attribute class pour rendre accessible une variable en lecture seulement
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : PropertyAttribute { }
}
