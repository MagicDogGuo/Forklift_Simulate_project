using System;
using UnityEngine;

public class GetAllJoysEvent : MonoBehaviour
{

    [HideInInspector]
    public static bool HandBrake_btn4;
    [HideInInspector]
    public static bool FrontBar_btn5;
    [HideInInspector]
    public static bool FrontBar_btn6;
    [HideInInspector]
    public static float UpDownBar_Ry_升降;
    [HideInInspector]
    public static float DegreeBar_Rz_傾斜;

    private string currentButton, currentButton1;
 

    void Update()
    {
        var values = Enum.GetValues(typeof(KeyCode));
        //var spanAxis = Enum.GetValues(typeof(SnapAxis));



        //存储所有的按键 
        for (int x = 0; x < values.Length; x++)
        {
            if (Input.GetKeyDown((KeyCode)values.GetValue(x)))
            {
                currentButton = values.GetValue(x).ToString();
                Debug.Log("Current Button : " + currentButton);

            }
        }




        //手煞、前進、退後
        HandBrake_btn4 = false;
        FrontBar_btn5 = false;
        FrontBar_btn6 = false;

        if (Input.GetKey(KeyCode.JoystickButton4))//從Button0開始算
        {
            HandBrake_btn4 = true;
            Debug.Log("按下按钮4_手煞");
        }

        if (Input.GetKey(KeyCode.JoystickButton5))//從Button0開始算
        {
            FrontBar_btn5 = true;
            Debug.Log("按下按钮5_前進");
        }


        if (Input.GetKey(KeyCode.JoystickButton6))//從Button0開始算
        {
            FrontBar_btn6 = true;
            Debug.Log("按下按钮6_後退");
        }

        //升降、傾斜拉桿
        UpDownBar_Ry_升降 = Input.GetAxis("Handbar_inside");
        DegreeBar_Rz_傾斜 = Input.GetAxis("Handbar_outside");



        //Debug.Log("Horizontal:" + Input.GetAxis("Horizontal"));//方向盤
        //Debug.Log("Vertical:" + Input.GetAxis("Vertical"));//吋動踏板
        //Debug.Log("Handbar_inside:" + Input.GetAxis("Handbar_inside"));//升降
        //Debug.Log("Handbar_outside:" + Input.GetAxis("Handbar_outside"));//傾斜


    }
}
