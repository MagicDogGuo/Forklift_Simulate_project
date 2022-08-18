using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK.Examples;
using VRTK;
using VRTK.Controllables.ArtificialBased;
using VRTK.GrabAttachMechanics;
public class HandelContorller : MonoBehaviour
{
    public bool isPushHandTrig;//把手的開關

    Vector3 TipUIOffset = new Vector3(0.09f, 0.25f, 0.025f);
    [SerializeField]
    GameObject TipUIObj;
    [SerializeField]
    GameObject TipChooseUIObj;
    [SerializeField]
    AudioClip HornSound;
    [SerializeField]
    GameObject Key_onHand;
    [SerializeField]
    GameObject Key_onTable;
    [SerializeField]
    GameObject Table;

    AudioSource _audioSourse;

    float pushDegree = -13;///////////-20
    float unPushDegree = 0;

    CheckDeviceManager checkDeviceManager;
    
    //離合器
    string CluthPadelName_good;
    string CluthPadelName_bad;
    bool isInCluthPadel_Nor;
    bool isInCluthPadel_abNor;
    GameObject CluthPadel_Nor;
    GameObject CluthPadel_abNor;
    float oriTemp_Cluth = 0;
    GameObject TipObj_NormalBrakeCluthPadel;
    GameObject TipObj_abNormalCluthPadel;

    //煞車
    string BrakePadelName_good;
    string BrakePadelName_bad;
    string BrakePadelName_breakRearLight;
    bool isInBrakePadel_Nor;
    bool isInBrakePadel_abNor;
    GameObject BrakePadel_Nor;
    GameObject BrakePadel_abNor;
    float oriTemp_Brake = 0;
    GameObject TipObj_NormalBrakePadel;
    GameObject TipObj_abNormalBrakePadel;

    //方向盤
    string WheelBreakKeyWord;
    string WheelNormalKeyWord;
    bool isInWheel_nor;
    bool isInWheel_ab;
    GameObject Wheel;
    GameObject TipObj_BrakeWheel;

    //手煞車
    string HandBrakePadelName_good;
    string HandBrakePadelName_bad;
    bool isInHandBrakePadel_Nor;
    bool isInHandBrakePadel_abNor;
    GameObject HandBrakePadel_Nor;
    GameObject HandBrakePadel_abNor;
    float oriTemp_HandBrake = -5;
    GameObject TipObj_NormalHandBrakePadel;
    GameObject TipObj_abNormalHandBrakePadel;
    static  bool  isInUnStopBrakeArea=false;
    bool isPushRedDot = false;


    //安全帶
    string SafeBaltName_good;
    string SafeBaltName_bad;
    bool isInSafeBalt_Nor;
    bool isInSafeBalt_abNor;
    GameObject SafeBalt_Nor;
    GameObject SafeBalt_abNor;
    GameObject TipObj_NormalSafeBalt;
    GameObject TipObj_abNormalSafeBalt;



    //螺絲
    string ScrewBreakKeyWord;
    string ScrewNormalWord;
    bool isInScrewBreak;
    bool isInScrewNormal;
    GameObject BrakeScrew;
    GameObject NormalScrew;
    GameObject TipObj_BrakeScrew;
    GameObject TipObj_NormalScrew;

    //喇吧
    string HronBreakKeyWord;
    string HronNormalKeyWord;
    bool isInHorn_nor;
    bool isInHorn_ab;
    GameObject Hron;

    //鑰匙
    bool isInKey_onHand = false;
    bool isInKey_onTable = false;
    bool isInKey_onCar = false;
    bool isKey_onHand = false;
    public static bool isKey_onTable = false;
    bool isKey_onCar = false;
    GameObject Key_onCar;
    GameObject TipObj_KeyRotateTooMuch;
    bool isInTable = false;


 

    //右控制桿
    string ControlRight_右控制桿Name_Nor;
    string ControlRight_右控制桿Name_ab_BigLight;
    string ControlRight_右控制桿Name_ab_DirtLight_R;
    string ControlRight_右控制桿Name_ab_DirtLight_L;
    bool isInControlRight_右控制桿;
    GameObject ControlRight_右控制桿;
    GameObject TipChooseCnavasObj;

    //大燈拉桿
    GameObject Control_大燈拉桿;
    string Control_大燈拉桿Name_good;
    string Control_大燈拉桿Name_bad;
    bool isIn大燈拉桿_nor;
    bool isIn大燈拉桿_ab;

    //左控制桿
    string ControlLeft_左控制桿Name_Nor;
    string ControlLeft_左控制桿Name_ab_Light;
    string ControlLeft_左控制桿Name_ab_Sound;
    bool isInControlLeft_左控制桿;
    GameObject ControlLeft_左控制桿;
    float oriTempRotate_ControlLeft = 0;

    //固定銷
    string forkNameR_good;
    string forkNameR_bad;
    bool isInfork_固定銷R;
    GameObject fork_固定銷R;
    string forkNameL_good;
    string forkNameL_bad;
    bool isInfork_固定銷L;
    GameObject fork_固定銷L;

    string fork固定銷CurrentName;

    //油門踏板(額外)
    GameObject GasPadel_Nor;
    bool isInGasPadel =false;
    float oriTemp_GasPadel = 0;


    public VRTK_ControllerEvents events;

    bool isTakingObj;

    HandelContorller[] handelContorllers;


    public static bool isBackDirLight = true;
    public static bool isBackBigLight = true;
    public static bool isBackHandbrake = true;
    public static bool isBackFrontBack = true;


