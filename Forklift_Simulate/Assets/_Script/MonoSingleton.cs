using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T: MonoBehaviour {

    static T m_Instance;
    static bool m_IsDestroyed;  

    public static bool IsNull()
    {
        return m_Instance = null;
    }

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType(typeof(T)) as T;

                if (m_Instance == null)
                {
                    var gameObject = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(gameObject);
                    m_Instance = gameObject.AddComponent(typeof(T)) as T;
                }
            }
            return m_Instance;
        }
    }
}
