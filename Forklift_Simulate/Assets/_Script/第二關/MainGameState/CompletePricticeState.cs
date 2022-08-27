using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletePricticeState : IMainGameState
{
    public CompletePricticeState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "CompletePricticeState";
    }
    GameObject WarningUI;

    public override void StateBegin()
    {
        Debug.Log("完成練習!");

        if (WarningUI == null) WarningUI = GameObject.Instantiate(MainGameManager.Instance.WarningUIs, MainGameManager.Instance.ForkitCanvasPoss.transform);
        WarningUI.GetComponentInChildren<Text>().text = "完成練習!";
        WarningUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        //紀錄
        string PassResult;
        int WrongAmount;
        string[] WrongContent;


        PassResult = "完成練習!";
        WrongAmount = ScoreGroupComp.WrongAmount;
        WrongContent = ScoreGroupComp.RecoedList.ToArray();
        RecordUserDate.RecordUserData_SecondState(PassResult, WrongAmount, WrongContent);
    }
    public override void StateUpdate()
    {
        Debug.Log("完成練習!");
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject.Destroy(WarningUI);
            m_Conrtoller.SetState(MainGameStateControl.GameFlowState.Init, m_Conrtoller);

        }

    }

    public override void StateEnd()
    {
        MainGameManager.Instance.ScoreManagers.ReleaseEvent();
        MainGameManager.Instance.DestoryForkkit();
        GameObject.Destroy(MainGameManager.Instance.PipeGroupObjs);
    }
}
