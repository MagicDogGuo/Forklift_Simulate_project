using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMainGameState{

    //狀態名稱
    string m_StateName = "IMainGameState";
    public string StateName
    {
        get { return m_StateName; }
        set { m_StateName = value; }
    }

    //狀態控制者
    protected MainGameStateControl m_Conrtoller = null;

    //建構狀態
    public IMainGameState(MainGameStateControl Controller)
    {
        m_Conrtoller = Controller;
    }

    //開始
    public virtual void StateBegin() { }

    //結束
    public virtual void StateEnd() { }

    //更新
    public virtual void StateUpdate() { }
}
