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
        string PassResult="";


        Debug.Log("============測驗結束"+ MainGameManager.Instance.TotalWrongScore);

        if (MainGameManager.Instance.IsSussuesPassTest == 1)
        {
            Debug.Log("============測驗成功");
            PassResult = "通過";
        }
        else if ( MainGameManager.Instance.IsSussuesPassTest == 0)
        {
            Debug.Log("============測驗失敗");
            PassResult = "未通過";

        }


        //紀錄
        int WrongAmount;
        string[] WrongContent;

        WrongAmount = ScoreGroupComp.WrongAmount;
        WrongContent = ScoreGroupComp.RecoedList.ToArray();
        RecordUserDate.RecordUserData_SecondState(PassResult, WrongAmount, WrongContent);

    }
    public override void StateUpdate()
    {
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().HandBrakeInput = 25;
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().StopGameBrake();
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().IsEngineOn = false;
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().enabled = false;

        if (Input.GetKeyDown(KeyCode.R))
        {
            m_Conrtoller.SetState(MainGameStateControl.GameFlowState.Init, m_Conrtoller);
        }
    }

    public override void StateEnd()
    {
        MainGameManager.Instance.ScoreManagers.ReleaseEvent();
        MainGameManager.Instance.DestoryForkkit();
        GameObject.Destroy(MainGameManager.Instance.PipeGroupObjs);

    }

    void BackToStart()
    {

    }
}
