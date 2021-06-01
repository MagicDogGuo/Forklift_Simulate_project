using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    MainGameManager mainGameManager;

    int TimeScore;
    int pipeFallScore;
    int SpeedToHightScore;
    int ForkPositionHightScore;
    int ForkMastTiltScore;
    int ForkHandBrakeScore;
    //int ForkBrakeScore; //腳煞
    int ForkCluthScore;
    int ForkBackFrontNorStopScore;
    int OnRoadNotEngineScore;

    PipeDetectControl _pipeDetectControl;
    WSMGameStudio.Vehicles.WSMVehicleController _wSMVehicleController;
    WSMGameStudio.HeavyMachinery.ForkliftController _forkliftController;

    bool isCheckToHightSpeed = false;
    bool isCheckPositionHight = false;
    bool isChecMastTilt = false;
    bool isCheckForkHandBrake = false;
    //bool isCheckBrake = false;
    bool isCheckForkCluth = false;
    bool isCheckBackFront = false;
    bool isCheckOnRoadNotEngine = false;

    private void OnGUI()
    {
        GUI.color = Color.black;
        GUI.Box(new Rect(10, 10, 210, 250),"");
        GUI.color = Color.white;
        GUI.Label(new Rect(10, 10, 200, 100), "pipeFallScore"+ pipeFallScore);
        GUI.Label(new Rect(10, 30, 200, 100), "SpeedToHightScore" + SpeedToHightScore);
        GUI.Label(new Rect(10, 50, 200, 100), "ForkPositionHightScore" + ForkPositionHightScore);
        GUI.Label(new Rect(10, 70, 200, 100), "ForkMastTiltScore" + ForkMastTiltScore);
        GUI.Label(new Rect(10, 90, 200, 100), "ForkHandBrakeScore" + ForkHandBrakeScore);
        //GUI.Label(new Rect(10, 110, 200, 100), "ForkBrakeScore" + ForkBrakeScore);
        GUI.Label(new Rect(10, 110, 200, 100), "ForkCluthScore" + ForkCluthScore);
        GUI.Label(new Rect(10, 130, 200, 100), "ForkBackFrontNorStopScore" + ForkBackFrontNorStopScore);
        GUI.Label(new Rect(10, 150, 200, 100), "OnRoadNotEngineScore" + OnRoadNotEngineScore);


    }

    void Start()
    {
        mainGameManager = this.GetComponent<MainGameManager>();
        _pipeDetectControl = mainGameManager.PipeGroupObj.GetComponent<PipeDetectControl>();
        _wSMVehicleController = mainGameManager.ForkleftObj.GetComponent<WSMGameStudio.Vehicles.WSMVehicleController>();
        _forkliftController = mainGameManager.ForkleftObj.GetComponent<WSMGameStudio.HeavyMachinery.ForkliftController>();
    }

    void Update()
    {
        if(mainGameManager.CurrentState == MainGameStateControl.GameFlowState.DriveForkKit)
        {
            PipeFall();
            SpeedToHight(20);
            ForkPositionHight(3, .2f);
            ForkMastTiltRotate(3, 0.25f);
            ForkHandBrake(20);
            //ForkBrake(20, 10);
            ForkCluth(20, 10);
            ForkBackFrontNorStop(3);
            OnRoadNotEngine(3);
        }

    }

    void PipeFall()
    {
        pipeFallScore = _pipeDetectControl.BeColliderTotalAmount;

    }

    /// <summary>
    /// 超過速限後+1，如果回到規定速限以下再次超速再+1
    /// </summary>
    /// <param name="limitSpeed"></param>
    void SpeedToHight(int limitSpeed)
    {
        if (_wSMVehicleController.CurrentSpeed >= limitSpeed && !isCheckToHightSpeed)
        {
            isCheckToHightSpeed = true;
            SpeedToHightScore += 1;
            Debug.Log("超過速限" + limitSpeed + "__" + SpeedToHightScore+"次");
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
    void ForkPositionHight(float speed , float forkHight)
    {
        if (_wSMVehicleController.CurrentSpeed > speed &&
            _forkliftController.CurrentForksVertical <= forkHight
            && !isCheckPositionHight)
        {
            isCheckPositionHight = true;
            ForkPositionHightScore += 1;
            Debug.Log("貨插位置錯誤_高度" + ForkPositionHightScore + "次");
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
    void ForkMastTiltRotate(float speed, float tiltRotate)
    {
        Debug.Log("_CurrentMastTilt" + _forkliftController.CurrentMastTilt);

        if (_wSMVehicleController.CurrentSpeed > speed &&
            _forkliftController.CurrentMastTilt > tiltRotate
            && !isChecMastTilt)
        {
            isChecMastTilt = true;
            ForkMastTiltScore += 1;
            Debug.Log("貨插位置錯誤_傾斜角度" + ForkMastTiltScore + "次");
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
    void ForkHandBrake(float speed)
    {
        if(_wSMVehicleController.CurrentHandbrake == 1 &&
            _wSMVehicleController.CurrentSpeed > speed&&
            !isCheckForkHandBrake)
        {
            ForkHandBrakeScore += 1;
            isCheckForkHandBrake = true;

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
    void ForkCluth(float speed, int clutch)
    {
        if (_wSMVehicleController.CurrentClutch >= clutch &&
          _wSMVehicleController.CurrentSpeed > speed &&
          !isCheckForkCluth)
        {
            ForkCluthScore += 1;
            isCheckForkCluth = true;
        }
        else if (_wSMVehicleController.CurrentBrakes < clutch && isCheckForkCluth)
        {
            isCheckForkCluth = false;
        }
    }

    /// <summary>
    /// 當移動速度超過speed且切換前/後/空檔則+1
    /// </summary>
    /// <param name="speed"></param>
    float lastFrameFrontBack = 0;
    void ForkBackFrontNorStop(float speed)
    {
        if (_wSMVehicleController.CurrentBackFront != lastFrameFrontBack &&
        _wSMVehicleController.CurrentSpeed > speed &&
        !isCheckBackFront)
        {
            ForkBackFrontNorStopScore += 1;
            isCheckBackFront = true;

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
    void OnRoadNotEngine(float speed)
    {
        if (_wSMVehicleController.CurrentEngineOn == false &&
        _wSMVehicleController.CurrentSpeed > speed &&
        !isCheckOnRoadNotEngine)
        {
            OnRoadNotEngineScore += 1;
            isCheckOnRoadNotEngine = true;
        }
        else if (_wSMVehicleController.CurrentEngineOn == true && isCheckOnRoadNotEngine)
        {
            isCheckOnRoadNotEngine = false;
        }
    }
}
