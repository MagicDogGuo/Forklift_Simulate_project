using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonLoader : MonoSingleton<SingletonLoader> {

    private void Awake()
    {
        Object[] count = FindObjectsOfType(typeof(SingletonLoader));

        if (count.Length == 1)
        {
            GameObject go = this.transform.gameObject;

            //要考慮順序
            //go.AddComponent<DatabaseManager>();
            //go.AddComponent<GameEventSystem>();
            go.AddComponent<GameLoop>();
            //go.AddComponent<AudioManager>();
            //go.AddComponent<RobotInterfaceManager>();
            DontDestroyOnLoad(this.gameObject);
        }

        if (count.Length > 1)
        {
            DestroyImmediate(this.gameObject);
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Time.timeScale = 3;
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Time.timeScale = 1;
        //}
    }
}
