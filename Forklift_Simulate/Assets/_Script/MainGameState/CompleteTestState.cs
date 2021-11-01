using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Vehicles;
public class CompleteTestState : IMainGameState
{
    public CompleteTestState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "CompleteTestState";
    }

    public override void StateBegin()
    {
        Debug.Log("============測驗結束"+ MainGameManager.Instance.TotalWrongScore);

        if (MainGameManager.Instance.IsSussuesPassTest == 1)
        {
            Debug.Log("============測驗成功");
        }
        else if ( MainGameManager.Instance.IsSussuesPassTest == 0)
        {
            Debug.Log("============測驗失敗");
        }


    }
    public override void StateUpdate()
    {
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().HandBrakeInput = 25;
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().StopGameBrake();
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().IsEngineOn = false;
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().enabled = false;

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
