using System;
using UnityEngine;
//https://cloud.tencent.com/developer/article/1837239
public class JoyCtrl : MonoBehaviour
{
    private string currentButton, currentButton1;

    //当前按下的按键 
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame 
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
               // Debug.Log("Current Button : " + currentButton);

            }
        }

        //Debug.Log("Horizontal:" + Input.GetAxis("Horizontal"));
        //Debug.Log("Vertical:" + Input.GetAxis("Vertical"));
        //Debug.Log("Handbar_inside:" + Input.GetAxis("Handbar_inside"));
        //Debug.Log("Handbar_outside:" + Input.GetAxis("Handbar_outside"));

        


        for (int i = 3; i < 29; i++)
        {
            if (i < 10)
            {
                //Debug.Log("axis" + i + ":" + Input.GetAxis("Axis0" + i));
            }
            else
            {
                //Debug.Log("axis" + i + ":" + Input.GetAxis("Axis" + i));
            }
            if (i == 6)
            {
                //////Debug.Log("axis" + i + ":" + Input.GetAxis("Axis0" + i));
            }
            if (i == 5)
            {
                /////Debug.Log("axis" + i + ":" + Input.GetAxis("Axis0" + i));
            }
        }


        //Debug.Log("Vertical_Joy01:" + Input.GetAxis("Vertical_Joy01"));
        //Debug.Log("Horizontal_Joy01:" + Input.GetAxis("Horizontal_Joy01"));


        //Debug.Log("Vertical_Joy02:" + Input.GetAxis("Vertical_Joy02"));
        //Debug.Log("Horizontal_Joy02" + Input.GetAxis("Horizontal_Joy02"));


        if (Input.GetKey(KeyCode.Joystick1Button6))//從Button0開始算
        {
            Debug.Log("按下按钮7");
        }   

    }
    // Show some data 
    void OnGUI()
    {
        GUI.TextArea(new Rect(100,100 , 0,0 ), "Current Button : " + currentButton);
    }
}