﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;


//VR頭盔固定在要駕駛的高度與位置(現實世界)，頭盔需水平放置，此時VR畫面看的位置會是錯誤的，且堆高機需在初始位置
//接下來按下L來重新定位VR的視野，此時畫面就會到正確的駕駛位置

public class ResetVRPosition : MonoBehaviour
{
    [SerializeField]
    GameObject VREyeObj;
    [SerializeField]
    GameObject SteamVRObj;
    [SerializeField]
    GameObject VRTrackerObj_L;
    [SerializeField]
    GameObject VRTrackerObj_R;
    [SerializeField]
    GameObject ResetRotateObj;




    //[SerializeField]
    public static Vector3 OriDriveForkleftPos =  new Vector3(-0.08f, 2.1f, -0.3f);//new Vector3(-0.33f,1.74f,-0.1f);
    [SerializeField]
    Vector3 OriDriveForkleftRot = new Vector3(0f, 0f, 0f);


    static Vector3 VRLookPos;
    static Vector3 VRLookRot;


    static Vector3 SteamObjOriPos;
    static Vector3 SteamObjOriRot;


    [SerializeField]
    InputField SetX;
    [SerializeField]
    InputField SetY;
    [SerializeField]
    InputField SetZ;
    [SerializeField]
    Text RealWorldPos;

    GameObject VRTK_SDKManagerObj;


    //VR堆高機的數據
    float A_地板至踏板 = 80;
    float B_VR原點至方向盤前緣 = 10.5f;



    void Start()
    {
      
        
        VRTK_SDKManagerObj = GameObject.Find("[VRTK_SDKManager]");

        SetX.text = ResetVRPosition.OriDriveForkleftPos.x + "";
        SetY.text = ResetVRPosition.OriDriveForkleftPos.y + "";
        SetZ.text = ResetVRPosition.OriDriveForkleftPos.z + "";


        //己經有校正過
        if (VRLookPos != Vector3.zero && VRLookRot != Vector3.zero)
        {
            StartCoroutine(DalayMove());
        }
        else
        {
            SteamObjOriPos = new Vector3(SteamVRObj.transform.position.x, SteamVRObj.transform.position.y, SteamVRObj.transform.position.z);
            SteamObjOriRot = new Vector3(SteamVRObj.transform.eulerAngles.x, SteamVRObj.transform.eulerAngles.y, SteamVRObj.transform.eulerAngles.z);
        }

    }

    IEnumerator DalayMove()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        //讓ResetRotateObj的位置與VR EYE相同
        ResetRotateObj.transform.position = VREyeObj.transform.position;
        yield return new WaitForEndOfFrame();

        //把VR EYE關掉VR功能並設定成ResetRotateObj的子物件(如此VR EYE會在(0,0,0))
        VREyeObj.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();

        //VR EYE設定成ResetRotateObj的子物件(如此VR EYE會在(0,0,0))
        VREyeObj.transform.SetParent(ResetRotateObj.transform);
        yield return new WaitForEndOfFrame();

        //旋轉SteamVRObj到校正旋轉角度
        //把VR EYE設定成SteamVRObj的子物件       
        VRLookRot = VREyeObj.transform.eulerAngles;
        Vector3 moveRot = SteamObjOriRot - VRLookRot + OriDriveForkleftRot;//rotate會有問題因為Eye是子物件且沒有在(0,0,0)  SteamVRObj.transform.eulerAngles
        SteamVRObj.transform.eulerAngles += new Vector3(SteamObjOriRot.x, moveRot.y, SteamObjOriRot.z);//只轉Y就好 所以要水平放置
        VREyeObj.transform.SetParent(SteamVRObj.transform);

        VRTrackerObj_L.transform.SetParent(SteamVRObj.transform);
        VRTrackerObj_R.transform.SetParent(SteamVRObj.transform);

        yield return new WaitForEndOfFrame();

        //開啟VR功能
        VREyeObj.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();

        //最後把SteamVRObj移動至校正位置
        VRLookPos = VREyeObj.transform.position;
        Vector3 movePos = SteamObjOriPos - VRLookPos + OriDriveForkleftPos;
        SteamVRObj.transform.position += movePos;
        Debug.Log("===================" + VREyeObj.transform.position);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            SetX.transform.parent.gameObject.SetActive(!SetX.transform.parent.gameObject.active);
            //關閉VR模式
            //XRSettings.enabled = false;

        }
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    //開啟VR模式
        //    XRSettings.enabled = true;
        //}
        ResetVRPosition.OriDriveForkleftPos =
                 new Vector3(float.Parse(SetX.text), float.Parse(SetY.text), float.Parse(SetZ.text));
        float realworldX = 0;
        float realworldY = float.Parse(SetY.text)*100 - A_地板至踏板;
        float realworldZ =  B_VR原點至方向盤前緣- float.Parse(SetZ.text)*100;
        float realworldX_1_19 = 0;
        float realworldY_1_19 = realworldY/1.19f;
        float realworldZ_1_19 = realworldZ / 1.19f;
        RealWorldPos.text = "真實世界VR頭盔:\n高度Y:" + realworldY + "(" + realworldY_1_19 + "),"
              + "  距離方向盤前緣Z:" + realworldZ + "(" + realworldZ_1_19 + "),"
              +"   X軸對準方向盤中心";

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(DalayMove());
        }
    }
}
