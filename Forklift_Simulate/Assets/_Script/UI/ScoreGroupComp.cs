using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGroupComp : MonoBehaviour
{
    [SerializeField]
    GameObject[] StateBoard;
    [SerializeField]
    Text[] ScoreTxt;
    [SerializeField]
    Text[] TimeTxt;
    [SerializeField]
    GameObject[] StateTextGroup;

    [SerializeField]
    GameObject StatePerfab;

    [SerializeField]
    GameObject ScoreTestCanvas;
    [SerializeField]
    GameObject StateTextGroup_Test;
    [SerializeField]
    Text TimeTxt_Test;
    [SerializeField]
    Text titleTxt_Test;

    ScoreManager scoreManager;

    int TimeScore;
    int pipeFallScore;
    int SpeedToHightScore;
    int ForkPositionHightScore;
    int ForkMastTiltScore;
    int ForkHandBrakeScore;
    int ForkCluthScore;
    int ForkBackFrontNorStopScore;
    int OnRoadNotEngineScore;

    string contentString;

    string TimeScoreTxt = "超時駕駛";
    string pipeFallScoreTxt = "堆高機撞到柱子";
    string SpeedToHightScoreTxt = "堆高機超速駕駛";
    string ForkPositionHightScoreTxt = "堆高機行駛前未事先升高貨插";
    string ForkMastTiltScoreTxt = "堆高機行駛前未事先傾斜貨插";
    string ForkHandBrakeScoreTxt = "堆高機行駛前手煞車未放";
    string ForkCluthScoreTxt = "堆高機行駛時誤踩吋動踏板";
    string ForkBackFrontNorStopScoreTxt = "堆高機行駛時突然變換前後檔";
    string OnRoadNotEngineScoreTxt = "堆高機行駛時熄火";


    MainGameManager.GameMode GameMode;

    void Start()
    {
        scoreManager = MainGameManager.Instance.ScoreManagers;
        scoreManager.OnTimeScore += OnTimeScore;
        scoreManager.OnPipeFallScore += OnPipeFallScore;
        scoreManager.OnSpeedToHightScore += OnSpeedToHightScore;
        scoreManager.OnForkPositionHightScore += OnForkPositionHightScore;
        scoreManager.OnForkMastTiltScore += OnForkMastTiltScore;
        scoreManager.OnForkHandBrakeScore += OnForkHandBrakeScore;
        scoreManager.OnForkCluthScore += OnForkCluthScore;
        scoreManager.OnForkBackFrontNorStopScore += OnForkBackFrontNorStopScore;
        scoreManager.OnOnRoadNotEngineScore += OnOnRoadNotEngineScore;

        if (GameMode == MainGameManager.GameMode.PracticeMode)
        {
            OnPracticeMode();

        }
        else if (GameMode == MainGameManager.GameMode.TestMode)
        {
            OnTestMode();
           
        }
    }

    void Update()
    {
        //狀態
        GameMode = MainGameManager.Instance.GameModes;
        Debug.Log("GameMode"+GameMode);
        //計時
        Clock();
        //分數
        TimeScore = scoreManager.TimeScore;
        pipeFallScore = scoreManager.pipeFallScore;
        SpeedToHightScore = scoreManager.SpeedToHightScore;
        ForkPositionHightScore = scoreManager.ForkPositionHightScore;
        ForkMastTiltScore = scoreManager.ForkMastTiltScore;
        ForkHandBrakeScore = scoreManager.ForkHandBrakeScore;
        ForkCluthScore = scoreManager.ForkCluthScore;
        ForkBackFrontNorStopScore = scoreManager.ForkBackFrontNorStopScore;
        OnRoadNotEngineScore = scoreManager.OnRoadNotEngineScore;

        if(GameMode == MainGameManager.GameMode.PracticeMode)
        {
            OnPracticeMode();
        }
        if (GameMode == MainGameManager.GameMode.TestMode)
        {
            if(MainGameManager.Instance.IsSussuesPassTest == 0)
            {
                ScoreTestCanvas.SetActive(true);
                titleTxt_Test.text = "測驗失敗";
            }
            //else if (MainGameManager.Instance.TotalWrongScore < 2)
            //{
            //    ScoreTestCanvas.SetActive(true);
            //    titleTxt_Test.text = "測驗成功";
            //}

        }

    }

    private float m_Timer;
    private int m_Hour;//時
    private int m_Minute;//分
    private int m_Second;//秒

    void Clock()
    {
        m_Timer = scoreManager.Timer;
        m_Second = (int)m_Timer;
        if (m_Second > 59.0f)
        {
            m_Second = (int)(m_Timer - (m_Minute * 60));
        }
        m_Minute = (int)(m_Timer / 60);
        if (m_Minute > 59.0f)
        {
            m_Minute = (int)(m_Minute - (m_Hour * 60));
        }
        m_Hour = m_Minute / 60;
        if (m_Hour >= 24.0f)
        {
            m_Timer = 0;
        }

        //練習模式
        foreach (var txt in TimeTxt)
        {
            txt.text = string.Format("經過時間：{0:d2}:{1:d2}:{2:d2}", m_Hour, m_Minute, m_Second);
        }

        //測驗模式
        TimeTxt_Test.text = string.Format("經過時間：{0:d2}:{1:d2}:{2:d2}", m_Hour, m_Minute, m_Second);
    }

    void OnPracticeMode()
    {
        for(int i = 0; i < TimeTxt.Length; i++)
        {
            TimeTxt[i].enabled = true;
            ScoreTxt[i].enabled = true;
            StateBoard[i].SetActive(true);
        }
        ScoreTestCanvas.SetActive(false);

    }

    void OnTestMode()
    {
        for (int i = 0; i < TimeTxt.Length; i++)
        {
            TimeTxt[i].enabled = false;
            ScoreTxt[i].enabled = false;
            StateBoard[i].SetActive(false);
        }
        ScoreTestCanvas.SetActive(false);

    }

    void OnTimeScore(int i)
    {
        if (i > 1) return;
        foreach (var stateTextGroup in StateTextGroup)
        {
            GameObject obj = Instantiate(StatePerfab, stateTextGroup.transform);
            obj.GetComponent<StateComp>().Label.text = TimeScoreTxt;
        }

        //測驗模式
        GameObject obj_test = Instantiate(StatePerfab, StateTextGroup_Test.transform);
        obj_test.GetComponent<StateComp>().Label.text = TimeScoreTxt;     
    }

    bool isPipeAppear = false;
    void OnPipeFallScore(int i)
    {
        if (isPipeAppear) return;
        foreach (var stateTextGroup in StateTextGroup)
        {
            isPipeAppear = true;
            GameObject obj = Instantiate(StatePerfab, stateTextGroup.transform);
            obj.GetComponent<StateComp>().Label.text = pipeFallScoreTxt;
        }

        //測驗模式
        GameObject obj_test = Instantiate(StatePerfab, StateTextGroup_Test.transform);
        obj_test.GetComponent<StateComp>().Label.text = pipeFallScoreTxt;
    }
    void OnSpeedToHightScore(int i)
    {
        if (i > 1) return;

        foreach (var stateTextGroup in StateTextGroup)
        {
            GameObject obj = Instantiate(StatePerfab, stateTextGroup.transform);
            obj.GetComponent<StateComp>().Label.text = SpeedToHightScoreTxt;
        }

        //測驗模式
        GameObject obj_test = Instantiate(StatePerfab, StateTextGroup_Test.transform);
        obj_test.GetComponent<StateComp>().Label.text = SpeedToHightScoreTxt;
    }
    void OnForkPositionHightScore(int i)
    {
        if (i > 1) return;

        foreach (var stateTextGroup in StateTextGroup)
        {
            GameObject obj = Instantiate(StatePerfab, stateTextGroup.transform);
            obj.GetComponent<StateComp>().Label.text = ForkPositionHightScoreTxt;
        }

        //測驗模式
        GameObject obj_test = Instantiate(StatePerfab, StateTextGroup_Test.transform);
        obj_test.GetComponent<StateComp>().Label.text = ForkPositionHightScoreTxt;
    }
    void OnForkMastTiltScore(int i)
    {
        if (i > 1) return;

        foreach (var stateTextGroup in StateTextGroup)
        {
            GameObject obj = Instantiate(StatePerfab, stateTextGroup.transform);
            obj.GetComponent<StateComp>().Label.text = ForkMastTiltScoreTxt;
        }

        //測驗模式
        GameObject obj_test = Instantiate(StatePerfab, StateTextGroup_Test.transform);
        obj_test.GetComponent<StateComp>().Label.text = ForkMastTiltScoreTxt;
    }
    void OnForkHandBrakeScore(int i)
    {
        if (i > 1) return;

        foreach (var stateTextGroup in StateTextGroup)
        {
            GameObject obj = Instantiate(StatePerfab, stateTextGroup.transform);
            obj.GetComponent<StateComp>().Label.text = ForkHandBrakeScoreTxt;
        }

        //測驗模式
        GameObject obj_test = Instantiate(StatePerfab, StateTextGroup_Test.transform);
        obj_test.GetComponent<StateComp>().Label.text = ForkHandBrakeScoreTxt;
    }
    void OnForkCluthScore(int i)
    {
        if (i > 1) return;

        foreach (var stateTextGroup in StateTextGroup)
        {
            GameObject obj = Instantiate(StatePerfab, stateTextGroup.transform);
            obj.GetComponent<StateComp>().Label.text = ForkCluthScoreTxt;
        }

        //測驗模式
        GameObject obj_test = Instantiate(StatePerfab, StateTextGroup_Test.transform);
        obj_test.GetComponent<StateComp>().Label.text = ForkCluthScoreTxt;
    }
    void OnForkBackFrontNorStopScore(int i)
    {
        if (i > 1) return;
        foreach (var stateTextGroup in StateTextGroup)
        {
            GameObject obj = Instantiate(StatePerfab, stateTextGroup.transform);
            obj.GetComponent<StateComp>().Label.text = ForkBackFrontNorStopScoreTxt;
        }

        //測驗模式
        GameObject obj_test = Instantiate(StatePerfab, StateTextGroup_Test.transform);
        obj_test.GetComponent<StateComp>().Label.text = ForkBackFrontNorStopScoreTxt;
    }
    void OnOnRoadNotEngineScore(int i)
    {
        if (i > 1) return;

        foreach (var stateTextGroup in StateTextGroup)
        {
            GameObject obj = Instantiate(StatePerfab, stateTextGroup.transform);
            obj.GetComponent<StateComp>().Label.text = OnRoadNotEngineScoreTxt;
        }

        //測驗模式
        GameObject obj_test = Instantiate(StatePerfab, StateTextGroup_Test.transform);
        obj_test.GetComponent<StateComp>().Label.text = OnRoadNotEngineScoreTxt;
    }
}
