using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayControlHandTrigger : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineR;
    [SerializeField]
    GameObject HandCubeObj;


    [SerializeField]
    Sprite crosshairImage;
    [SerializeField]
    Sprite crosshairImageChange;

    [SerializeField]
    Image ReticleImg;
    [SerializeField]
    Text ReticleTxt;
    [SerializeField]
    Text OutsideCarTipTxt;


    bool isOutsideCar = true;

    Vector3 cameraPrevirewPos;

    void Start()
    {
        ReticleImg.sprite = crosshairImage;
        isOutsideCar = true;
        OutsideCarTipTxt.enabled = false;
    }

    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            lineR.SetPosition(0, Camera.main.transform.position);//+ new Vector3(1, 0, 0)
            lineR.SetPosition(1, ray.GetPoint(hit.distance));

            //游標變化
            if (hit.collider.GetComponent<MousePointInteractObj>() != null)
            {
                ReticleImg.sprite = crosshairImageChange;
                ReticleTxt.text = hit.collider.GetComponent<MousePointInteractObj>().ObjName;
            }
            else
            {
                ReticleImg.sprite = crosshairImage;
                ReticleTxt.text = "";

            }

            //移動HandTrigger
            if (Input.GetMouseButton(0))
            {
                if(hit.collider.tag != "HandTrigger")
                {
                    Debug.Log(hit.collider.name);
                    HandCubeObj.transform.position = hit.point;
                }

               
                HandCubeObj.GetComponent<HandelContorller>().isPushHandTrig = true;
            }
            else
            {
                HandCubeObj.GetComponent<HandelContorller>().isPushHandTrig = false;
                HandCubeObj.transform.localPosition =new Vector3(0.15f,0,0.5f);

            }

            //鏡頭切換
            if (hit.collider.GetComponent<CameraPosChangeObj>() != null)
            {
                if (Input.GetMouseButton(0))
                {   
                    
                    //紀錄進入前的位置
                    if (isOutsideCar == true)
                    {
                        cameraPrevirewPos = this.transform.position;
                    }

                    this.GetComponent<CapsuleCollider>().isTrigger = true;
                    this.GetComponent<Rigidbody>().useGravity = false;
                    this.GetComponent<FirstPersonController>().playerCanMove = false;

                    this.transform.eulerAngles = hit.collider.GetComponent<CameraPosChangeObj>().CamChangePos.eulerAngles;
                    this.transform.position = hit.collider.GetComponent<CameraPosChangeObj>().CamChangePos.position;
                    isOutsideCar = false;

                }

        
            }
            else
            {


            }

            //在車內時
            if (isOutsideCar == false)
            {
                OutsideCarTipTxt.enabled = true;
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    isOutsideCar = true;
                    OutsideCarTipTxt.enabled = false;
                    this.transform.position = cameraPrevirewPos;
                    this.GetComponent<CapsuleCollider>().isTrigger = false;
                    this.GetComponent<Rigidbody>().useGravity = true;
                    this.GetComponent<FirstPersonController>().playerCanMove = true;
                }
            }

        }

    }
}
