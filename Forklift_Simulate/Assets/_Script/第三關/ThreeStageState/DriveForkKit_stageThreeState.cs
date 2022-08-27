using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Vehicles;
using WSMGameStudio.HeavyMachinery;
using UnityEngine.UI;

public class DriveForkKit_stageThreeState : IMainGameState
{
    public DriveForkKit_stageThreeState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "DriveForkKit_stageThree";
    }

    float delayScoreCount = 0;
    MainGameManager.GameMode _gameMode;
    StartPoint _startPoint_Forkleft;
    EndPoint _endPoint_Forkleft;
    GoodsStartPoint _startPoint_Goods;
    GoodsEndPoint _endPoint_Goods;
    GoodsEndPoint _endPoint_Goods02;
    GameObject _ScoreGroupCanvas;
    WSMVehicleController _wSMVehicleController;
    ForkliftController _forkliftController;
    GameObject WarningUI;
    ScoreManager _scoreManager;
    GameObject WarningUI_退後;
    GameObject WarningUI_超出格子;

    List<GameObject> GoodsList;
    List<Vector3> GoodsOriPosList;
    List<Vector3> GoodsOriRotList;




    bool _isCountScore_ScoreManager = false;
    bool isStopNow = false;

    LogtichControl logtichControl;



    //bool stepOne_拉起手煞 = false;
    //bool stepOne_前後檔回歸 = false;
    //bool stepOne_升降拉桿放回 = false;
    //bool stepTwo_開啟手煞 = false;
    //bool stepTwo_拉到退檔 = false;
    //bool stepTwo_升降拉桿上升 = false;
    //bool stepIsAllOK = false;


    public override void StateBegin()
    {   //wheel歸位
        WheelBackPos();

        //stepIsAllOK = false;

        delayScoreCount = 0;
        StageThreeGameManager.Instance.CreateForkkit();
        StageThreeGameManager.Instance.ScoreManagers.Init();
        _isCountScore_ScoreManager = true;
        isStopNow = false;
        MainGameManager.Instance.IsForkitOnRoadOutLineObj.Init();

        _wSMVehicleController = StageThreeGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>();
        _forkliftController = StageThreeGameManager.Instance.ForkleftObj.GetComponent<ForkliftController>();
        _gameMode = StageThreeGameManager.Instance.GameModes;
        _scoreManager = StageThreeGameManager.Instance.ScoreManagers;

        _wSMVehicleController.enabled = true;



        if (_gameMode == MainGameManager.GameMode.PracticeMode)
        {
            _ScoreGroupCanvas = GameObject.Instantiate(StageThreeGameManager.Instance.ScoreGroupCanvass,
                                                        StageThreeGameManager.Instance.ForkitCanvasPoss.transform);

        }

        else if (_gameMode == MainGameManager.GameMode.TestMode)
        {
            _ScoreGroupCanvas = GameObject.Instantiate(StageThreeGameManager.Instance.ScoreGroupCanvass,
                                                        StageThreeGameManager.Instance.ForkitCanvasPoss.transform);

            //判斷回原位(撞柱子、壓線)
            _scoreManager.OnPipeFallScore += OnPipeFall_Test;
            _scoreManager.OnForkitOnLineScore += OnForkitOnLine_Test;

 
        }


        _startPoint_Forkleft = StageThreeGameManager.Instance.StartPointObjs.GetComponent<StartPoint>();
        //_endPoint = MainGameManager.Instance.EndPointObjs.GetComponent<EndPoint>();

        _startPoint_Goods = StageThreeGameManager.Instance.StartPointObj_Goods.GetComponent<GoodsStartPoint>();
        _endPoint_Goods = StageThreeGameManager.Instance.EndPointObj_Goods.GetComponent<GoodsEndPoint>();
        _endPoint_Goods02 = StageThreeGameManager.Instance.EndPointObj_Goods02s.GetComponent<GoodsEndPoint>();

        GoodsList = new List<GameObject>();
        GoodsOriPosList = new List<Vector3>();
        GoodsOriRotList = new List<Vector3>();

        Goods[] gs =  GameObject.FindObjectsOfType<Goods>();
        foreach(var g in gs)
        {
            Debug.Log("=================================" + g.name);
            GoodsList.Add(g.gameObject);
            GoodsOriPosList.Add(g.gameObject.transform.position);
            GoodsOriRotList.Add(g.gameObject.transform.eulerAngles);
        }

        logtichControl = GameObject.FindObjectOfType<LogtichControl>();


        ////測驗中
        //MainGameManager.Instance.IsSussuesPassTest = 2;



    }
    public override void StateUpdate()
    {

        ////測試記錄用
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    m_Conrtoller.SetState(MainGameStateControl.GameFlowState.CompletePrictice, m_Conrtoller);

        //}


        //string t = "_startPoint.isOnStartPoint_Forkit: " + _startPoint.isOnStartPoint_Forkit
        //  + "\n _endPoint.isAllreadyArraivalEndPoint: " + _endPoint.isAllreadyArraivalEndPoint
        //  + "\n _wSMVehicleController.CurrentHandbrake == 1: " + (_wSMVehicleController.CurrentHandbrake == 1)
        //  + "\n _wSMVehicleController.CurrentBackFront == 0: " + (_wSMVehicleController.CurrentBackFront == 0)
        //  + "\n _forkliftController.CurrentForksVertical <= 0.02f: " + (_forkliftController.CurrentForksVertical <= 0.02f)
        //  + "\n _forkliftController.CurrentMastTilt > 0.6f: " + (_forkliftController.CurrentMastTilt > 0.6f);
        //MainGameManager.Instance.TestText.GetComponent<Text>().text = t;

        //延遲出現分數版
        delayScoreCount += Time.deltaTime;
        if ((int)delayScoreCount >= 1 && _isCountScore_ScoreManager)
        {

            StageThreeGameManager.Instance.ScoreManagers.ScoreUpdate();

            //MainGameManager.Instance.ScoreManagers.enabled = true;
        }


        ////Debug.Log("=====IsSussuesPassTest:" + MainGameManager.Instance.IsSussuesPassTest);

        if (_gameMode == StageThreeGameManager.GameMode.PracticeMode)
        {
            OnPractice();
        }
        else if (_gameMode == StageThreeGameManager.GameMode.TestMode)
        {
            OnTest();
        }
        //隨時歸位
        if (logtichControl.CheckEnterUI || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.O))
        {
            if (!_startPoint_Forkleft.isOnStartPoint_Forkit) BackToOri();
        }
        ////到終點
        //if (Input.GetKey(KeyCode.K))
        //{
        //    ToEndPoint();
        //}

    }

    void BackToOri()
    {
        Debug.Log("sssssssssssssssssssssssss");
        MainGameManager.Instance.ForkleftObj.transform.position
                = MainGameManager.Instance.ForkitOriPoss.position;

        MainGameManager.Instance.ForkleftObj.transform.localRotation
            = MainGameManager.Instance.ForkitOriPoss.localRotation;

        //pipe歸位
        MainGameManager.Instance.PipeGroupObjs.GetComponent<PipeDetectControl>().PipeBackToOri();

        //wheel歸位
        WheelBackPos();

        if (WarningUI != null) GameObject.Destroy(WarningUI);
    }

    void ToEndPoint()
    {
        MainGameManager.Instance.ForkleftObj.transform.position
         = MainGameManager.Instance.ForkitOriPoss_End.position;

        MainGameManager.Instance.ForkleftObj.transform.localRotation
            = MainGameManager.Instance.ForkitOriPoss_End.localRotation;
    }

    public override void StateEnd()
    {
        _endPoint_Goods.isAllreadyArraivalEndPoint = false;
        _endPoint_Goods02.isAllreadyArraivalEndPoint = false;
    }


    void WheelBackPos()
    {
        MainGameManager.Instance.WheelBackPos();
    }

    void OnPractice()
    {

        //Debug.Log("_______forkliftController.CurrentMastTilt: "+ _forkliftController.CurrentMastTilt);

        //是否完成練習
        if (_startPoint_Forkleft.isOnStartPoint_Forkit
             && _startPoint_Goods.isOnStartPoint_Goods
             && _endPoint_Goods.isAllreadyArraivalEndPoint
             && _endPoint_Goods02.isAllreadyArraivalEndPoint //貨架上的貨物
             && _wSMVehicleController.CurrentHandbrake == 1
             && _wSMVehicleController.CurrentBackFront == 0
             && _forkliftController.CurrentForksVertical <= 0.02f//高度
             && _forkliftController.CurrentMastTilt > 0.59f)//傾斜
        {
            Debug.Log("[完成練習]=================");
            m_Conrtoller.SetState(MainGameStateControl.GameFlowState.CompletePrictice_stageThree, m_Conrtoller);
            StageThreeGameManager.Instance.IsSussuesPassTest = 1;
        }
        else
        {
            //練習模式可以一直練不會失敗
            StageThreeGameManager.Instance.IsSussuesPassTest = 0;
        }

        //壓線出UI
        if (StageThreeGameManager.Instance.IsForkitOnRoad == false)
        {
            if (WarningUI == null) WarningUI = GameObject.Instantiate(StageThreeGameManager.Instance.WarningUIs, StageThreeGameManager.Instance.ForkitCanvasPoss.transform);
            WarningUI.GetComponentInChildren<Text>().text = "超出車道範圍，請採煞車並按下確認按鈕回到出發點";

           
        }
        if (WarningUI != null)
        {
            //歸位
            if (logtichControl.CheckEnterUI || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.O))
            {
                if (!_startPoint_Forkleft.isOnStartPoint_Forkit) BackToOri();
                //貨物回原位
                foreach (var gl in GoodsList)
                {
                    gl.transform.position = GoodsOriPosList[GoodsList.IndexOf(gl)];
                    gl.transform.eulerAngles = GoodsOriRotList[GoodsList.IndexOf(gl)];
                }

            }
        }

        if (StageThreeGameManager.Instance.IsForkitOnRoad == true)
        {
            if (WarningUI != null) GameObject.Destroy(WarningUI);
        }

        //Debug.Log("==========" + stepIsAllOK
        //    + " " + _endPoint.isAllreadyArraivalEndPoint
        //    + " " + _wSMVehicleController.CurrentHandbrake
        //    + " " + _wSMVehicleController.CurrentBackFront);

        ////在倒車點要做到指定動作
        //if (_endPoint.isOnEndPoint_Forkit
        //    && _wSMVehicleController.CurrentHandbrake == 1
        //    && _wSMVehicleController.CurrentBackFront == 0
        //    && _forkliftController.CurrentForksVertical <= 0.02f) //高度
        //{
        //    //第一步驟
        //    stepOne_拉起手煞 = true;
        //    stepOne_前後檔回歸 = true;
        //    stepOne_升降拉桿放回 = true;

        //}
        ////第二步驟
        //if (stepOne_拉起手煞 && stepOne_前後檔回歸 && stepOne_升降拉桿放回)
        //{
        //    if (_endPoint.isAllreadyArraivalEndPoint
        //    && _wSMVehicleController.CurrentHandbrake == 0//放開剎車
        //    && _wSMVehicleController.CurrentBackFront == -1 //倒車
        //    && _forkliftController.CurrentForksVertical > 0.02f) //高度
        //    {
        //        stepIsAllOK = true;
        //    }
        //}
        ////是否彈出警告
        //if (stepIsAllOK == false
        //    && _endPoint.isAllreadyArraivalEndPoint
        //    && _wSMVehicleController.CurrentHandbrake == 0//放開剎車
        //    && _wSMVehicleController.CurrentBackFront == -1) //倒車 
        //{
        //    MainGameManager.Instance.stepIsWrong_倒車 = true;

        //    if (WarningUI_退後 == null)
        //    {
        //        WarningUI_退後 = GameObject.Instantiate(MainGameManager.Instance.WarningUIs, MainGameManager.Instance.ForkitCanvasPoss.transform);
        //        WarningUI_退後.GetComponent<WarningUIAudio>().AS.clip = WarningUI_退後.GetComponent<WarningUIAudio>().BackGoTipAudioClip;
        //        WarningUI_退後.GetComponent<WarningUIAudio>().AS.Stop();
        //        WarningUI_退後.GetComponent<WarningUIAudio>().AS.Play();
        //    }
        //    WarningUI_退後.GetComponentInChildren<Text>().text = "倒車前請完成以下步驟：\n拉起手煞 >> 前後檔回歸 >> 將升降貨插放回 >> 升降貨插上升 >> 開啟手煞 >> 打檔至退後檔 >> 開始向後開";
        //    WarningUI_退後.GetComponentInChildren<Text>().fontSize = 28;
        //    WarningUI_退後.transform.localPosition = new Vector3(0.365f, 0.098f, 0.17f);
        //    WarningUI_退後.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

        //}
        //if (stepIsAllOK)
        //{
        //    if (WarningUI_退後 != null) GameObject.Destroy(WarningUI_退後);
        //}

        //停車位置
        if (CurrentPosLimit.isInPosLimit && _wSMVehicleController.CurrentHandbrake == 1)//剎車且碰到限制區
        {
            if (WarningUI_超出格子 == null) WarningUI_超出格子 = GameObject.Instantiate(MainGameManager.Instance.WarningUIs, MainGameManager.Instance.ForkitCanvasPoss.transform);
            WarningUI_超出格子.GetComponentInChildren<Text>().text = "堆高機未停好!";
            WarningUI_超出格子.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            if (WarningUI_超出格子 != null) GameObject.Destroy(WarningUI_超出格子);

        }

    }




    void OnTest()
    {
        if (StageThreeGameManager.Instance.TotalWrongScore >= 20)
        {
            StageThreeGameManager.Instance.IsSussuesPassTest = 0;
            m_Conrtoller.SetState(MainGameStateControl.GameFlowState.CompleteTest_stageThree, m_Conrtoller);
        }

        if (_startPoint_Forkleft.isOnStartPoint_Forkit
            && _startPoint_Goods.isOnStartPoint_Goods
            && _endPoint_Goods.isAllreadyArraivalEndPoint
            && _endPoint_Goods02.isAllreadyArraivalEndPoint //貨架上的貨物
            && _wSMVehicleController.CurrentHandbrake == 1
            && _wSMVehicleController.CurrentBackFront == 0
            && _forkliftController.CurrentForksVertical <= 0.02f//高度
            && _forkliftController.CurrentMastTilt > 0.59f)//傾斜
        {
            StageThreeGameManager.Instance.IsSussuesPassTest = 1;
            m_Conrtoller.SetState(MainGameStateControl.GameFlowState.CompleteTest_stageThree, m_Conrtoller);
        }

        Debug.Log("===========isStopNow: " + isStopNow);

        //醜一暫停後
        if (isStopNow && StageThreeGameManager.Instance.IsSussuesPassTest == 2)
        {
            StopDrive();

            if (WarningUI == null) WarningUI = GameObject.Instantiate(MainGameManager.Instance.WarningUIs, MainGameManager.Instance.ForkitCanvasPoss.transform);
            WarningUI.GetComponentInChildren<Text>().text = "超出車道範圍，請採煞車並按下確認按鈕回到出發點";

            if (logtichControl.CheckEnterUI || Input.GetKey(KeyCode.Return))
            {
                //堆高機回原位
                MainGameManager.Instance.ForkleftObj.transform.position
                    = MainGameManager.Instance.ForkitOriPoss.position;

                MainGameManager.Instance.ForkleftObj.transform.localRotation
                    = MainGameManager.Instance.ForkitOriPoss.localRotation;

                //貨物回原位
                foreach(var gl in GoodsList)
                {
                    gl.transform.position = GoodsOriPosList[GoodsList.IndexOf(gl)];
                    gl.transform.eulerAngles = GoodsOriRotList[GoodsList.IndexOf(gl)];
                }

                GameObject.Destroy(WarningUI);

                StartDrive();
                isStopNow = false;

                _isCountScore_ScoreManager = true;

                //回原位
                WheelBackPos();
            }
        }
        else if (!isStopNow)
        {
        }

        //停車位置
        if (CurrentPosLimit.isInPosLimit && _wSMVehicleController.CurrentHandbrake == 1)//剎車且碰到限制區
        {
            if (WarningUI_超出格子 == null) WarningUI_超出格子 = GameObject.Instantiate(MainGameManager.Instance.WarningUIs, MainGameManager.Instance.ForkitCanvasPoss.transform);
            WarningUI_超出格子.GetComponentInChildren<Text>().text = "堆高機未停好!";
            WarningUI_超出格子.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            if (WarningUI_超出格子 != null) GameObject.Destroy(WarningUI_超出格子);

        }

        ////在倒車點要做到指定動作
        //if (_endPoint_Forkleft.isOnEndPoint_Forkit
        //    && _wSMVehicleController.CurrentHandbrake == 1
        //    && _wSMVehicleController.CurrentBackFront == 0
        //    && _forkliftController.CurrentForksVertical <= 0.02f) //高度
        //{
        //    //第一步驟
        //    stepOne_拉起手煞 = true;
        //    stepOne_前後檔回歸 = true;
        //    stepOne_升降拉桿放回 = true;

        //}
        ////第二步驟
        //if (stepOne_拉起手煞 && stepOne_前後檔回歸 && stepOne_升降拉桿放回)
        //{
        //    if (_endPoint_Forkleft.isAllreadyArraivalEndPoint
        //    && _wSMVehicleController.CurrentHandbrake == 0//放開剎車
        //    && _wSMVehicleController.CurrentBackFront == -1 //倒車
        //    && _forkliftController.CurrentForksVertical > 0.02f) //高度
        //    {
        //        Debug.Log("=====+++++++++++++++++++++++1");
        //        stepIsAllOK = true;
        //    }
        //}
        ////是否彈出警告
        //if (stepIsAllOK == false
        //    && _endPoint_Forkleft.isAllreadyArraivalEndPoint
        //    && _wSMVehicleController.CurrentHandbrake == 0//放開剎車
        //    && _wSMVehicleController.CurrentBackFront == -1) //倒車 
        //{
        //    Debug.Log("=====+++++++++++++++++++++++2");

        //    MainGameManager.Instance.stepIsWrong_倒車 = true;

        //    MainGameManager.Instance.IsSussuesPassTest = 0;
        //    m_Conrtoller.SetState(MainGameStateControl.GameFlowState.CompleteTest, m_Conrtoller);
        //}

    }

    void OnPipeFall_Test(int i)
    {
        _isCountScore_ScoreManager = false;

        if (isStopNow == false && i < 2) //醜一
        {
            isStopNow = true;
        }

    }

    void OnForkitOnLine_Test(int i)
    {
        _isCountScore_ScoreManager = false;

        if (isStopNow == false && i < 2)//醜一
        {
            isStopNow = true;
        }
    }

    void StopDrive()
    {
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().HandBrakeInput = 25;
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().StopGameBrake();
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().IsEngineOn = false;
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().enabled = false;
    }

    void StartDrive()
    {
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().HandBrakeInput = 0;
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().IsEngineOn = true;
        MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>().enabled = true;
    }
}
