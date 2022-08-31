using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonUpdateTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainGameManager.Instance.MainGameBegin();
    }

    // Update is called once per frame
    void Update()
    {
        MainGameManager.Instance.MainGameUpdate();

    }
}