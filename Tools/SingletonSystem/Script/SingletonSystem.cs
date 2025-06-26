using UnityEngine;

namespace CustomSystem.Singleton
{
    /// <summary>
    /// Custom system d'implementation de singleton
    /// </summary>
    /// <typeparam name="T">Le script que l'on souhaite avoir en singleton</typeparam>
    public abstract class SingletonSystem<T> : MonoBehaviour where T : SingletonSystem<T>
    {
        public static T Instance;
        
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
