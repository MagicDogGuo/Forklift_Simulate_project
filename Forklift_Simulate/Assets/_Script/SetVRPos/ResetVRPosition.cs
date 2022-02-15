using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//VR頭盔固定在要駕駛的高度與位置(現實世界)，頭盔需水平放置，此時VR畫面看的位置會是錯誤的
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

    void Start()
    {
    }

    IEnumerator DalayMove()
    {
               
        //讓ResetRotateObj的位置與VR EYE相同
        ResetRotateObj.transform.position = VREyeObj.transform.position;
        yield return new WaitForEndOfFrame();
        
        //把VR EYE關掉VR功能並設定成ResetRotateObj的子物件(如此VR EYE會在(0,0,0))
        VREyeObj.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        
        //旋轉SteamVRObj到校正旋轉角度
        VREyeObj.transform.SetParent(ResetRotateObj.transform);
        yield return new WaitForEndOfFrame();

        // 把VR EYE設定成SteamVRObj的子物件
        VRLookRot = VREyeObj.transform.eulerAngles;
        Vector3 moveRot = SteamVRObj.transform.eulerAngles - VRLookRot + OriDriveForkleftRot;//rotate會有問題因為Eye是子物件且沒有在(0,0,0)
        SteamVRObj.transform.eulerAngles += moveRot;
        VREyeObj.transform.SetParent(SteamVRObj.transform);
        yield return new WaitForEndOfFrame();
        
        //開啟VR功能
        VREyeObj.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();

        //最後把SteamVRObj移動至校正位置
        VRLookPos = VREyeObj.transform.position;
        Vector3 movePos = SteamVRObj.transform.position - VRLookPos + OriDriveForkleftPos;
        SteamVRObj.transform.position += movePos;


    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(DalayMove());
        }
    }
}
