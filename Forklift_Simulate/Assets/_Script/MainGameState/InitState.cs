using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitState : IMainGameState
{
    public InitState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "InitState";
    }
    public override void StateBegin()
    {
        m_Conrtoller.SetState(MainGameStateControl.GameFlowState.DriveForkKit, m_Conrtoller);

    }
    public override void StateUpdate()
    {
    }

    public override void StateEnd()
    {
       
    }

    void LoginGame()
    {
    }
}
