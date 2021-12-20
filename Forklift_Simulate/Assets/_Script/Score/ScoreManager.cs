using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    MainGameManager mainGameManager;

    [HideInInspector]
    public int TotalWrongAmount = 0; //以5為一單位

    [HideInInspector]
    public float Timer;


    [HideInInspector]
    public int TimeScore;
    [HideInInspector]
    public int pipeFallScore;
    [HideInInspector]
    public int ForkitOnLineScore;
    [HideInInspector]
    public int StopTooLongScore;
    [HideInInspector]
    public int SpeedToHightScore;
    [HideInInspector]
    public int ForkPositionHightScore;
    [HideInInspector]
    public int ForkMastTiltScore;
    [HideInInspector]
    public int ForkHandBrakeScore;
    [HideInInspector]
    public int ForkCluthScore;
    [HideInInspector]
    public int ForkBackFrontNorStopScore;
    [HideInInspector]
    public int OnRoadNotEngineScore;


    public UnityAction<int> OnTimeScore;
    public UnityAction<int> OnPipeFallScore;
    public UnityAction<int> OnForkitOnLineScore;
    public UnityAction<int> OnStopTooLongScore;
    public UnityAction<int> OnSpeedToHightScore;
    public UnityAction<int> OnForkPositionHightScore;
    public UnityAction<int> OnForkMastTiltScore;
    public UnityAction<int> OnForkHandBrakeScore;
    public UnityAction<int> OnForkCluthScore;
    public UnityAction<int> OnForkBackFrontNorStopScore;
    public UnityAction<int> OnOnRoadNotEngineScore;


    [SerializeField]
    AudioClip[] wrongVoice;

    AudioSource AS;

    PipeDetectControl _pipeDetectControl;
    WSMGameStudio.Vehicles.WSMVehicleController _wSMVehicleController;
    WSMGameStudio.HeavyMachinery.ForkliftController _forkliftController;

    bool isTimeUp = false;
    bool isCheckOnRoadLine = false;
    bool isCheckStopTooLong = false;
    bool isCheckToHightSpeed = false;
    bool isCheckPositionHight = false;
    bool isChecMastTilt = false;
    bool isCheckForkHandBrake = false;
    //bool isCheckBrake = false;
    bool isCheckForkCluth = false;
    bool isCheckBackFront = false;
    bool isCheckOnRoadNotEngine = false;

    //private void OnGUI()
    //{
    //    GUI.color = Color.black;
    //    GUI.Box(new Rect(10, 10, 210, 250),"");
    //    GUI.color = Color.white;
    //    GUI.Label(new Rect(10, 10, 200, 100), "pipeFallScore"+ pipeFallScore);
    //    GUI.Label(new Rect(10, 30, 200, 100), "SpeedToHightScore" + SpeedToHightScore);
    //    GUI.Label(new Rect(10, 50, 200, 100), "ForkPositionHightScore" + ForkPositionHightScore);
    //    GUI.Label(new Rect(10, 70, 200, 100), "ForkMastTiltScore" + ForkMastTiltScore);
    //    GUI.Label(new Rect(10, 90, 200, 100), "ForkHandBrakeScore" + ForkHandBrakeScore);
    //    //GUI.Label(new Rect(10, 110, 200, 100), "ForkBrakeScore" + ForkBrakeScore);
    //    GUI.Label(new Rect(10, 110, 200, 100), "ForkCluthScore" + ForkCluthScore);
    //    GUI.Label(new Rect(10, 130, 200, 100), "ForkBackFrontNorStopScore" + ForkBackFrontNorStopScore);
    //    GUI.Label(new Rect(10, 150, 200, 100), "OnRoadNotEngineScore" + OnRoadNotEngineScore);
    //}

    //void Start()
    //{
    //    Init();
    //}

    public void Init()
    {
        mainGameManager = this.GetComponent<MainGameManager>();
        _pipeDetectControl = mainGameManager.PipeGroupObjs.GetComponent<PipeDetectControl>();
        _wSMVehicleController = mainGameManager.ForkleftObj.GetComponent<WSMGameStudio.Vehicles.WSMVehicleController>();
        _forkliftController = mainGameManager.ForkleftObj.GetComponent<WSMGameStudio.HeavyMachinery.ForkliftController>();
        AS = GetComponent<AudioSource>();

        TotalWrongAmount = 0;

        _pipeDetectControl.Init();

        TimeScore = 0;
        pipeFallScore = 0;
        ForkitOnLineScore = 0;
        SpeedToHightScore = 0;
        ForkPositionHightScore = 0;
        ForkMastTiltScore = 0;
        ForkHandBrakeScore = 0;
        ForkCluthScore = 0;
        ForkBackFrontNorStopScore = 0;
        OnRoadNotEngineScore = 0;

        isTimeUp = false;
        isCheckOnRoadLine = false;
        isCheckStopTooLong = false;
        isCheckToHightSpeed = false;
        isCheckPositionHight = false;
        isChecMastTilt = false;
        isCheckForkHandBrake = false;
        isCheckForkCluth = false;
        isCheckBackFront = false;
        isCheckOnRoadNotEngine = false;
    }

    public void ScoreUpdate()
    {
        if(mainGameManager.CurrentState == MainGameStateControl.GameFlowState.DriveForkKit)
        {
            //撞到柱子或壓線
            //超速駕駛
            //行駛時未拉高貨插
            //行駛時未傾斜貨插
            //行駛時手煞車未放
            //行駛時誤踩吋動踏板
            //行駛時突然變換前後檔
            //行駛時熄火

            CountTime(480,20);
            PipeFall(10);
            ForkitOnRoad(10);
            OnStopTooLong(2, 20);
            SpeedToHight(20,10);
            ForkPositionHight(2, 0.02f,5);
            ForkMastTiltRotate(2, 0.6f,5);
            ForkHandBrake(2,10);
            //ForkBrake(20, 10);
            ForkCluth(2, 10,10);
            ForkBackFrontNorStop(2,10);
            OnRoadNotEngine(2,10);
        }

    }

    public void ReleaseEvent()
    {
        OnTimeScore = null;
        OnPipeFallScore = null;
        OnForkitOnLineScore = null;
        OnStopTooLongScore = null;
        OnSpeedToHightScore = null;
        OnForkPositionHightScore = null;
        OnForkMastTiltScore = null;
        OnForkHandBrakeScore = null;
        OnForkCluthScore = null;
        OnForkBackFrontNorStopScore = null;
        OnOnRoadNotEngineScore = null;
    }

    /// <summary>
    /// 計時
    /// </summary>
    /// <param name="limitSec"></param>
    void CountTime(int limitSec,int score)
    {
        Timer += Time.deltaTime;
        if(Timer > limitSec && isTimeUp ==false)
        {
            isTimeUp = true;
            TimeScore = 1;
            OnTimeScore(TimeScore);
            PlayWrongVoice(wrongVoice[0]);
            TotalWrongAmount += score;
        }
    }

    int temp;
    /// <summary>
    /// 判斷柱子
    /// </summary>
    void PipeFall(int score)
    {
        temp = pipeFallScore;
        pipeFallScore = _pipeDetectControl.BeColliderTotalAmount;

        if(pipeFallScore > 0 && temp != pipeFallScore)
        {
            OnPipeFallScore(pipeFallScore);
            PlayWrongVoice(wrongVoice[1]);
            TotalWrongAmount += score;
        }
    }

    /// <summary>
    /// 壓線
    /// </summary>
    /// <param name="score"></param>
    void ForkitOnRoad(int score)
    {
        if (MainGameManager.Instance.IsForkitOnRoadOutLine && !isCheckOnRoadLine)
        {
            isCheckOnRoadLine = true;
            ForkitOnLineScore += 1;
            Debug.Log("壓線" + ForkitOnLineScore + "次");
            OnForkitOnLineScore(ForkitOnLineScore);
            //PlayWrongVoice(wrongVoice[2]);
            TotalWrongAmount += score;
        }
        else if (!MainGameManager.Instance.IsForkitOnRoadOutLine)
        {
            isCheckOnRoadLine = false;
        }
    }

    /// <summary>
    /// 原地停超過5秒
    /// </summary>
    bool isMove = false;
    float countTime = 0;
    void OnStopTooLong(int stopSpeed, int score)
    {
        //在開始點不計算
        if (!mainGameManager.StartPointObjs.GetComponent<StartPoint>().isOnStartPoint_Forkit)
        {
            if (_wSMVehicleController.CurrentSpeed >= stopSpeed)
            {
                isMove = true;
            }
            if (isMove)
            {
                if (_wSMVehicleController.CurrentSpeed <= 0.001f)
                {
                    countTime += Time.deltaTime;

                }
                if (_wSMVehicleController.CurrentSpeed <= 0.001f && !isCheckStopTooLong && countTime > 5)
                {
                    isCheckStopTooLong = true;
                    StopTooLongScore += 1;
                    Debug.Log("停留超過5秒" + StopTooLongScore + "次");
                    OnStopTooLongScore(StopTooLongScore);
                    //PlayWrongVoice(wrongVoice[2]);
                    TotalWrongAmount += score;
                }
                else if (_wSMVehicleController.CurrentSpeed > 0.001f && isCheckStopTooLong)
                {
                    countTime = 0;
                    isCheckStopTooLong = false;
                }
            }

        }

      
       
    }

    /// <summary>
    /// 超過速限後+1，如果回到規定速限以下再次超速再+1
    /// </summary>
    /// <param name="limitSpeed"></param>
    void SpeedToHight(int limitSpeed, int score)
    {
        if (_wSMVehicleController.CurrentSpeed >= limitSpeed && !isCheckToHightSpeed)
        {
            isCheckToHightSpeed = true;
            SpeedToHightScore += 1;
            Debug.Log("超過速限" + limitSpeed + "__" + SpeedToHightScore+"次");
            OnSpeedToHightScore(SpeedToHightScore);
            PlayWrongVoice(wrongVoice[2]);
            TotalWrongAmount += score;

        }
        else if(_wSMVehicleController.CurrentSpeed < limitSpeed - 5 && isCheckToHightSpeed)
        {
            isCheckToHightSpeed = false;
        }
    }

    /// <summary>
    /// 當移動速度超過speed且貨插高度位置不正確+1，當貨插上升至指定高度後，如果再次將貨插下降至指定高度以下且移動速度超過speed則再次+1
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="forkHight"></param>
    void ForkPositionHight(float speed , float forkHight, int score)
    {
        //Debug.Log("CurrentForksVertical:"+_forkliftController.CurrentForksVertical);
        if (_wSMVehicleController.CurrentSpeed > speed &&
            _forkliftController.CurrentForksVertical <= forkHight
            && !isCheckPositionHight)
        {
            isCheckPositionHight = true;
            ForkPositionHightScore += 1;
            Debug.Log(_forkliftController.CurrentForksVertical + "貨插位置錯誤_高度" + ForkPositionHightScore + "次");
            OnForkPositionHightScore(ForkPositionHightScore);
            PlayWrongVoice(wrongVoice[3]);
            TotalWrongAmount += score;

        }
        else if (_forkliftController.CurrentForksVertical > forkHight && isCheckPositionHight)
        {
            isCheckPositionHight = false;
        }
    }

    /// <summary>
    /// 當移動速度超過speed且貨插高度傾斜角度不正確+1，當貨插傾斜至指定角度後，如果再次將貨插傾斜至指定角度以外且移動速度超過speed則再次+1
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="tiltRotate"></param>
    void ForkMastTiltRotate(float speed, float tiltRotate, int score)
    {
        //Debug.Log("_CurrentMastTilt" + _forkliftController.CurrentMastTilt);

        if (_wSMVehicleController.CurrentSpeed > speed &&
            _forkliftController.CurrentMastTilt > tiltRotate
            && !isChecMastTilt)
        {
            isChecMastTilt = true;
            ForkMastTiltScore += 1;
            Debug.Log(_forkliftController.CurrentMastTilt + "貨插位置錯誤_傾斜角度" + ForkMastTiltScore + "次");
            OnForkMastTiltScore(ForkMastTiltScore);
            PlayWrongVoice(wrongVoice[4]);
            TotalWrongAmount += score;

        }
        else if (_forkliftController.CurrentMastTilt <= tiltRotate && isChecMastTilt)
        {
            isChecMastTilt = false;
        }
    }

    /// <summary>
    /// 當移動速度超過speed且手剎車拉起則+1，將手煞車放下後，如果再次手剎車拉起且移動速度超過speed則再次+1
    /// </summary>
    /// <param name="speed"></param>
    void ForkHandBrake(float speed, int score)
    {
        //Debug.Log("=======_wSMVehicleController.CurrentSpeed " + _wSMVehicleController.CurrentSpeed);
        if (_wSMVehicleController.CurrentHandbrake == 1 &&
            _wSMVehicleController.CurrentSpeed > speed&&
            !isCheckForkHandBrake)
        {
            ForkHandBrakeScore += 1;
            isCheckForkHandBrake = true;
            OnForkHandBrakeScore(ForkHandBrakeScore);
            PlayWrongVoice(wrongVoice[5]);
            TotalWrongAmount += score;

        }
        else if (_wSMVehicleController.CurrentHandbrake == 0 && isCheckForkHandBrake)
        {
            isCheckForkHandBrake = false;
        }

    }


    //void ForkBrake(float speed,int brake)
    //{
    //    if (_wSMVehicleController.CurrentBrakes >= brake &&
    //        _wSMVehicleController.CurrentSpeed > speed &&
    //        !isCheckBrake)
    //    {
    //        ForkBrakeScore += 1;
    //        isCheckBrake = true;
    //    }
    //    else if (_wSMVehicleController.CurrentBrakes < brake && isCheckBrake)
    //    {
    //        isCheckBrake = false;
    //    }
    //}

    /// <summary>
    /// 當移動速度超過speed且壓下吋動踏板則+1，將吋動踏板釋放，如果再次壓下吋動踏板且移動速度超過speed則再次+1
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="clutch"></param>
    void ForkCluth(float speed, int clutch, int score)
    {
        if (_wSMVehicleController.CurrentClutch >= clutch &&
          _wSMVehicleController.CurrentSpeed > speed &&
          !isCheckForkCluth)
        {
            ForkCluthScore += 1;
            isCheckForkCluth = true;
            OnForkCluthScore(ForkCluthScore);
            PlayWrongVoice(wrongVoice[6]);
            TotalWrongAmount += score;

        }
        else if (_wSMVehicleController.CurrentClutch < clutch && isCheckForkCluth)
        {
            isCheckForkCluth = false;
        }
    }

    /// <summary>
    /// 當移動速度超過speed且切換前/後/空檔則+1
    /// </summary>
    /// <param name="speed"></param>
    float lastFrameFrontBack = 0;
    void ForkBackFrontNorStop(float speed, int score)
    {
        if (_wSMVehicleController.CurrentBackFront != lastFrameFrontBack &&
        _wSMVehicleController.CurrentSpeed > speed &&
        !isCheckBackFront)
        {
            ForkBackFrontNorStopScore += 1;
            isCheckBackFront = true;
            OnForkBackFrontNorStopScore(ForkBackFrontNorStopScore);
            PlayWrongVoice(wrongVoice[7]);
            TotalWrongAmount += score;

        }
        else if (_wSMVehicleController.CurrentBackFront == lastFrameFrontBack && isCheckBackFront)
        {
            isCheckBackFront = false;
        }

        lastFrameFrontBack = _wSMVehicleController.CurrentBackFront;
    }

    /// <summary>
    /// 當移動速度超過speed且引擎熄火則+1，將引擎開啟後，如果再次引擎熄火且移動速度超過speed則再次+1
    /// </summary>
    /// <param name="speed"></param>
    void OnRoadNotEngine(float speed, int score)
    {
        if (_wSMVehicleController.CurrentEngineOn == false &&
        _wSMVehicleController.CurrentSpeed > speed &&
        !isCheckOnRoadNotEngine)
        {
            OnRoadNotEngineScore += 1;
            isCheckOnRoadNotEngine = true;
            OnOnRoadNotEngineScore(OnRoadNotEngineScore);
            PlayWrongVoice(wrongVoice[8]);
            TotalWrongAmount += score;

        }
        else if (_wSMVehicleController.CurrentEngineOn == true && isCheckOnRoadNotEngine)
        {
            isCheckOnRoadNotEngine = false;
        }
    }


    void PlayWrongVoice(AudioClip clip)
    {
        AS.clip = clip;
        AS.Stop();
        AS.Play();
    }
}
