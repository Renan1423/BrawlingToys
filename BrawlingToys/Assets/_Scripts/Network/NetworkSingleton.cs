using Unity.Netcode;
using UnityEngine;

namespace BrawlingToys.Network
{
    public class NetworkSingleton<T> : NetworkBehaviour where T : MonoBehaviour
    {
        [Header("Singleton Settings")]

        [Tooltip("If its true, the singleton will be add to Dont Destroy On Load Scene in initialization")]
        [SerializeField] private bool _isGlobal; 

        private static T _localInstance;

        public static T LocalInstance
        {
            get
            {
                if (_localInstance == null)
                {
                    _localInstance = FindObjectOfType<T>();

                    if (_localInstance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _localInstance = singletonObject.AddComponent<T>();
                    }
                }

                return _localInstance;
            }
        }

        protected virtual void Awake()
        {
            if (_localInstance != null && _localInstance != this)
            {
                Debug.LogError($"More then one instance of: {typeof(T).Name}");
                Destroy(gameObject);
                return;
            }

            _localInstance = this as T;

            if (_isGlobal)
            {
                DontDestroyOnLoad(gameObject); 
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy(); 
            
            if (_localInstance == this)
            {
                _localInstance = null;
            }
        }
    }
}
