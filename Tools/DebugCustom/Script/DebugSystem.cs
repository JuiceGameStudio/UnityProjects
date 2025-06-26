using UnityEngine;
using static MonoBehaviourUtility;

namespace CustomSystem.Debug
{
    /// <summary>
    /// Interface pour implémenter l'activation du debug local
    /// </summary>
    public interface IDebugSystem
    {
        bool localDebugEnabled { get; set; }
    }

    /// <summary>
    /// Script gérant les fonctions de debugs custom
    /// </summary>
    public static class DebugSystem
    {
        // Variable global qui déinis si l'on active tout les debugs logs ou non
        static bool globalDebugEnabled = false;

        /// <summary>
        /// Change l'état du debug local
        /// </summary>
        /// <param name="state">Le nouvelle état</param>
        public static void EnableLocalDebug(bool state, MonoBehaviour mb = null)
        {
            var script = mb == null ? GetCallingMonoBehaviour() : mb;
            if (script is IDebugSystem)
            {
                ((IDebugSystem)script).localDebugEnabled = state;
            }
        }

        /// <summary>
        /// Change l'état du debug global
        /// </summary>
        /// <param name="state">Le nouvelle état</param>
        public static void EnableGlobalDebug(bool state)
        {
            globalDebugEnabled = state;
        }

        /// <summary>
        /// Definit si le debug glabal est activé ou non
        /// </summary>
        /// <returns>L'état du debug global</returns>
        public static bool IsGlobalDebugEnabled()
        {
            return globalDebugEnabled;
        }

        /// <summary>
        /// Definit si le debug local est activé ou non
        /// </summary>
        /// <returns>L'état du debug local (vrai par défaut si le script n'implémente pas IDebugSystem)</returns>
        public static bool IsLocalDebugEnabled()
        {
            var script = GetCallingMonoBehaviour();
            if (script is IDebugSystem)
            {
                return ((IDebugSystem)script).localDebugEnabled;
            }
            else
            {
                return true; // Par défaut, si le script n'implémente pas IDebugSystem
            }
        }

        /// <summary>
        /// Affiche un log custom de type basic
        /// </summary>
        /// <param name="message">Le message a afficher dans le log</param>
        /// <param name="textColor">La couleur général du message</param>
        /// <param name="GO">La référence du gameobject sur lequel le debug est appelé</param>
        public static void Log(string message, Color? textColor = null, GameObject GO = null)
        {
            if (IsGlobalDebugEnabled() && IsLocalDebugEnabled())
            {
                string htmlColor = ColorUtility.ToHtmlStringRGB(textColor ?? Color.white);
                UnityEngine.Debug.Log($"[{GetCallingMonoBehaviour().GetType().Name}] <color=#{htmlColor}>{message}</color>", GO);
            }
        }

        /// <summary>
        /// Affiche un log custom de type warning
        /// </summary>
        /// <param name="message">Le message a afficher dans le log</param>
        /// <param name="textColor">La couleur général du message</param>
        /// <param name="GO">La référence du gameobject sur lequel le debug est appelé</param>
        public static void LogWarning(string message, Color? textColor = null, GameObject GO = null)
        {
            if (IsGlobalDebugEnabled() && IsLocalDebugEnabled())
            {
                string htmlColor = ColorUtility.ToHtmlStringRGB(textColor ?? Color.white);
                UnityEngine.Debug.LogWarning($"[{GetCallingMonoBehaviour().GetType().Name}] <color=#{htmlColor}>{message}</color>", GO);
            }
        }

        /// <summary>
        /// Affiche un log custom de type error
        /// </summary>
        /// <param name="message">Le message a afficher dans le log</param>
        /// <param name="textColor">La couleur général du message</param>
        /// <param name="GO">La référence du gameobject sur lequel le debug est appelé</param>
        public static void LogError(this MonoBehaviour script, string message, Color? textColor = null, GameObject GO = null)
        {
            if (IsGlobalDebugEnabled() && IsLocalDebugEnabled())
            {
                string htmlColor = ColorUtility.ToHtmlStringRGB(textColor ?? Color.white);
                UnityEngine.Debug.LogError($"[{GetCallingMonoBehaviour().GetType().Name}] <color=#{htmlColor}>{message}</color>", GO);
            }
        }
    }
}