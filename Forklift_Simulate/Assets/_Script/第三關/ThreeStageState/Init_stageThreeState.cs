using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init_stageThreeState : IMainGameState
{
    public Init_stageThreeState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "Init_stageThree";
    }
    public override void StateBegin()
    {
        //MainGameManager.Instance.ScoreManagers.enabled = false;

        //MainGameManager.Instance.InitPlayerCam.SetActive(true);

        //GameEventSystem.Instance.OnPushTestModeBtn += OnPushTestBtn;
        //GameEventSystem.Instance.OnPushPracticeModeBtn += OnPushPracticeBtn;

        StageThreeGameManager.Instance.InstantiateInitObject_stageThree();
    }
    public override void StateUpdate()
    {
        m_Conrtoller.SetState(MainGameStateControl.GameFlowState.DriveForkKit_stageThree, m_Conrtoller);
        
    }

    public override void StateEnd()
    {
        //MainGameManager.Instance.InitPlayerCam.SetActive(false);

        //GameObject.Destroy(MainGameManager.Instance.InitCanvass);
        ////GameObject.Destroy(MainGameManager.Instance.InitPlayerCam);

    }


    void OnPushPracticeBtn()
    {
        MainGameManager.Instance.GameModes = MainGameManager.GameMode.PracticeMode;
        m_Conrtoller.SetState(MainGameStateControl.GameFlowState.DriveForkKit_stageThree, m_Conrtoller);
    }

    void OnPushTestBtn()
    {
        MainGameManager.Instance.GameModes = MainGameManager.GameMode.TestMode;
        m_Conrtoller.SetState(MainGameStateControl.GameFlowState.DriveForkKit_stageThree, m_Conrtoller);
    }
}
