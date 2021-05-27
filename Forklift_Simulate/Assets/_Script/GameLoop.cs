using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour {

    // 場景狀態
    SceneStateControler m_SceneStateControler = new SceneStateControler();

    void Start()
    {
        // 設定起始的場景
        m_SceneStateControler.SetState(new TitleState(m_SceneStateControler), "");
    }

    void Update()
    {
        m_SceneStateControler.StateUpdate();
    }
}

