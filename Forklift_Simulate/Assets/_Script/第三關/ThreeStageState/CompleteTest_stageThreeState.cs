using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Vehicles;

public class CompleteTest_stageThreeState : IMainGameState
{
    public CompleteTest_stageThreeState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "CompleteTest_stageThreeState";
    }

    public override void StateBegin()
    {
        string PassResult = "";


        Debug.Log("============測驗結束" + StageThreeGameManager.Instance.TotalWrongScore);

        if (StageThreeGameManager.Instance.IsSussuesPassTest == 1)
        {
            Debug.Log("============測驗成功");
            PassResult = "通過";
        }
        else if (StageThreeGameManager.Instance.IsSussuesPassTest == 0)
        {
            Debug.Log("============測驗失敗");
            PassResult = "未通過";

        }


        //紀錄
        int WrongAmount;
        string[] WrongContent;

        WrongAmount = ScoreGroupComp.WrongAmount;
        WrongContent = ScoreGroupComp.RecoedList.ToArray();
        RecordUserDate.RecordUserData_ThirdState(PassResult, WrongAmount, WrongContent);

    }
    public override void StateUpdate()
    {
        StageThreeGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().HandBrakeInput = 25;
        StageThreeGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().StopGameBrake();
        StageThreeGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().IsEngineOn = false;
        StageThreeGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().enabled = false;

        if (Input.GetKeyDown(KeyCode.R))
        {
            m_Conrtoller.SetState(MainGameStateControl.GameFlowState.Init_stageThree, m_Conrtoller);
        }
    }

    public override void StateEnd()
    {
        StageThreeGameManager.Instance.ScoreManagers.ReleaseEvent();
        StageThreeGameManager.Instance.DestoryForkkit();
        GameObject.Destroy(StageThreeGameManager.Instance.PipeGroupObjs);

    }

}
