using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventSystem : MonoSingleton<GameEventSystem> {
    
    ////標題頁
    //public UnityAction OnPushLogInBtn;
    //public UnityAction OnPushExitBtn;
    //public UnityAction OnPushInfoBtn;

    ////主選單
    //public UnityAction OnPushBackTitleBtn;
    //public UnityAction<int> OnPushGameSceneBtn;

    //主遊戲
    public UnityAction OnPushTestModeBtn;
    public UnityAction OnPushPracticeModeBtn;


    //public void DisRegistEvents_Title()
    //{
    //    OnPushLogInBtn = null;
    //    OnPushExitBtn = null;
    //}


    //public void DisRegistEvents_MainMenu()
    //{
    //    OnPushBackTitleBtn = null;
    //    OnPushGameSceneBtn = null;
    //}

    public void DisRegistEvents_MainGame()
    {
        OnPushTestModeBtn = null;
        OnPushPracticeModeBtn = null;
    }

    
}
