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

    [SerializeField]
    Transform ObjFrontToSeePos;

    [SerializeField]
    GameObject 子母畫面Obj;

    bool isOutsideCar = true;
    bool isHaveObjInFront = false;

    Vector3 cameraPrevirewPos;

    void Start()
    {
        ReticleImg.sprite = crosshairImage;
        isOutsideCar = true;
        OutsideCarTipTxt.enabled = false;

        Debug.Log("StageThreeGameManager.Instance.GameModes" + StageThreeGameManager.Instance.GameModes);

        //測試模式關提示
        if(CheckDeviceManager.gameMode_FirstStage == CheckDeviceManager.GameMode_firstStage.TestMode )
        {
            ReticleTxt.enabled = false;
        }
        else
        {
            ReticleTxt.enabled = true;

        }

    }

    void Update()
    {

        //開關提示
        if (Input.GetKeyDown(KeyCode.T))
        {
            ReticleTxt.enabled = !ReticleTxt.enabled;
        }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            lineR.SetPosition(0, Camera.main.transform.position);//+ new Vector3(1, 0, 0)
            lineR.SetPosition(1, ray.GetPoint(hit.distance));

            //游標變化
            if (hit.collider.GetComponent<MousePointInteractObj>() != null&&!isHaveObjInFront)
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

                //有物件在前面不執行
               if(!isHaveObjInFront) HandCubeObj.GetComponent<HandelContorller>().isPushHandTrig = true;

            }
            else
            {
                HandCubeObj.GetComponent<HandelContorller>().isPushHandTrig = false;
                StartCoroutine(DelayMoveBackHandTrigger());
                //HandCubeObj.transform.localPosition =new Vector3(0.15f,0,0.5f);

            }

            //鏡頭切換
            if (hit.collider.GetComponent<CameraPosChangeObj>() != null)
            {
                if (Input.GetMouseButton(0) &&  !isHaveObjInFront)
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
                    this.transform.GetChild(0).eulerAngles = hit.collider.GetComponent<CameraPosChangeObj>().OffsetRot;

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
                OutsideCarTipTxt.text = "'滑鼠'右鍵拉近畫面 \n 'Z'退出車輛";

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    isOutsideCar = true;
                    OutsideCarTipTxt.enabled = false;
                    this.transform.position = cameraPrevirewPos;
                    this.GetComponent<CapsuleCollider>().isTrigger = false;
                    this.GetComponent<Rigidbody>().useGravity = true;
                    this.GetComponent<FirstPersonController>().playerCanMove = true;
                    this.transform.GetChild(0).localEulerAngles = Vector3.zero;

                }
                
            }

            //物品移動至面前
            if (hit.collider.GetComponent<MoveToFrontToSeeObj>() != null)
            {
                if (isHaveObjInFront == false)
                {
                    if ( Input.GetMouseButton(0) && isOutsideCar)
                    {
                        hit.collider.gameObject.transform.SetParent(ObjFrontToSeePos);
                        hit.collider.gameObject.transform.localPosition = Vector3.zero + hit.collider.gameObject.GetComponent<MoveToFrontToSeeObj>().offsetVect;
                        hit.collider.gameObject.transform.localEulerAngles = Vector3.zero;

                        this.GetComponent<CapsuleCollider>().isTrigger = true;
                        this.GetComponent<Rigidbody>().useGravity = false;
                        this.GetComponent<FirstPersonController>().playerCanMove = false;

                        isHaveObjInFront = true;
                    }
                   
                }           
            }
            if (isHaveObjInFront == true)
            {
                OutsideCarTipTxt.enabled = true;
                OutsideCarTipTxt.text="'Z'放回物件";
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    isHaveObjInFront = false;
                    OutsideCarTipTxt.enabled = false;
                    GameObject frontObj = ObjFrontToSeePos.GetChild(0).gameObject;
                    frontObj.transform.SetParent(frontObj.GetComponent<MoveToFrontToSeeObj>().OriParentTans);
                    frontObj.transform.localPosition = frontObj.GetComponent<MoveToFrontToSeeObj>().OriLocalPos;
                    frontObj.transform.localEulerAngles = frontObj.GetComponent<MoveToFrontToSeeObj>().OriLocalRot;

                    this.GetComponent<CapsuleCollider>().isTrigger = false;
                    this.GetComponent<Rigidbody>().useGravity = true;
                    this.GetComponent<FirstPersonController>().playerCanMove = true;
                }
            }

            //開啟子母畫面
            if (hit.collider.GetComponent<MousePointInteractObj>() != null)
            {
                if (hit.collider.GetComponent<MousePointInteractObj>().ObjName == "煞車")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        子母畫面Obj.SetActive(true);
                    }
                    else
                    {
                        子母畫面Obj.SetActive(false);
                    }
                }
            }
          
        }

    }

    IEnumerator DelayMoveBackHandTrigger()
    {
        yield return new WaitForEndOfFrame();
        HandCubeObj.transform.localPosition = new Vector3(0.15f, 0, 0.5f);

    }
}
