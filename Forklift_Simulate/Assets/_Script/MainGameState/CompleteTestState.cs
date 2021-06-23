using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteTestState : IMainGameState
{
    public CompleteTestState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "CompleteTestState";
    }

    public override void StateBegin()
    {
        Debug.Log("============測驗結束"+ MainGameManager.Instance.TotalWrongScore);

    }
    public override void StateUpdate()
    {
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMGameStudio.Vehicles.WSMVehicleController>().HandBrakeInput = 25;
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMGameStudio.Vehicles.WSMVehicleController>().StopGameBrake();
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMGameStudio.Vehicles.WSMVehicleController>().enabled = false;

        if (Input.GetKeyDown(KeyCode.R))
        {
            MainGameManager.Instance.ScoreManagers.ReleaseEvent();
            m_Conrtoller.SetState(MainGameStateControl.GameFlowState.Init, m_Conrtoller);
        }
    }

    public override void StateEnd()
    {
        MainGameManager.Instance.DestoryForkkit();
        GameObject.Destroy(MainGameManager.Instance.PipeGroupObjs);

    }

    void BackToStart()
    {

    }
}
