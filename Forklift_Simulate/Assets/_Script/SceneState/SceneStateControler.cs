using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateControler {

    ISceneState m_SceneState = null;
    bool m_bRunBegin = false;

    public SceneStateControler() { }

    AsyncOperation asyn;

    /// <summary>
    /// 設定要換到的場景
    /// </summary>
    /// <param name="State"></param>
    /// <param name="LoadSceneName"></param>
    public void SetState(ISceneState State,string LoadSceneName)
    {
        m_bRunBegin = false;

        //載入場景
        LoadScene(LoadSceneName);

        if (m_SceneState != null)
        {
            m_SceneState.StateEnd();
        }

        //設定State
        m_SceneState = State;
    }

    /// <summary>
    /// 載入場景
    /// </summary>
    /// <param name="LoadSceneName"></param>
    void LoadScene(string LoadSceneName)
    {
        if (LoadSceneName == null || LoadSceneName.Length == 0)
        {
            return;
        }
        asyn = PhotonNetwork.LoadLevelAsync(LoadSceneName);//連線用
        //asyn = SceneManager.LoadSceneAsync(LoadSceneName);
    }

    /// <summary>
    /// Start跟Update在這裡運作
    /// </summary>
    public void StateUpdate()
    {
        //Debug.Log(asyn);
        //是否載入場景
        if (asyn != null)
            if (asyn.progress < 1) return;

        //通知新的State開始
        if(m_SceneState!=null&& m_bRunBegin == false)
        {
            m_SceneState.StateBegin();
            m_bRunBegin = true;
        }

        if (m_SceneState != null)
        {
            m_SceneState.StateUpdate();
        }
    }
}
