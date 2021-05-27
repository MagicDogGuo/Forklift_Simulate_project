using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleState : ISceneState
{

    public TitleState(SceneStateControler Controler) : base(Controler)
    {
        this.StateName = "TitleState";
    }

    public override void StateBegin()
    {
        //BGM
        //GetAudioSource comp = AudioManager.Instance.GetComponent<GetAudioSource>();
        //if (comp != null) comp.PlayBGM(GetAudioSource.BGMKinds.MainMenu);

        //GameEventSystem.Instance.OnPushLogInBtn += LoginGame;
        //GameEventSystem.Instance.OnPushExitBtn += ExitGame;
    }

    public override void StateUpdate()
    {
        LoginGame();
    }

    public override void StateEnd()
    {
        //GameEventSystem.Instance.OnPushStartGameBtn -= StartGame;
        //GameEventSystem.Instance.OnPushExitGameBtn -= ExitGame;
    }

    void LoginGame()
    {
        m_Controler.SetState(new MainGameState(m_Controler), "MainGameState");
    }


    void ExitGame()
    {
        
        //System.Diagnostics.Process.GetCurrentProcess().Kill(); //強制關閉
        Application.Quit();

        Debug.Log("exit");
    }
}
