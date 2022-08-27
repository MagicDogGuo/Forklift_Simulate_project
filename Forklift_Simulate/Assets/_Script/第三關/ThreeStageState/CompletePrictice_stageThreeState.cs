using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CompletePrictice_stageThreeState : IMainGameState
{
    public CompletePrictice_stageThreeState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "CompletePrictice_stageThreeState";
    }
    GameObject WarningUI;

    public override void StateBegin()
    {
        Debug.Log("完成練習!");

        if (WarningUI == null) WarningUI = GameObject.Instantiate(StageThreeGameManager.Instance.WarningUIs, StageThreeGameManager.Instance.ForkitCanvasPoss.transform);
        WarningUI.GetComponentInChildren<Text>().text = "完成練習!";
        WarningUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        //紀錄
        string PassResult;
        int WrongAmount;
        string[] WrongContent;


        PassResult = "完成練習!";
        WrongAmount = ScoreGroupComp.WrongAmount;
        WrongContent = ScoreGroupComp.RecoedList.ToArray();
        RecordUserDate.RecordUserData_ThirdState(PassResult, WrongAmount, WrongContent);
    }
    public override void StateUpdate()
    {
        Debug.Log("完成練習!");
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject.Destroy(WarningUI);
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
