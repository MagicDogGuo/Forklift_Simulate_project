using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : IMainGameState
{
    public InitState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "InitState";
    }
    public override void StateBegin()
    {
        MainGameManager.Instance.ScoreManagers.enabled = false;

        MainGameManager.Instance.InitPlayerCam.SetActive(true);

        GameEventSystem.Instance.OnPushTestModeBtn += OnPushTestBtn;
        GameEventSystem.Instance.OnPushPracticeModeBtn += OnPushPracticeBtn;

        MainGameManager.Instance.InstantiateInitObject();
    }
    public override void StateUpdate()
    {
    }

    public override void StateEnd()
    {
        MainGameManager.Instance.InitPlayerCam.SetActive(false);

        GameObject.Destroy(MainGameManager.Instance.InitCanvass);
        //GameObject.Destroy(MainGameManager.Instance.InitPlayerCam);

    }


    void OnPushPracticeBtn()
    {
        MainGameManager.Instance.GameModes = MainGameManager.GameMode.PracticeMode;
        m_Conrtoller.SetState(MainGameStateControl.GameFlowState.DriveForkKit, m_Conrtoller);
    }

    void OnPushTestBtn()
    {
        MainGameManager.Instance.GameModes = MainGameManager.GameMode.TestMode;
        m_Conrtoller.SetState(MainGameStateControl.GameFlowState.DriveForkKit, m_Conrtoller);
    }
}
