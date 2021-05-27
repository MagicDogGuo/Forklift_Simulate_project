using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameState : ISceneState
{
    public MainGameState(SceneStateControler controler) : base(controler)
    {
        this.StateName = "MainGameState";
    }

    public override void StateBegin()
    {
        MainGameManager.Instance.MainGameBegin();
        //GameEventSystem.Instance.OnPushBackMainMenu_MainGameBtn += PushBackMainMenu_MainGameBtn;
    }

    public override void StateUpdate()
    {
        MainGameManager.Instance.MainGameUpdate();
    }

    public override void StateEnd()
    {
        //GameEventSystem.Instance.DisRegistEvents_MainGame();
    }

    void PushBackMainMenu_MainGameBtn()
    {
        m_Controler.SetState(new TitleState(m_Controler), "TitleState");

    }

}
