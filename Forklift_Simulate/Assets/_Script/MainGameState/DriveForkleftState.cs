﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.Vehicles;
using WSMGameStudio.HeavyMachinery;
using UnityEngine.UI;

public class DriveForkleftState : IMainGameState
{
    public DriveForkleftState(MainGameStateControl Controller) : base(Controller)
    {
        this.StateName = "CompleteState";
    }

    float delayScoreCount=0;
    MainGameManager.GameMode _gameMode;
    StartPoint _startPoint;
    EndPoint _endPoint;
    GameObject _ScoreGroupCanvas;
    WSMVehicleController _wSMVehicleController;
    ForkliftController _forkliftController;
    GameObject WarningUI;
    ScoreManager _scoreManager;

    bool _isCountScore_ScoreManager = false;
    bool isStopNow = false;

    LogtichControl logtichControl;
    public override void StateBegin()
    {   //wheel歸位
        WheelBackPos();

        delayScoreCount = 0;
        MainGameManager.Instance.CreateForkkit();
        MainGameManager.Instance.ScoreManagers.Init();
        _isCountScore_ScoreManager = true;
        isStopNow = false;
        MainGameManager.Instance.IsForkitOnRoadOutLineObj.Init();

        _wSMVehicleController = MainGameManager.Instance.ForkleftObj.GetComponent<WSMVehicleController>();
        _forkliftController = MainGameManager.Instance.ForkleftObj.GetComponent<ForkliftController>();
        _gameMode = MainGameManager.Instance.GameModes;
        _scoreManager = MainGameManager.Instance.ScoreManagers;

        _wSMVehicleController.enabled = true;

        

        if (_gameMode == MainGameManager.GameMode.PracticeMode)
        {
            _ScoreGroupCanvas = GameObject.Instantiate(MainGameManager.Instance.ScoreGroupCanvass,
                                                        MainGameManager.Instance.ForkitCanvasPoss.transform);
        }
        else if (_gameMode == MainGameManager.GameMode.TestMode)
        {
            _ScoreGroupCanvas = GameObject.Instantiate(MainGameManager.Instance.ScoreGroupCanvass,
                                                        MainGameManager.Instance.ForkitCanvasPoss.transform);

            //判斷回原位(撞柱子、壓線)
            _scoreManager.OnPipeFallScore += OnPipeFall_Test;
            _scoreManager.OnForkitOnLineScore += OnForkitOnLine_Test;
        }


        _startPoint = MainGameManager.Instance.StartPointObjs.GetComponent<StartPoint>();
        _endPoint = MainGameManager.Instance.EndPointObjs.GetComponent<EndPoint>();


        logtichControl = GameObject.FindObjectOfType<LogtichControl>();


        //測驗中
        MainGameManager.Instance.IsSussuesPassTest = 2;

    }
    public override void StateUpdate()
    {
        string t = "_startPoint.isOnStartPoint_Forkit: " + _startPoint.isOnStartPoint_Forkit
          + "\n _endPoint.isAllreadyArraivalEndPoint: " + _endPoint.isAllreadyArraivalEndPoint
          + "\n _wSMVehicleController.CurrentHandbrake == 1: " + (_wSMVehicleController.CurrentHandbrake == 1)
          + "\n _wSMVehicleController.CurrentBackFront == 0: " + (_wSMVehicleController.CurrentBackFront == 0)
          + "\n _forkliftController.CurrentForksVertical <= 0.02f: " + (_forkliftController.CurrentForksVertical <= 0.02f)
          + "\n _forkliftController.CurrentMastTilt > 0.6f: " + (_forkliftController.CurrentMastTilt > 0.6f);
        MainGameManager.Instance.TestText.GetComponent<Text>().text = t;

        //延遲出現分數版
        delayScoreCount += Time.deltaTime;
        if ((int)delayScoreCount >= 1 && _isCountScore_ScoreManager)
        {
            MainGameManager.Instance.ScoreManagers.ScoreUpdate();

            //MainGameManager.Instance.ScoreManagers.enabled = true;
        }


        //Debug.Log("=====IsSussuesPassTest:" + MainGameManager.Instance.IsSussuesPassTest);

        if (_gameMode == MainGameManager.GameMode.PracticeMode)
        {
            OnPractice();
        }
        else if (_gameMode == MainGameManager.GameMode.TestMode)
        {
            OnTest();
        }

        //隨時歸位
        if ( logtichControl.CheckEnterUI || Input.GetKey(KeyCode.Return))
        {
            BackToOri();
        }
    }

