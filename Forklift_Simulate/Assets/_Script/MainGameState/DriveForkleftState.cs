using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveForkleftState : IMainGameState
{
    public DriveForkleftState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "CompleteState";
    }

    float delayScoreCount=0;
    MainGameManager.GameMode _gameMode; 

    GameObject _ScoreGroupCanvas;
    public override void StateBegin()
    {
        delayScoreCount = 0;
        MainGameManager.Instance.CreateForkkit();
        MainGameManager.Instance.ScoreManagers.Init();

        MainGameManager.Instance.ForkleftObj.GetComponent<WSMGameStudio.Vehicles.WSMVehicleController>().enabled = true;
        _gameMode = MainGameManager.Instance.GameModes;

        if (_gameMode == MainGameManager.GameMode.PracticeMode)
        {
            _ScoreGroupCanvas = GameObject.Instantiate(MainGameManager.Instance.ScoreGroupCanvass);
        }
        else
        {
            _ScoreGroupCanvas = GameObject.Instantiate(MainGameManager.Instance.ScoreGroupCanvass,
                                                        MainGameManager.Instance.ForkitCanvasPoss.transform);
        }

    }
    public override void StateUpdate()
    {
        //延遲出現分數版
        delayScoreCount += Time.deltaTime;
        if ((int)delayScoreCount == 1)
        {
            MainGameManager.Instance.ScoreManagers.enabled = true;
        }

        if (_gameMode == MainGameManager.GameMode.PracticeMode)
        {

        }
        else if (_gameMode == MainGameManager.GameMode.TestMode)
        {
            if (MainGameManager.Instance.IsSussuesPassTest == 0)
            {
                m_Conrtoller.SetState(MainGameStateControl.GameFlowState.CompleteTest, m_Conrtoller);
            }
        }

    }

    public override void StateEnd()
    {

    }

}
