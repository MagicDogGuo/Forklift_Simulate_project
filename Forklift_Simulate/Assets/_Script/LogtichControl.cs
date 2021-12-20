using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogtichControl : MonoBehaviour
{
    [HideInInspector]
    public float LogitchSteelRotation;
    [HideInInspector]
    public float LogitchGasRotation;
    [HideInInspector]
    public float LogitchBreakRotation;
    [HideInInspector]
    public float LogitchCluthRotation;

    [HideInInspector]
    public bool ForkUp;
    [HideInInspector]
    public bool ForkDown;
    [HideInInspector]
    public bool MastTiltForwards;
    [HideInInspector]
    public bool MastTiltBackwards;

    [HideInInspector]
    public bool FrontMove;
    [HideInInspector]
    public bool BackMove;


    private string actualState;
    private void Awake()
    {
        LogitchGasRotation = 0;
        LogitchBreakRotation = 0;
        LogitchCluthRotation = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        LogitchGasRotation = 0;
        LogitchBreakRotation = 0;
        LogitchCluthRotation = 0;

        LogitechGSDK.LogiSteeringInitialize(false);
        Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            actualState = "Steering wheel current state : \n\n";
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);
            actualState += "x-axis position :" + rec.lX + "\n";

            float actualState_450 = rec.lX * 0.01373f;
            //Debug.Log("actualState_450:" + actualState_450);

            LogitchSteelRotation = actualState_450;

            LogitchGasRotation = MapValue(32767, -32767, 0, 25, rec.lY);
            LogitchBreakRotation = MapValue(32767, -32767, 0, 25, rec.lRz);
            LogitchCluthRotation = MapValue(32767, -32767, 0, 25, rec.rglSlider[0]);

            //Debug.Log("LogitchCluthRotation" + LogitchCluthRotation);

            //Button status :
            string buttonStatus = "Button pressed : ";
            for (int i = 0; i < 128; i++)
            {
                if (rec.rgbButtons[i] == 128)
                {
                    buttonStatus += "Button " + i + " pressed\n";
                    Debug.Log(" buttonStatus: " + buttonStatus);
                }
                if (rec.rgbButtons[3] == 128) ForkUp = true;
                else if (rec.rgbButtons[3] != 128) ForkUp = false;

                if (rec.rgbButtons[0] == 128) ForkDown = true;
                else if (rec.rgbButtons[0] != 128) ForkDown = false;

                if (rec.rgbButtons[1] == 128) MastTiltForwards = true;
                else if (rec.rgbButtons[1] != 128) MastTiltForwards = false;

                if (rec.rgbButtons[2] == 128) MastTiltBackwards = true;
                else if (rec.rgbButtons[2] != 128) MastTiltBackwards = false;


                if (rec.rgbButtons[14] == 128) FrontMove = true;
                else if (rec.rgbButtons[14] != 128) FrontMove = false;


                if (rec.rgbButtons[15] == 128) BackMove = true;
                else if (rec.rgbButtons[15] != 128) BackMove = false;
            }

        }


        //Spring Force -> S
        if (Input.GetKeyUp(KeyCode.S))
        {
            //if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SPRING))
            //{
            //    LogitechGSDK.LogiStopSpringForce(0);
            //    Debug.Log("A");
            //}
            //else
            //{
            //    LogitechGSDK.LogiPlaySpringForce(0, 0, 30, 30);
            //    Debug.Log("B");

            //}

            if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_DIRT_ROAD))
            {
                LogitechGSDK.LogiStopDirtRoadEffect(0);
                LogitechGSDK.LogiStopDamperForce(0);
            }
            else
            {
                LogitechGSDK.LogiPlayDamperForce(0, 45);
                LogitechGSDK.LogiPlayDirtRoadEffect(0, 30);
            }
        }
    }

    public float MapValue(float a0, float a1, float b0, float b1, float a)
    {
        return b0 + (b1 - b0) * ((a - a0) / (a1 - a0));
    }
}
