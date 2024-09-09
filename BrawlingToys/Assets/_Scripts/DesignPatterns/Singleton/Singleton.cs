using UnityEngine;

namespace BrawlingToys.DesignPatterns
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        [Header("Singleton Settings")]

        [Tooltip("If its true, the singleton will be add to Dont Destroy On Load Scene in initialization")]
        [SerializeField] private bool _dontDestroyOnLoad = true; 
        
        public static T instance { get; private set; }

        protected virtual void Awake()
        {
            int timeManagers = FindObjectsOfType<T>().Length;

            if (instance != null && instance != this as T
                && timeManagers > 1)
            {
                Destroy(gameObject);
                return;
            }

            if(_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(transform.root.gameObject);
                DontDestroyOnLoad(gameObject);
            }
            
            instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (instance == this as T)
            {
                instance = null; 
            }
        }
    }
}