    void Start()
    {
        ControlLeft_左控制桿TriggerCount = 0;

        handelContorllers = GameObject.FindObjectsOfType<HandelContorller>();

        isKey_onTable = true;
        Key_onHand.SetActive(false);


        checkDeviceManager = GameObject.FindObjectOfType<CheckDeviceManager>();
        _audioSourse = this.GetComponent<AudioSource>();

        isInCluthPadel_Nor = false;
        isInBrakePadel_Nor = false;

        //抓取名字
        CluthPadelName_good = checkDeviceManager.clutch.goodObjName;
        CluthPadelName_bad = checkDeviceManager.clutch.badObjName;

        BrakePadelName_good = checkDeviceManager.brake.goodObjName;
        BrakePadelName_bad = checkDeviceManager.brake.badObjName;
        BrakePadelName_breakRearLight = checkDeviceManager.brake.breakRearLightName;

        WheelBreakKeyWord = checkDeviceManager.wheels.BreakWheelName;
        WheelNormalKeyWord = checkDeviceManager.wheels.NormalWheelName;

        HandBrakePadelName_good = checkDeviceManager.handBrake.goodObjName;
        HandBrakePadelName_bad = checkDeviceManager.handBrake.badObjName;

        SafeBaltName_good = checkDeviceManager.safeBalt.goodObjName;
        SafeBaltName_bad = checkDeviceManager.safeBalt.badObjName;

        ScrewBreakKeyWord = checkDeviceManager.tire.BreakScrewName;
        ScrewNormalWord = checkDeviceManager.tire.NormalScrewName;

        HronBreakKeyWord = checkDeviceManager.wheels.BreakHronName;
        HronNormalKeyWord = checkDeviceManager.wheels.NormalHornName;

        Key_onCar = checkDeviceManager.dashbroad.key_onCar;
        Key_onCar.GetComponent<MeshRenderer>().enabled =false;

        ControlRight_右控制桿Name_Nor = checkDeviceManager.carLight.NormalControlRight_右控制桿Name;
       // ControlRight_右控制桿Name_ab_BigLight = checkDeviceManager.carLight.ControlLeft_右控制桿_ab_BigLightName;
        ControlRight_右控制桿Name_ab_DirtLight_R = checkDeviceManager.carLight.ControlLeft_右控制桿_ab_DirtLight_RName;
        ControlRight_右控制桿Name_ab_DirtLight_L = checkDeviceManager.carLight.ControlLeft_右控制桿_ab_DirtLight_LName;
        ControlRight_右控制桿 = checkDeviceManager.carLight.ControlRight_右控制桿;
       
        Control_大燈拉桿 = checkDeviceManager.carLight.Control_大燈拉桿;
        Control_大燈拉桿Name_good = checkDeviceManager.carLight.Control_大燈拉桿_goodObjName;
        Control_大燈拉桿Name_bad = checkDeviceManager.carLight.Control_大燈拉桿_badObjName;

        ControlLeft_左控制桿Name_Nor = checkDeviceManager.carLight.NormalControlLeft_左控制桿Name;
        ControlLeft_左控制桿Name_ab_Light = checkDeviceManager.carLight.ControlLeft_左控制桿_ab_LightName;
        ControlLeft_左控制桿Name_ab_Sound = checkDeviceManager.carLight.ControlLeft_左控制桿_ab_SoundName;
        ControlLeft_左控制桿 = checkDeviceManager.carLight.ControlLeft_左控制桿;

        forkNameR_good = checkDeviceManager.fork_固定銷.NormalFork_固定銷RName;
        forkNameR_bad = checkDeviceManager.fork_固定銷.BreakFork_固定銷RName;
        fork_固定銷R = checkDeviceManager.fork_固定銷.Fork_固定銷R;

        forkNameL_good = checkDeviceManager.fork_固定銷.NormalFork_固定銷LName;
        forkNameL_bad = checkDeviceManager.fork_固定銷.BreakFork_固定銷LName;
        fork_固定銷L = checkDeviceManager.fork_固定銷.Fork_固定銷L;

        GasPadel_Nor = checkDeviceManager.GasPadel;

    }


    void Update()
    {
        //連動指定把手
        if (events != null)
        {
            isPushHandTrig = events.triggerPressed;

            if (events.triggerPressed)
            {
                Debug.Log("=====================+++++++++++++++++");
            }
        }


        //按下trigger時關閉另一個handcontrol
        if (isPushHandTrig)
        {
            foreach (var handCon in handelContorllers)
            {
                if (handCon.gameObject != this.gameObject)
                {
                    handCon.enabled = false;
                }
            }

        }
        else
        {
            foreach (var handCon in handelContorllers)
            {
                if (handCon.gameObject != this.gameObject)
                {
                    handCon.enabled = true;
                }
            }
        }

        //Debug.Log(this.name + " isInGasPade:" + isInGasPadel);

        PadelControl(CluthPadel_Nor, CluthPadel_abNor,isInCluthPadel_Nor, isInCluthPadel_abNor, "C");
        PadelControl(BrakePadel_Nor, BrakePadel_abNor, isInBrakePadel_Nor, isInBrakePadel_abNor, "B");
        PadelControl(HandBrakePadel_Nor, HandBrakePadel_abNor, isInHandBrakePadel_Nor, isInHandBrakePadel_abNor, "H");
        PadelControl(GasPadel_Nor, null, isInGasPadel, false, "G");//額外油門
        WheelControl();
        SafeBaltControl();
        ScrewControl();
        HornControl();
        ControlRight_右控制桿Control();
        Control_大燈拉桿Control();
        ControlLeft_左控制桿Control();
        Controlfork_固定銷();
        GetKeyOnTable();
        GiveKeyToCar();
        GetCanBeTakeObj();
    }

    [SerializeField]
    Vector3 offsetTip = new Vector3(1, 1, 0);

    bool ischeckHandBrakeTouch=false;

