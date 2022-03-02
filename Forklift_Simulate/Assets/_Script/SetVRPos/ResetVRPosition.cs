﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//VR頭盔固定在要駕駛的高度與位置(現實世界)，頭盔需水平放置，此時VR畫面看的位置會是錯誤的，且堆高機需在初始位置
//接下來按下L來重新定位VR的視野，此時畫面就會到正確的駕駛位置

public class ResetVRPosition : MonoBehaviour
{
    [SerializeField]
    GameObject VREyeObj;
    [SerializeField]
    GameObject SteamVRObj;
    [SerializeField]
    GameObject ResetRotateObj;

    [SerializeField]
    Vector3 OriDriveForkleftPos = new Vector3(-0.33f,1.74f,-0.1f);
    [SerializeField]
    Vector3 OriDriveForkleftRot = new Vector3(0f, 0f, 0f);


    Vector3 VRLookPos;
    Vector3 VRLookRot;

    Vector3 SteamObjOriPos;
    Vector3 SteamObjOriRot;

    void Start()
    {
        SteamObjOriPos = new Vector3(SteamVRObj.transform.position.x, SteamVRObj.transform.position.y, SteamVRObj.transform.position.z);
        SteamObjOriRot = new Vector3(SteamVRObj.transform.eulerAngles.x, SteamVRObj.transform.eulerAngles.y, SteamVRObj.transform.eulerAngles.z);
    }

    IEnumerator DalayMove()
    {
 
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
     

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(DalayMove());
        }
    }
}
