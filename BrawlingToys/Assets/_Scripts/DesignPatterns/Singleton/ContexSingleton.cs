using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrawlingToys.DesignPatterns
{
    public abstract class ContextSingleton<T> : MonoBehaviour where T : Component
    {
        [Header("Singleton Settings")]

        [Tooltip("If its true, the singleton will be add to Dont Destroy On Load Scene in initialization")]
        [SerializeField] private bool _dontDestroyOnLoad = false; 
        
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this as T)
            {
                Destroy(gameObject);
                return;
            }

            if(_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
            
            Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this as T)
            {
                Instance = null; 
            }
        }
    }
}
