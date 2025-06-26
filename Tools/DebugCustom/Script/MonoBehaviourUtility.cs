using UnityEngine;

public static class MonoBehaviourUtility
{
    /// <summary>
    /// Obtient automatiquement l'instance de MonoBehaviour à partir de l'appelant
    /// </summary>
    public static MonoBehaviour GetCallingMonoBehaviour()
    {
        var stackTrace = new System.Diagnostics.StackTrace();
        for (int i = 2; i < stackTrace.FrameCount; i++)
        {
            var frame = stackTrace.GetFrame(i);
            var method = frame.GetMethod();
            var declaringType = method.DeclaringType;

            if (declaringType != null && typeof(MonoBehaviour).IsAssignableFrom(declaringType))
            {
                return Object.FindObjectOfType(declaringType) as MonoBehaviour;
            }
        }

        return null;
    }
}
