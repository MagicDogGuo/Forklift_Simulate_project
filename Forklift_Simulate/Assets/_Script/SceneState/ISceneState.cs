using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISceneState {

    //狀態名稱
    string m_StateName = "ISceneState";
    public string StateName
    {
        get { return m_StateName; }
        set { m_StateName = value; }
    }

    //狀態控制
    protected SceneStateControler m_Controler = null;

    //建構子
    public ISceneState(SceneStateControler Controler)
    {
        m_Controler = Controler;
    }

    //開始
    public virtual void StateBegin() { }

    //更新 
    public virtual void StateUpdate() { }

    //結束 
    public virtual void StateEnd() { }
}