    void PadelControl(GameObject padel_Nor, GameObject padel_abNor, bool isInPadel_Nor, bool isInPadel_abNor, string type)
    {
        if (padel_Nor != null)
        {
            switch (type)
            {
                case "C":
                    CluthMove();
                    break;
                case "B":
                    BrakeMove();
                    break;
                case "H":
                    HandBrakeMove();
                    break;
                case "G":
                    GasPadelMove();
                    break;
            }
        }
        if (padel_abNor != null)
        {
            switch (type)
            {
                case "C":
                    CluthBreakTip();
                    break;
                case "B":
                    BrakeTip();
                    break;
                case "H":
                    HandBrakeBreakTip();
                    break;
            }
        }

        //正常
        void CluthMove()
        {
            if (isInPadel_Nor && isPushHandTrig)
            {
                oriTemp_Cluth = Mathf.MoveTowards(oriTemp_Cluth, pushDegree, 70f * Time.deltaTime);
            }
            else
            {
                oriTemp_Cluth = Mathf.MoveTowards(oriTemp_Cluth, 0, 70f * Time.deltaTime);
            }
            padel_Nor.transform.localEulerAngles = new Vector3(oriTemp_Cluth, 0, 0);

        }

        void BrakeMove()
        {
            if (isInPadel_Nor && isPushHandTrig)
            {
                oriTemp_Brake = Mathf.MoveTowards(oriTemp_Brake, pushDegree, 70f * Time.deltaTime);

                if (BrakePadel_Nor.name.Contains(BrakePadelName_breakRearLight))
                {//剎車燈異常
                    checkDeviceManager.ControlLight(false, "Brake_On");//////////////////////
                    //checkDeviceManager.ControlLight(true, "Brake_Off");

                }
                else
                {//剎車燈正常

                    checkDeviceManager.ControlLight(true, "Brake_On");
                }         
            }
            else
            {
                oriTemp_Brake = Mathf.MoveTowards(oriTemp_Brake, 0, 70f * Time.deltaTime);
                checkDeviceManager.ControlLight(true, "Brake_Off");
                checkDeviceManager.ControlLight(false, "Brake_Off");//////////////////////

            }

            padel_Nor.transform.localEulerAngles = new Vector3(oriTemp_Brake, 0, 0);

        }

        void HandBrakeMove()
        {
            if (!isPushHandTrig)
            {
                ischeckHandBrakeTouch = false;
            }

            if (isInPadel_Nor)
            {
                //先判斷是在哪裡，沒按下紅點的情況
                if (!isPushRedDot)
                {
                    if ((padel_Nor.transform.localEulerAngles.y - 360) >= -342 &&
                  (padel_Nor.transform.localEulerAngles.y - 360) <= -338)
                    {
                        isInUnStopBrakeArea = true;
                        isBackHandbrake = true;////////

                    }
                    else
                    {
                        isInUnStopBrakeArea = false;
                        isBackHandbrake = false;////////////////

                    }

                }

                //判斷有無按下紅點
                if (isPushHandTrig)
                {
                    //手煞紅點
                    padel_Nor.transform.GetChild(0).transform.localPosition = new Vector3(0.0043f, 0, 0.2819f);
                    isPushRedDot = true;
                }
                else
                {
                    //手煞紅點 原位
                    padel_Nor.transform.GetChild(0).transform.localPosition = new Vector3(0.004561948f, 0, 0.2845623f);
                    isPushRedDot = false;
                }

                //在移動把手
                if (!isInUnStopBrakeArea)//在原位
                {
                 
                    if (isPushRedDot )
                    {
                        oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, 20, 70f * Time.deltaTime);
                    }
                }
                else//在解煞車點
                {
                    if (isPushRedDot)
                    {
                        oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, -5, 70f * Time.deltaTime);

                        //oriTemp_HandBrake = -5;
                   
                    }
                }


            
                //
                //if (padel_Nor.transform.localEulerAngles.y >= 350 && isPushRedDot)//&& ischeckHandBrakeTouch == false)
                //{
                //    oriTemp_HandBrake = 25;
                //    //padel_Nor.transform.localEulerAngles = new Vector3(0, 20, 180);
                //   // ischeckHandBrakeTouch = true;
                //}

                //if ((padel_Nor.transform.localEulerAngles.y - 360) >= -342 &&
                //    (padel_Nor.transform.localEulerAngles.y - 360) <= -338 )
                //{
                //    isInUnStopBrakeArea = true;
                //}

         
            }
            else
            {
                ////手煞紅點
                //padel_Nor.transform.GetChild(0).transform.localPosition = new Vector3(0.004561948f, 0, 0.2845623f);

               // oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, 0, 70f * Time.deltaTime);

                isPushRedDot = false;
            }
            padel_Nor.transform.localEulerAngles = new Vector3(0, oriTemp_HandBrake, 180);

        }

        void GasPadelMove()
        {
            if (isInPadel_Nor && isPushHandTrig)
            {
                oriTemp_GasPadel = Mathf.MoveTowards(oriTemp_GasPadel, pushDegree, 70f * Time.deltaTime);
            }
            else
            {
                oriTemp_GasPadel = Mathf.MoveTowards(oriTemp_GasPadel, 0, 70f * Time.deltaTime);
            }
            padel_Nor.transform.localEulerAngles = new Vector3(0, (-oriTemp_GasPadel*1.8f)+30, 90);
            padel_Nor.transform.parent.localEulerAngles = new Vector3(0, oriTemp_GasPadel*0.8f, 0);
        }

        //異常
        void CluthBreakTip()
        {       
            if (isInPadel_abNor && isPushHandTrig)
            {
                oriTemp_Cluth = Mathf.MoveTowards(oriTemp_Cluth, -5, 70f * Time.deltaTime);

                if (TipObj_abNormalCluthPadel == null)
                {

                    //TipObj_abNormalCluthPadel = Instantiate(TipUIObj, padel_abNor.transform);
                    TipObj_abNormalCluthPadel = Instantiate(TipUIObj, this.transform);
                    TipObj_abNormalCluthPadel.GetComponentInChildren<Text>().text = "離合器踏板卡住!";
                    TipObj_abNormalCluthPadel.transform.localPosition = TipUIOffset;//new Vector3(0, 0.19f, 0.15f);
                    //TipObj_abNormalCluthPadel.transform.localEulerAngles = new Vector3(33, 0, -168);
                }
                if (TipObj_abNormalCluthPadel != null)
                {

                    TipObj_abNormalCluthPadel.transform.LookAt(GameObject.Find("TipUILookTraget").transform);
                }

            }
            else
            {
                oriTemp_Cluth = Mathf.MoveTowards(oriTemp_Cluth, 0, 70f * Time.deltaTime);

                Destroy(TipObj_abNormalCluthPadel);
            }
            //不動
            //padel_abNor.transform.localEulerAngles = new Vector3(oriTemp_Cluth, 0, 0);

        }
        void BrakeTip()
        {
            if (isInPadel_abNor && isPushHandTrig)
            {   
                //小移動
                oriTemp_Brake = Mathf.MoveTowards(oriTemp_Brake, -5, 70f * Time.deltaTime);
                if (TipObj_abNormalBrakePadel == null)
                {
                 
                    //TipObj_abNormalBrakePadel = Instantiate(TipUIObj, padel_abNor.transform);
                    TipObj_abNormalBrakePadel = Instantiate(TipUIObj, this.transform);
                    TipObj_abNormalBrakePadel.GetComponentInChildren<Text>().text = "煞車踏板卡住!";
                    TipObj_abNormalBrakePadel.transform.localPosition = TipUIOffset;//new Vector3(0, 0.19f, 0.15f);
                    //TipObj_abNormalBrakePadel.transform.localEulerAngles = new Vector3(33, 0, -168);

                }
                if (TipObj_abNormalBrakePadel != null)
                {
                    TipObj_abNormalBrakePadel.transform.LookAt(GameObject.Find("TipUILookTraget").transform);
                }

            }
            else
            {
                oriTemp_Brake = Mathf.MoveTowards(oriTemp_Brake, 0, 70f * Time.deltaTime);

                Destroy(TipObj_abNormalBrakePadel);
            }
            //不動
            //padel_abNor.transform.localEulerAngles = new Vector3(oriTemp_Brake, 0, 0);

        }
        void HandBrakeBreakTip()
        {
            if (!isPushHandTrig)
            {
                ischeckHandBrakeTouch = false;
            }

            if (isInPadel_abNor)
            {
                //先判斷是在哪裡，沒按下紅點的情況
                if (!isPushRedDot)
                {
                    if ((padel_abNor.transform.localEulerAngles.y - 360) >= -342 &&
                        (padel_abNor.transform.localEulerAngles.y - 360) <= -318)
                    {
                        isInUnStopBrakeArea = true;
                    }
                    else
                    {
                        isInUnStopBrakeArea = false;
                    }
                }

                //判斷有無按下紅點
                if (isPushHandTrig)
                {
                    //手煞紅點
                    padel_abNor.transform.GetChild(0).transform.localPosition = new Vector3(0.0043f, 0, 0.2819f);
                    isPushRedDot = true;
                }
                else
                {
                    //手煞紅點 原位
                    padel_abNor.transform.GetChild(0).transform.localPosition = new Vector3(0.004561948f, 0, 0.2845623f);
                    isPushRedDot = false;
                }

                //在移動把手
                if (!isInUnStopBrakeArea)//在原位
                {
                    if (isPushRedDot)
                    {                      
                        //oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, 20, 70f * Time.deltaTime);
                    }
                }
                else//在解煞車點
                {
                    if (isPushRedDot)
                    {
                        oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, -5, 70f * Time.deltaTime);
                    }
                }
            }
            else
            {
                //回彈回原位
                //oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, -5, 70f * Time.deltaTime);
                oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, 20, 70f * Time.deltaTime);
                isPushRedDot = false;
            }
            padel_abNor.transform.localEulerAngles = new Vector3(0, oriTemp_HandBrake+20, 180);

            //提示
            if (isInPadel_abNor && isPushHandTrig)
            {
                if (TipObj_abNormalHandBrakePadel == null)
                {
                    //TipObj_abNormalHandBrakePadel = Instantiate(TipUIObj, padel_abNor.transform);
                    TipObj_abNormalHandBrakePadel = Instantiate(TipUIObj, this.transform);
                    TipObj_abNormalHandBrakePadel.GetComponentInChildren<Text>().text = "手煞無法固定!";
                    TipObj_abNormalHandBrakePadel.transform.localPosition = TipUIOffset;//new Vector3(0.04f, 0.05f, 0.4f);
                    //TipObj_abNormalHandBrakePadel.transform.localEulerAngles = new Vector3(0, 0, 130);

                }
                if (TipObj_abNormalHandBrakePadel != null)
                {
                    TipObj_abNormalHandBrakePadel.transform.LookAt(GameObject.Find("TipUILookTraget").transform);
                }
            }
            else if(isPushHandTrig==false)
            {
                Destroy(TipObj_abNormalHandBrakePadel);
            }
        }
    }

    void WheelControl()
    {
        if (Wheel != null)
        {
            //正常
            if (isInWheel_nor && isPushHandTrig)
            {
                //Wheel.GetComponentInParent<VRTK_ArtificialRotator>().enabled = true;
                //Wheel.transform.parent.GetComponentInParent<VRTK_InteractableObject>().enabled = true;
            }

            //異常
            if (isInWheel_ab && isPushHandTrig)
            {
                //Wheel.transform.parent.GetComponentInParent<VRTK_InteractableObject>().enabled = false;
                //Wheel.GetComponentInParent<VRTK_ArtificialRotator>().enabled = false;
                
                if (TipObj_BrakeWheel == null)
                {
                    //TipObj_BrakeWheel = Instantiate(TipUIObj, Wheel.transform.parent.transform);
                    TipObj_BrakeWheel = Instantiate(TipUIObj, this.transform);
                    TipObj_BrakeWheel.GetComponentInChildren<Text>().text = "轉不動方向盤!";
                    TipObj_BrakeWheel.transform.localPosition = TipUIOffset;//new Vector3(0, 0.2f, -0.34f);
                }
                if (TipObj_BrakeWheel != null)
                    TipObj_BrakeWheel.transform.LookAt(GameObject.Find("TipUILookTraget").transform);

            }
            else
            {
               if(TipObj_BrakeWheel!=null) Destroy(TipObj_BrakeWheel);
            }
        }
    }

    bool isInSeatBeltCheck = false;

    //安全帶
    void SafeBaltControl()
    {
        if (isPushHandTrig == false)
        {
            isInSeatBeltCheck = false;
        }

        if (isInSafeBalt_Nor && isPushHandTrig && isInSeatBeltCheck == false)
        {
            isInSeatBeltCheck = true;

            SafeBalt_Nor.GetComponent<MeshRenderer>().enabled = false;
           
            GameObject SeatBeltLong = GameObject.Find("seat belt_belt");
            SeatBeltLong.GetComponent<MeshRenderer>().enabled = !SeatBeltLong.GetComponent<MeshRenderer>().enabled;
       
            if(SeatBeltLong.GetComponent<MeshRenderer>().enabled == false)
            {
                SafeBalt_Nor.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        if (isInSafeBalt_abNor && isPushHandTrig)
        {
            if (TipObj_abNormalSafeBalt == null)
            {
                SafeBalt_abNor.transform.localPosition = new Vector3(-0.003849184f, 0.1943655f,-0.05f);
                TipObj_abNormalSafeBalt = Instantiate(TipUIObj, this.transform);
                TipObj_abNormalSafeBalt.GetComponentInChildren<Text>().text = "安全帶卡住!";
                TipObj_abNormalSafeBalt.transform.localPosition = TipUIOffset;//new Vector3(0.04f, 0.05f, 0.4f);
                                                                                    //TipObj_abNormalHandBrakePadel.transform.localEulerAngles = new Vector3(0, 0, 130);
            }
            if (TipObj_abNormalSafeBalt != null)
            {

                if (GameObject.Find("TipUILookTraget")!=null) TipObj_abNormalSafeBalt.transform.LookAt(GameObject.Find("TipUILookTraget").transform);
            }
            //Debug.Log("ssssssssssssssssssssssssssssssssssssss");

        }
        else
        {
           if(SafeBalt_abNor!=null) SafeBalt_abNor.transform.localPosition = new Vector3(-0.003849184f, 0.1943655f, -0.0797f);
            Destroy(TipObj_abNormalSafeBalt);
        }
    }


    float tempScrewRot=0;
    void ScrewControl()
    {
        if (BrakeScrew != null)
        {
            if (isInScrewBreak && isPushHandTrig)
            {
                Debug.Log("============tempScrewRot:" + tempScrewRot);

                //螺絲鬆動
                tempScrewRot += 45 * Time.deltaTime;
                BrakeScrew.transform.GetChild(3).transform.localEulerAngles = new Vector3(90, 0, tempScrewRot);
                BrakeScrew.transform.GetChild(7).transform.localEulerAngles = new Vector3(90, 0, tempScrewRot*0.9f);

                if (TipObj_BrakeScrew == null)
                {
                    //TipObj_BrakeScrew = Instantiate(TipUIObj, BrakeScrew.transform);
                    TipObj_BrakeScrew = Instantiate(TipUIObj, this.transform);
                    TipObj_BrakeScrew.GetComponentInChildren<Text>().text = "螺絲未鎖緊!";
                    TipObj_BrakeScrew.transform.localPosition = TipUIOffset;// new Vector3(-0.05f, -.25f, 0);
                 
                }
                if (TipObj_BrakeScrew != null) {
                    TipObj_BrakeScrew.transform.LookAt(GameObject.Find("TipUILookTraget").transform.position);
                }

            }
            else 
            {
                tempScrewRot = 0;
                Destroy(TipObj_BrakeScrew);
            }
        }
        if (NormalScrew != null)
        {
            if (isInScrewNormal && isPushHandTrig)
            {
                if (TipObj_NormalScrew == null)
                {
                    //TipObj_NormalScrew = Instantiate(TipUIObj, NormalScrew.transform);
                    TipObj_NormalScrew = Instantiate(TipUIObj, this.transform);
                    TipObj_NormalScrew.GetComponentInChildren<Text>().text = "所有螺絲正常!";
                    TipObj_NormalScrew.transform.localPosition = TipUIOffset;// new Vector3(0.09f, 0.25f, 0.025f);

                }
                if (TipObj_NormalScrew != null)
                {
                    TipObj_NormalScrew.transform.LookAt(GameObject.Find("TipUILookTraget").transform);
                }
            }
            else 
            {
                Destroy(TipObj_NormalScrew);
            }
        }
    }

    bool isSound = false;
    void HornControl()
    {
        //if(isInHorn_nor || isInHorn_ab)
        //{
        //    if (Wheel != null && isInWheel_ab)//按喇叭時不轉方向盤///////////////////////////////////
        //    {
        //        Wheel.transform.parent.GetComponentInParent<VRTK_InteractableObject>().enabled = false;
        //        Wheel.GetComponentInParent<VRTK_ArtificialRotator>().enabled = false;
        //    }
        //}
    

        if (Hron != null)
        {
            //正常
            if (isInHorn_nor && isPushHandTrig&& !isSound)
            {
                _audioSourse.clip = HornSound;
                Hron.transform.localPosition = new Vector3(0.0048f, 0, 0.0086f);
                if (!_audioSourse.isPlaying)
                {
                    _audioSourse.Stop();
                    _audioSourse.Play();
                    isSound = true;
                }
            }
            //異常
            if(isInHorn_ab && isPushHandTrig)
            {
                Hron.transform.localPosition = new Vector3(0.0048f, 0, 0.0086f);
            }

            if(isPushHandTrig == false)
            {
                Hron.transform.localPosition = new Vector3(0.0048f, 0, 0.0112f);
                isSound = false;
            }
        }
    }



    void ControlRight_右控制桿Control()
    {
        if (isInControlRight_右控制桿 && isPushHandTrig)
        {
            if (TipChooseCnavasObj == null)
            {
                TipChooseCnavasObj = Instantiate(TipChooseUIObj, ControlRight_右控制桿.transform);
                TipChooseCnavasObj.transform.localRotation = new Quaternion(0, 180, 0, 0);
                TipChooseCnavasObj.transform.localPosition = new Vector3(0, 0.06f, 0.04f);
                Button[] btn = TipChooseCnavasObj.GetComponent<TipChooseUI_CheckDevice>().stateBtn;
                btn[0].GetComponentInChildren<Text>().text = "後推";
                btn[1].GetComponentInChildren<Text>().text = "前推";
                btn[2].GetComponentInChildren<Text>().text = "回歸";
                btn[3].gameObject.SetActive(false) ;
                //btn[4].GetComponentInChildren<Text>().text = "旋轉回歸";

                //正常
                btn[0].onClick.AddListener(() => { checkDeviceManager.ControlLight(true, "R_On");Destroy(TipChooseCnavasObj); isBackDirLight = false; });
                btn[1].onClick.AddListener(() => { checkDeviceManager.ControlLight(true, "L_On"); Destroy(TipChooseCnavasObj); isBackDirLight = false; });
                //btn[2].onClick.AddListener(() => { checkDeviceManager.ControlLight(true, "Big_On"); Destroy(TipChooseCnavasObj); });
                btn[2].onClick.AddListener(() => { checkDeviceManager.ControlLight(true, "Dirct_Off"); isBackDirLight = true; });
                
                ////大燈異常
                //if (ControlRight_右控制桿.name.Contains(ControlRight_右控制桿Name_ab_BigLight))
                //{
                //    btn[2].onClick.RemoveAllListeners();
                //    //btn[4].onClick.RemoveAllListeners();
                //    btn[2].onClick.AddListener(() => { checkDeviceManager.ControlLight(false, "Big_On"); Destroy(TipChooseCnavasObj); });

                //}
                //右燈異常
                if (ControlRight_右控制桿.name.Contains(ControlRight_右控制桿Name_ab_DirtLight_R))
                {
                    btn[0].onClick.RemoveAllListeners();
                    btn[0].onClick.AddListener(() => { checkDeviceManager.ControlLight(false, "R_On"); Destroy(TipChooseCnavasObj); });

                }
                //左燈異常
                if (ControlRight_右控制桿.name.Contains(ControlRight_右控制桿Name_ab_DirtLight_L))
                {
                    btn[1].onClick.RemoveAllListeners();
                    btn[1].onClick.AddListener(() => { checkDeviceManager.ControlLight(false, "L_On"); Destroy(TipChooseCnavasObj); });

                }
            }
        }
    }


    public static int Control_大燈桿TriggerCount = 0;//只會算這個(獨一無二)
    public bool isCount_大燈 = false;

    void Control_大燈拉桿Control ()
    {

        if (isPushHandTrig == false)
        {
            isCount_大燈 = false;
        }
        if (isPushHandTrig && isIn大燈拉桿_nor && isCount_大燈 == false )
        {
            Debug.Log("=-==============" + isIn大燈拉桿_nor);

            Control_大燈桿TriggerCount++;
            isCount_大燈 = true;
            ////正常
            //checkDeviceManager.ControlLight(true, "Big_On");
        }
        if (isPushHandTrig && isIn大燈拉桿_ab && isCount_大燈 == false)
        {
            Control_大燈桿TriggerCount++;
            isCount_大燈 = true;
          
        }
        //if(isPushHandTrig&& isIn大燈拉桿_ab)
        //{
        //    //異常
        //    checkDeviceManager.ControlLight(false, "Big_On");
        //}


        if (Control_大燈桿TriggerCount % 2 != 0)
        {
            isBackBigLight = false;

            if (isIn大燈拉桿_nor)
            {
                //正常
                checkDeviceManager.ControlLight(true, "Big_On");
            }
            if (isIn大燈拉桿_ab)
            {
                //異常
                checkDeviceManager.ControlLight(false, "Big_On");
            }



        }
        else
        {//控制桿固定關
            isBackBigLight = true;

            if (isIn大燈拉桿_nor)
            {
                //正常
                checkDeviceManager.ControlLight(true, "Big_Off");
            }
            if (isIn大燈拉桿_ab)
            {
                //異常
                checkDeviceManager.ControlLight(false, "Big_Off");
            }
        }




    }

    public static int ControlLeft_左控制桿TriggerCount=0;//只會算這個(獨一無二)
    public bool isCount_左控制桿 = false;
    void ControlLeft_左控制桿Control()
    {
        if (isPushHandTrig == false)
        {
            isCount_左控制桿 = false;

            //其他把手同步
            //foreach (var handCon in handelContorllers)
            //{
            //    if (handCon.gameObject != this.gameObject)
            //    {
            //        handCon.ControlLeft_左控制桿TriggerCount = ControlLeft_左控制桿TriggerCount;
            //        handCon.isCount_左控制桿 = isCount_左控制桿;
            //    }
            //}
        }
        if (isInControlLeft_左控制桿 && isPushHandTrig && isCount_左控制桿==false && isInHandBrakePadel_Nor == false)///////////////////////////////////////////
        {
            ControlLeft_左控制桿TriggerCount++;
            isCount_左控制桿 = true; 
            
            //其他把手同步
            //foreach (var handCon in handelContorllers)
            //{
            //    if (handCon.gameObject != this.gameObject)
            //    {
            //        handCon.ControlLeft_左控制桿TriggerCount = ControlLeft_左控制桿TriggerCount;
            //        handCon.isCount_左控制桿 = isCount_左控制桿;
            //    }
            //}
        }


        if (ControlLeft_左控制桿TriggerCount % 2 != 0)
        {//控制桿固定開
            oriTempRotate_ControlLeft = Mathf.MoveTowards(oriTempRotate_ControlLeft, pushDegree, 70f * Time.deltaTime);
            //正常
            if (ControlLeft_左控制桿.name == ControlLeft_左控制桿Name_Nor)
            {
                checkDeviceManager.ControlLight(true, "RevLight_On");
            }
            //倒車燈異常
            if (ControlLeft_左控制桿.name.Contains( ControlLeft_左控制桿Name_ab_Light))
            {
                //Debug.Log("sssssssssssssssssssssssssssssssssssssss   "  + ControlLeft_左控制桿TriggerCount);
                //Debug.Log("ControlLeft_左控制桿" + ControlLeft_左控制桿);

                checkDeviceManager.ControlLight(true, "RevLight_On_BreakLight");//////////////////////
            }
            //倒車警示異常
            if (ControlLeft_左控制桿.name.Contains(ControlLeft_左控制桿Name_ab_Sound))
            {
                checkDeviceManager.ControlLight(true, "RevLight_On_BreakSound");////////////////////////////
            }

            isBackFrontBack = false;
        }
        else
        {//控制桿固定關

            oriTempRotate_ControlLeft = Mathf.MoveTowards(oriTempRotate_ControlLeft, 0, 70f * Time.deltaTime);
            checkDeviceManager.ControlLight(true, "RevLight_Off");
            isBackFrontBack = true;
        }
        ControlLeft_左控制桿.transform.localEulerAngles = new Vector3(0, 0, -oriTempRotate_ControlLeft);
    }

    void Controlfork_固定銷()
    {
        //R
        if (isInfork_固定銷R && isPushHandTrig)
        {
            if(fork固定銷CurrentName == forkNameR_good)
            {
                fork_固定銷R.gameObject.GetComponent<Animator>().SetBool("IsTouchNor", true);

            }
            else if (fork固定銷CurrentName == forkNameR_bad)
            {
                fork_固定銷R.gameObject.GetComponent<Animator>().SetTrigger("IsTouchAbNor");
            }
        }
        else
        {
            if (fork固定銷CurrentName == forkNameR_good)
            {
                fork_固定銷R.gameObject.GetComponent<Animator>().SetBool("IsTouchNor", false);
            }

        }
        //L
        if (isInfork_固定銷L && isPushHandTrig)
        {
            if (fork固定銷CurrentName == forkNameL_good)
            {
                Debug.Log("other.name");

                fork_固定銷L.gameObject.GetComponent<Animator>().SetBool("IsTouchNor", true);

            }
            //else if (fork固定銷CurrentName == forkNameL_bad)
            //{
            //    fork_固定銷L.gameObject.GetComponent<Animator>().SetTrigger("IsTouchAbNor");
            //}
        }
        else
        {
            if (fork固定銷CurrentName == forkNameL_good)
            {
                fork_固定銷L.gameObject.GetComponent<Animator>().SetBool("IsTouchNor", false);
            }

        }

    }

    void GetKeyOnTable()
    {
        Debug.Log("isInTable" + isInTable + " isInKey_onTable" + isInKey_onTable);

        //桌上拿起
        if (isInKey_onTable && isPushHandTrig && isKey_onTable == true)
        {
            isKey_onTable = false;
            isKey_onHand = true;
            isKey_onCar = false;
            Key_onHand.SetActive(true);
            Key_onTable.SetActive(false);
            GameObject.Find("TableUICanvas").GetComponent<Canvas>().enabled = true;
        }
        //放回桌上
        if (isInTable == true && isPushHandTrig && isInKey_onTable == false && isKey_onHand == true)
        {
            isKey_onTable = true;
            isKey_onHand = false;
            isKey_onCar = false;
            Key_onHand.SetActive(false);
            Key_onTable.SetActive(true);
            GameObject.Find("TableUICanvas").GetComponent<Canvas>().enabled = false;
        }
    }
    void GiveKeyToCar()
    {
        if ((isInKey_onCar && isPushHandTrig && isKey_onHand)
            || (isInKey_onCar && isKey_onCar && isPushHandTrig))////////////////////////////鑰匙有在手上
        {

            isKey_onTable = false;
            isKey_onHand = false;
            isKey_onCar = true;
            Key_onHand.SetActive(false);
            Key_onCar.GetComponent<MeshRenderer>().enabled = true;



            if (TipChooseCnavasObj == null)
            {
                TipChooseCnavasObj = Instantiate(TipChooseUIObj, Key_onCar.transform.parent.transform);
                TipChooseCnavasObj.transform.localEulerAngles = new Vector3(28, 90, 92);
                TipChooseCnavasObj.transform.localPosition = new Vector3(-0.16f, -0.02f, -0.15f);
                Button[] btn = TipChooseCnavasObj.GetComponent<TipChooseUI_CheckDevice>().stateBtn;
                btn[0].transform.parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(180, 120);
                btn[0].GetComponentInChildren<Text>().text = "轉動1階（約45°）";
                btn[1].GetComponentInChildren<Text>().text = "轉動2階（約90°）";
                btn[2].GetComponentInChildren<Text>().text = "拔出鑰匙";
                btn[3].gameObject.SetActive(false);
                
                TipChooseCnavasObj.GetComponent<TipChooseUI_CheckDevice>().closeBtn.onClick.AddListener(() => { isInKey_onCar = false; });

                //正常
                btn[0].onClick.AddListener(() => {
                    isInKey_onCar = false;
                    if (checkDeviceManager.dashbroad.OnKeyPlugIn_NoSound != null)
                    {
                        checkDeviceManager.dashbroad.OnKeyPlugIn_NoSound();
                        Key_onHand.SetActive(false);
                      

                    }

                    if (TipObj_KeyRotateTooMuch != null)
                    {
                        Destroy(TipObj_KeyRotateTooMuch);
                    }

                    Key_onCar.transform.localEulerAngles = new Vector3(-45, -27.31f, -90);
                    Destroy(TipChooseCnavasObj);

                    //警示燈
                    if (checkDeviceManager.alertLight_工作警示燈.goodObj_燈罩.GetActive() == true)
                    {
                        checkDeviceManager.alertLight_工作警示燈.goodObj_燈罩.GetComponent<MeshRenderer>().enabled = true;
                        checkDeviceManager.alertLight_工作警示燈.goodObj_燈罩.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

                    }


                });
                btn[1].onClick.AddListener(() => {
                    isInKey_onCar = false;
                    if (checkDeviceManager.dashbroad.OnKeyPlugIn_HaveSound != null)
                    {
                        checkDeviceManager.dashbroad.OnKeyPlugIn_HaveSound();
                        Key_onHand.SetActive(false);
                        
                    }

                    //警告
                    if (CheckDeviceManager.gameMode_FirstStage == CheckDeviceManager.GameMode_firstStage.PracticeMode)
                    {//練習
                        if (TipObj_KeyRotateTooMuch == null)
                        {
                            GameObject pa = GameObject.Find("Group038");
                            TipObj_KeyRotateTooMuch = Instantiate(TipUIObj, pa.transform);// this.transform);
                            TipObj_KeyRotateTooMuch.GetComponentInChildren<Text>().text = "危險：你已發動引擎，請切換階級";
                            TipObj_KeyRotateTooMuch.transform.localPosition = TipUIOffset + new Vector3(0, 0.03f, 0.25f);
                        }
                    }
                    else if (CheckDeviceManager.gameMode_FirstStage == CheckDeviceManager.GameMode_firstStage.TestMode)
                    {//測驗
                        CheckDeviceManager.isLetEngineOn = true;
                    }


                    Key_onCar.transform.localEulerAngles = new Vector3(-90, -27.31f, -90);
                    Destroy(TipChooseCnavasObj);

                    //警示燈
                    if (checkDeviceManager.alertLight_工作警示燈.goodObj_燈罩.GetActive() == true)
                    {
                        checkDeviceManager.alertLight_工作警示燈.goodObj_燈罩.GetComponent<MeshRenderer>().enabled = true;
                        checkDeviceManager.alertLight_工作警示燈.goodObj_燈罩.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

                    }
                });

                btn[2].onClick.AddListener(() =>
                {
                    isInKey_onCar = false;
                    isKey_onCar = false;
                    isKey_onHand = true;
                    Key_onCar.GetComponent<MeshRenderer>().enabled = false;
                    Key_onHand.SetActive(true);

                    Destroy(TipChooseCnavasObj);

                    if (TipObj_KeyRotateTooMuch != null)
                    {
                        Destroy(TipObj_KeyRotateTooMuch);
                    }
                    if (checkDeviceManager.dashbroad.OnKeyPlugOut != null)
                    {
                        checkDeviceManager.dashbroad.OnKeyPlugOut();
                    }

                    //警示燈關
                    if (checkDeviceManager.alertLight_工作警示燈.goodObj_燈罩.GetActive() == true)
                    {
                        checkDeviceManager.alertLight_工作警示燈.goodObj_燈罩.GetComponent<MeshRenderer>().enabled = false;
                        checkDeviceManager.alertLight_工作警示燈.goodObj_燈罩.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                        GameObject.Find("ReverseAlarmLight").GetComponent<Light>().enabled = false;
                    }
                });
            }




            //if (checkDeviceManager.dashbroad.OnKeyPlugIn_NoSound != null)
            //{
            //    checkDeviceManager.dashbroad.OnKeyPlugIn_NoSound();
            //    Key_onHand.SetActive(false);
            //}

            //if (checkDeviceManager.dashbroad.OnKeyPlugIn_HaveSound != null)
            //{
            //    checkDeviceManager.dashbroad.OnKeyPlugIn_HaveSound();
            //    Key_onHand.SetActive(false);
            //}
        }
        if (TipObj_KeyRotateTooMuch != null)
        {
            TipObj_KeyRotateTooMuch.transform.LookAt(GameObject.Find("TipUILookTraget").transform);
        }
    }

    void GetCanBeTakeObj()
    {
        if (isInBeTakeObj == true && isPushHandTrig)
        {
            //Debug.Log("==============引擎機油尺=");
           // BeTakingObj.transform.position = this.transform.position + BeTakingObj.GetComponent<CanBeHandTakeObj>().OffsetPos;
            //BeTakingObj.transform.localEulerAngles = new Vector3(BeTakingObj.GetComponent<CanBeHandTakeObj>().oriRota.x, 
            //BeTakingObj.GetComponent<CanBeHandTakeObj>().oriRota.y,
            //this.transform.localEulerAngles.z);
            BeTakingObj.transform.SetParent(this.transform.gameObject.transform);

            //Debug.Log("------------------'" + GameObject.FindObjectOfType<VRTK_TransformFollow>().transform.GetChild(0).name);
            if (GameObject.Find("[VRTK][AUTOGEN][RightControllerScriptAlias][BasePointerRenderer_Origin_Smoothed]") != null)
                GameObject.Find("[VRTK][AUTOGEN][RightControllerScriptAlias][BasePointerRenderer_Origin_Smoothed]").transform.GetChild(0).gameObject.SetActive(false);
            
            if (GameObject.Find("[VRTK][AUTOGEN][LeftControllerScriptAlias][BasePointerRenderer_Origin_Smoothed]") != null)
                GameObject.Find("[VRTK][AUTOGEN][LeftControllerScriptAlias][BasePointerRenderer_Origin_Smoothed]").transform.GetChild(0).gameObject.SetActive(false);

        }
        else
        {
            if (BeTakingObj != null)
            {
                isInBeTakeObj = false;/////////////////////////////////////
                BeTakingObj.transform.SetParent(BeTakingObj.GetComponent<CanBeHandTakeObj>().oriPareant.transform);
                BeTakingObj.transform.localPosition = BeTakingObj.GetComponent<CanBeHandTakeObj>().oriPos;
                BeTakingObj.transform.localEulerAngles = BeTakingObj.GetComponent<CanBeHandTakeObj>().oriRota;
                if (GameObject.Find("[VRTK][AUTOGEN][RightControllerScriptAlias][BasePointerRenderer_Origin_Smoothed]") != null)
                    GameObject.Find("[VRTK][AUTOGEN][RightControllerScriptAlias][BasePointerRenderer_Origin_Smoothed]").transform.GetChild(0).gameObject.SetActive(true);
                if (GameObject.Find("[VRTK][AUTOGEN][LeftControllerScriptAlias][BasePointerRenderer_Origin_Smoothed]") != null)
                    GameObject.Find("[VRTK][AUTOGEN][LeftControllerScriptAlias][BasePointerRenderer_Origin_Smoothed]").transform.GetChild(0).gameObject.SetActive(true);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("=============");

        Debug.Log("other"+other.name); 
        //離和器
        if (other.name == CluthPadelName_good)
        {
            isInCluthPadel_Nor = true;
            CluthPadel_Nor = other.gameObject;
        }
        if (other.name == CluthPadelName_bad)
        {
            isInCluthPadel_abNor = true;
            CluthPadel_abNor = other.gameObject;
        }
        //剎車
        if (other.name.Contains(BrakePadelName_good))
        {
            isInBrakePadel_Nor = true;
            BrakePadel_Nor = other.gameObject;
        }
        if (other.name.Contains(BrakePadelName_bad))
        {
            isInBrakePadel_abNor = true;
            BrakePadel_abNor = other.gameObject;
        }  
        //方向盤
        if (other.name.Contains(WheelNormalKeyWord))
        {
            isInWheel_nor = true;
            Wheel = other.gameObject;
        }
        if (other.name.Contains(WheelBreakKeyWord))
        {
            isInWheel_ab = true;
            Wheel = other.gameObject;
            //Destroy(Wheel.transform.parent.GetComponentInParent<VRTK_InteractableObject>());
            //Destroy(Wheel.GetComponentInParent<VRTK_ArtificialRotator>());

            //Wheel.transform.parent.GetComponentInParent<VRTK_InteractableObject>().enabled = false;
            //Wheel.GetComponentInParent<VRTK_ArtificialRotator>().enabled = false;
        }
        //手煞
        if (other.name == HandBrakePadelName_good)
        {
            isInHandBrakePadel_Nor = true;
            HandBrakePadel_Nor = other.gameObject;
        }
        if (other.name == HandBrakePadelName_bad)
        {
            isInHandBrakePadel_abNor = true;
            HandBrakePadel_abNor = other.gameObject;
        }
        //安全帶
        if (other.name == SafeBaltName_good)
        {
            isInSafeBalt_Nor = true;
            SafeBalt_Nor = other.gameObject;
        }
        if (other.name == SafeBaltName_bad)
        {
            isInSafeBalt_abNor = true;
            SafeBalt_abNor = other.gameObject;
        }

        //螺絲
        if (other.name.Contains(ScrewBreakKeyWord))
        {
            isInScrewBreak = true;
            BrakeScrew = other.gameObject;
        }
        if (other.name.Contains(ScrewNormalWord))
        {
            isInScrewNormal= true;
            NormalScrew = other.gameObject;
        }
        //喇叭
        if (other.name.Contains(HronNormalKeyWord))
        {
            isInHorn_nor = true;
            Hron = other.gameObject;
        }
        if (other.name.Contains(HronBreakKeyWord))
        {
            isInHorn_ab = true;
            Hron = other.gameObject;
        }
        //鑰匙
        if (other.name == Key_onCar.name)
        {
            isInKey_onTable = false;
            isInKey_onHand = false;
            isInKey_onCar = true;
            isInTable = false;
        }
        if (other.name == Key_onTable.name )
        {
            isInKey_onTable = true;
            isInKey_onHand = false;
            isInKey_onCar = false;
            isInTable = false;
        }
        if (other.name == Table.name)
        {
            isInKey_onTable = false;
            isInKey_onHand = false;
            isInKey_onCar = false;
            isInTable = true;
        }
        
        //右控制感(燈號)
        if (other.name == ControlRight_右控制桿.name)
        {
            isInControlRight_右控制桿 = true;
        }
        //大燈/////////////////////////////////////////////////
        if(other.name == Control_大燈拉桿Name_good)
        {
             isIn大燈拉桿_nor=true;
        }
        if (other.name == Control_大燈拉桿Name_bad)
        {
            isIn大燈拉桿_ab = true;

        }

        //左控制感(前後)
        if (other.name == ControlLeft_左控制桿.name)
        {
            isInControlLeft_左控制桿 = true;
   
        }
       
        //貨插固定銷
        if (other.name == forkNameR_good|| other.name == forkNameR_bad)
        {
            isInfork_固定銷R = true;
            fork固定銷CurrentName = other.name;
        }
        if (other.name == forkNameL_good || other.name == forkNameL_bad)
        {
            isInfork_固定銷L = true;
            fork固定銷CurrentName = other.name;
        }

        //油門
        if (other.name == GasPadel_Nor.name)
        {
            Debug.Log("22222222222222222222223434");

            isInGasPadel = true;
        }
        
    }

    bool isInBeTakeObj=false;
    GameObject BeTakingObj;
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CanBeHandTakeObj>() != null)
        {
            if (isPushHandTrig)
            {
                BeTakingObj = other.gameObject;
                isInBeTakeObj = true;
            }
            else
            {
                isInBeTakeObj = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //離合
        if (other.name == CluthPadelName_good)
        {
            isInCluthPadel_Nor = false;
        }
        if (other.name == CluthPadelName_bad)
        {
            isInCluthPadel_abNor = false;
        }
        //剎車
        if (other.name.Contains(BrakePadelName_good))
        {
            isInBrakePadel_Nor = false;
        }
        if (other.name.Contains(BrakePadelName_bad))
        {
            isInBrakePadel_abNor = false;
        }
        //方向盤
        if (other.name.Contains(WheelNormalKeyWord))
        {
            isInWheel_nor = false;
        }
        if (other.name.Contains(WheelBreakKeyWord))
        {
            isInWheel_ab = false;
        }
        //手煞
        if (other.name == HandBrakePadelName_good)
        {
            isInHandBrakePadel_Nor = false;
        }
        if (other.name == HandBrakePadelName_bad)
        {
            isInHandBrakePadel_abNor = false;
        }
        //安全帶
        if (other.name == SafeBaltName_good)
        {
            isInSafeBalt_Nor = false;
        }
        if (other.name == SafeBaltName_bad)
        {
            isInSafeBalt_abNor = false;
        }
        //螺絲
        if (other.name.Contains(ScrewBreakKeyWord))
        {
            isInScrewBreak = false;
        }
        if (other.name.Contains(ScrewNormalWord))
        {
            isInScrewNormal = false;
        }
        //喇吧
        if (other.name.Contains(HronNormalKeyWord))
        {
            isInHorn_nor = false;
        }
        if (other.name.Contains(HronBreakKeyWord))
        {
            isInHorn_ab = false;
        }
        //鑰匙
        if (other.name == Key_onCar.name)
        {
            //isKey_onCar = false;
        }
        if (other.name == ControlRight_右控制桿.name)
        {
            isInControlRight_右控制桿 = false;
        }

        //大燈/////////////////////////////////////////////////
        if (other.name == Control_大燈拉桿Name_good)
        {
            isIn大燈拉桿_nor = false;
        }
        if (other.name == Control_大燈拉桿Name_bad)
        {
            isIn大燈拉桿_ab = false;

        }


        if (other.name == ControlLeft_左控制桿.name)
        {
            isInControlLeft_左控制桿 = false;
        }
        if (other.name == forkNameR_good || other.name == forkNameR_bad)
        {
            isInfork_固定銷R = false;
            fork固定銷CurrentName = other.name;
        }
        if (other.name == forkNameL_good || other.name == forkNameL_bad)
        {
            isInfork_固定銷L = false;
            fork固定銷CurrentName = other.name;
        }
        if (other.GetComponent<CanBeHandTakeObj>() != null)
        {
            isInBeTakeObj = false;

        }

        //油門
        if (other.name == GasPadel_Nor.name)
        {
            isInGasPadel = false;
        }
    }
}
