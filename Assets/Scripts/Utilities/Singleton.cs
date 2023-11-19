using UnityEngine;

namespace Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>  
    {
        public static T Instance;

        public bool isPersistant = true;

        protected virtual void Awake()
        {
            if (isPersistant)
            {
                if (Instance)
                {
                    Destroy(gameObject); 
                }
                else
                {
                    Instance = this as T;
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Instance = this as T;
            }
        }
    }
}