    void BackToOri()
    {
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

    public override void StateEnd()
    {

    }


    void WheelBackPos()
    {
        MainGameManager.Instance.WheelBackPos();
    }

    void OnPractice()
    {
   
        //Debug.Log("_______forkliftController.CurrentMastTilt: "+ _forkliftController.CurrentMastTilt);
        if (_startPoint.isOnStartPoint_Forkit
             && _endPoint.isAllreadyArraivalEndPoint
             && _wSMVehicleController.CurrentHandbrake == 1
             && _wSMVehicleController.CurrentBackFront == 0
             && _forkliftController.CurrentForksVertical <= 0.02f//高度
             && _forkliftController.CurrentMastTilt > 0.6f)//傾斜
        {
            m_Conrtoller.SetState(MainGameStateControl.GameFlowState.CompletePrictice, m_Conrtoller);
            MainGameManager.Instance.IsSussuesPassTest = 1;
        }
        else
        {
            //練習模式可以一直練不會失敗
            //MainGameManager.Instance.IsSussuesPassTest = 0;
        }

        //壓線出UI
        if (MainGameManager.Instance.IsForkitOnRoad == false)
        {
            if (WarningUI == null) WarningUI = GameObject.Instantiate(MainGameManager.Instance.WarningUIs, MainGameManager.Instance.ForkitCanvasPoss.transform);
            WarningUI.GetComponentInChildren<Text>().text = "超出車道範圍，請採煞車並按下確認按鈕回到出發點";

        }


        if (MainGameManager.Instance.IsForkitOnRoad == true)
        {
            if (WarningUI != null) GameObject.Destroy(WarningUI);
        }

    }

    void OnTest()
    {
        if (MainGameManager.Instance.TotalWrongScore >= 20)
        {
            MainGameManager.Instance.IsSussuesPassTest = 0;
            m_Conrtoller.SetState(MainGameStateControl.GameFlowState.CompleteTest, m_Conrtoller);
        }

        if (_startPoint.isOnStartPoint_Forkit 
            && _endPoint.isAllreadyArraivalEndPoint
            && _wSMVehicleController.CurrentHandbrake == 1
            && _wSMVehicleController.CurrentBackFront == 0
            && _forkliftController.CurrentForksVertical <= 0.02f//高度
            && _forkliftController.CurrentMastTilt > 0.6f)//傾斜
        {
            MainGameManager.Instance.IsSussuesPassTest = 1;
            m_Conrtoller.SetState(MainGameStateControl.GameFlowState.CompleteTest, m_Conrtoller);
        }

        Debug.Log("===========isStopNow: " + isStopNow);

        //醜一暫停後
        if (isStopNow && MainGameManager.Instance.IsSussuesPassTest == 2)
        {
            StopDrive();

            if (WarningUI == null) WarningUI = GameObject.Instantiate(MainGameManager.Instance.WarningUIs, MainGameManager.Instance.ForkitCanvasPoss.transform);
            WarningUI.GetComponentInChildren<Text>().text = "超出車道範圍，請採煞車並按下確認按鈕回到出發點";

            if (logtichControl.CheckEnterUI || Input.GetKey(KeyCode.Return))
            {
                MainGameManager.Instance.ForkleftObj.transform.position
                    = MainGameManager.Instance.ForkitOriPoss.position;

                MainGameManager.Instance.ForkleftObj.transform.localRotation
                    = MainGameManager.Instance.ForkitOriPoss.localRotation;

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

    }

    void OnPipeFall_Test(int i)
    {
        _isCountScore_ScoreManager = false;

        if (isStopNow == false && i < 2 ) //醜一
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
