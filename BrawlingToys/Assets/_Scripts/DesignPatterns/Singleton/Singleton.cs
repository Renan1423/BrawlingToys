using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
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

        DontDestroyOnLoad(transform.root.gameObject);
        DontDestroyOnLoad(gameObject);
        instance = this as T;
    }
}
