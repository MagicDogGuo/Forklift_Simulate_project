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
        StageThreeGameManager.Instance.ScoreManagers.enabled = false;

        StageThreeGameManager.Instance.InitPlayerCam.SetActive(true);

        GameEventSystem.Instance.OnPushTestModeBtn = null ;
        GameEventSystem.Instance.OnPushPracticeModeBtn = null;

        GameEventSystem.Instance.OnPushTestModeBtn += OnPushTestBtn;
        GameEventSystem.Instance.OnPushPracticeModeBtn += OnPushPracticeBtn;

        StageThreeGameManager.Instance.InstantiateInitObject_stageThree();

    }
    public override void StateUpdate()
    {
        //m_Conrtoller.SetState(MainGameStateControl.GameFlowState.DriveForkKit_stageThree, m_Conrtoller);

    }

    public override void StateEnd()
    {
        StageThreeGameManager.Instance.InitPlayerCam.SetActive(false);

        GameObject.Destroy(MainGameManager.Instance.InitCanvass);

    }


    void OnPushPracticeBtn()
    {
        StageThreeGameManager.Instance.GameModes = StageThreeGameManager.GameMode.PracticeMode;
        m_Conrtoller.SetState(MainGameStateControl.GameFlowState.DriveForkKit_stageThree, m_Conrtoller);
    }

    void OnPushTestBtn()
    {
        StageThreeGameManager.Instance.GameModes = StageThreeGameManager.GameMode.TestMode;
        m_Conrtoller.SetState(MainGameStateControl.GameFlowState.DriveForkKit_stageThree, m_Conrtoller);
    }
}
