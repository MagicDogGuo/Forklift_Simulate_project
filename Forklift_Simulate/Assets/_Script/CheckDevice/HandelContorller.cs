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

    AudioSource _audioSourse;

    float pushDegree = -20;
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
    float oriTemp_HandBrake = 0;
    GameObject TipObj_NormalHandBrakePadel;
    GameObject TipObj_abNormalHandBrakePadel;

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
    bool isKey_onTable = false;
    bool isKey_onCar = false;
    GameObject Key_onCar;

    //右控制桿
    string ControlRight_右控制桿Name_Nor;
    string ControlRight_右控制桿Name_ab_BigLight;
    string ControlRight_右控制桿Name_ab_DirtLight_R;
    string ControlRight_右控制桿Name_ab_DirtLight_L;
    bool isInControlRight_右控制桿;
    GameObject ControlRight_右控制桿;
    GameObject TipChooseCnavasObj;

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

    void Start()
    {
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

        ScrewBreakKeyWord = checkDeviceManager.tire.BreakScrewName;
        ScrewNormalWord = checkDeviceManager.tire.NormalScrewName;

        HronBreakKeyWord = checkDeviceManager.wheels.BreakHronName;
        HronNormalKeyWord = checkDeviceManager.wheels.NormalHornName;

        Key_onCar = checkDeviceManager.dashbroad.key_onCar;
        Key_onCar.GetComponent<MeshRenderer>().enabled =false;

        ControlRight_右控制桿Name_Nor = checkDeviceManager.carLight.NormalControlRight_右控制桿Name;
        ControlRight_右控制桿Name_ab_BigLight = checkDeviceManager.carLight.ControlLeft_右控制桿_ab_BigLightName;
        ControlRight_右控制桿Name_ab_DirtLight_R = checkDeviceManager.carLight.ControlLeft_右控制桿_ab_DirtLight_RName;
        ControlRight_右控制桿Name_ab_DirtLight_L = checkDeviceManager.carLight.ControlLeft_右控制桿_ab_DirtLight_LName;
        ControlRight_右控制桿 = checkDeviceManager.carLight.ControlRight_右控制桿;

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

        Debug.Log(this.name + " isInGasPade:" + isInGasPadel);

        PadelControl(CluthPadel_Nor, CluthPadel_abNor,isInCluthPadel_Nor, isInCluthPadel_abNor, "C");
        PadelControl(BrakePadel_Nor, BrakePadel_abNor, isInBrakePadel_Nor, isInBrakePadel_abNor, "B");
        PadelControl(HandBrakePadel_Nor, HandBrakePadel_abNor, isInHandBrakePadel_Nor, isInHandBrakePadel_abNor, "H");
        PadelControl(GasPadel_Nor, null, isInGasPadel, false, "G");//額外油門
        WheelControl();
        ScrewControl();
        HornControl();
        ControlRight_右控制桿Control();
        ControlLeft_左控制桿Control();
        Controlfork_固定銷();
        GetKeyOnTable();
        GiveKeyToCar();
        GetCanBeTakeObj();

 
  
    }

    [SerializeField]
    Vector3 offsetTip = new Vector3(1, 1, 0);


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
                    checkDeviceManager.ControlLight(true, "Brake_Off");
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

            }

            padel_Nor.transform.localEulerAngles = new Vector3(oriTemp_Brake, 0, 0);

        }

        void HandBrakeMove()
        {
            if (isInPadel_Nor && isPushHandTrig)
            {
                //手煞紅點
                padel_Nor.transform.GetChild(0).transform.localPosition = new Vector3(0.0043f, 0, 0.2819f);

                oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, pushDegree, 70f * Time.deltaTime);
            }
            else
            {
                //手煞紅點
                padel_Nor.transform.GetChild(0).transform.localPosition = new Vector3(0.004561948f, 0, 0.2845623f);

                oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, 0, 70f * Time.deltaTime);
            }
            padel_Nor.transform.localEulerAngles = new Vector3(0, -oriTemp_HandBrake, 180);

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
            padel_Nor.transform.localEulerAngles = new Vector3(0, -oriTemp_GasPadel+30, 90);
        }

        //異常
        void CluthBreakTip()
        {       
            if (isInPadel_abNor && isPushHandTrig)
            {
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
                Destroy(TipObj_abNormalCluthPadel);
            }        
        }
        void BrakeTip()
        {
            if (isInPadel_abNor && isPushHandTrig)
            {
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
                Destroy(TipObj_abNormalBrakePadel);
            }
        }
        void HandBrakeBreakTip()
        {
            if (isInPadel_abNor && isPushHandTrig)
            {
                if (TipObj_abNormalHandBrakePadel == null)
                {
                    //TipObj_abNormalHandBrakePadel = Instantiate(TipUIObj, padel_abNor.transform);
                    TipObj_abNormalHandBrakePadel = Instantiate(TipUIObj, this.transform);
                    TipObj_abNormalHandBrakePadel.GetComponentInChildren<Text>().text = "煞車拉柄卡住!";
                    TipObj_abNormalHandBrakePadel.transform.localPosition = TipUIOffset;//new Vector3(0.04f, 0.05f, 0.4f);
                    //TipObj_abNormalHandBrakePadel.transform.localEulerAngles = new Vector3(0, 0, 130);

                }
                if (TipObj_abNormalHandBrakePadel != null)
                {
                    TipObj_abNormalHandBrakePadel.transform.LookAt(GameObject.Find("TipUILookTraget").transform);
                }

            }
            else
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

    void ScrewControl()
    {
        if (BrakeScrew != null)
        {
            if (isInScrewBreak && isPushHandTrig)
            {
                if (TipObj_BrakeScrew == null)
                {
                    //TipObj_BrakeScrew = Instantiate(TipUIObj, BrakeScrew.transform);
                    TipObj_BrakeScrew = Instantiate(TipUIObj, this.transform);
                    TipObj_BrakeScrew.GetComponentInChildren<Text>().text = "螺絲鬆脫!";
                    TipObj_BrakeScrew.transform.localPosition = TipUIOffset;// new Vector3(-0.05f, -.25f, 0);
                }
                if (TipObj_BrakeScrew != null)
                    TipObj_BrakeScrew.transform.LookAt(GameObject.Find("TipUILookTraget").transform);

            }
            else 
            {
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
                    TipObj_NormalScrew.transform.LookAt(GameObject.Find("TipUILookTraget").transform);
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
                Hron.transform.localPosition = new Vector3(0, 0, 0.0085f);
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
                Hron.transform.localPosition = new Vector3(0, 0, 0.0085f);
            }

            if(isPushHandTrig == false)
            {
                Hron.transform.localPosition = new Vector3(0, 0, 0.0112192f);
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
                btn[0].GetComponentInChildren<Text>().text = "右方向燈開";
                btn[1].GetComponentInChildren<Text>().text = "左方向燈開";
                btn[2].GetComponentInChildren<Text>().text = "方向燈關";
                btn[3].GetComponentInChildren<Text>().text = "前燈開";
                btn[4].GetComponentInChildren<Text>().text = "前燈關";

                //正常
                
                btn[0].onClick.AddListener(() => checkDeviceManager.ControlLight(true, "R_On"));
                btn[1].onClick.AddListener(() => checkDeviceManager.ControlLight(true, "L_On"));
                btn[2].onClick.AddListener(() => checkDeviceManager.ControlLight(true, "Dirct_Off"));
                btn[3].onClick.AddListener(() => checkDeviceManager.ControlLight(true, "Big_On"));
                btn[4].onClick.AddListener(() => checkDeviceManager.ControlLight(true, "Big_Off"));
                
                //大燈異常
                if (ControlRight_右控制桿.name.Contains(ControlRight_右控制桿Name_ab_BigLight))
                {
                    btn[3].onClick.RemoveAllListeners();
                    btn[4].onClick.RemoveAllListeners();
                }
                //右燈異常
                if (ControlRight_右控制桿.name.Contains(ControlRight_右控制桿Name_ab_DirtLight_R))
                {
                    btn[0].onClick.RemoveAllListeners();

                }
                //左燈異常
                if (ControlRight_右控制桿.name.Contains(ControlRight_右控制桿Name_ab_DirtLight_L))
                {
                    btn[1].onClick.RemoveAllListeners();

                }
            }
        }
    }

    public int ControlLeft_左控制桿TriggerCount=0;
    public bool isCount_左控制桿 = false;
    void ControlLeft_左控制桿Control()
    {
        if (isPushHandTrig == false)
        {
            isCount_左控制桿 = false;

            //其他把手同步
            foreach (var handCon in handelContorllers)
            {
                if (handCon.gameObject != this.gameObject)
                {
                    handCon.ControlLeft_左控制桿TriggerCount = ControlLeft_左控制桿TriggerCount;
                    handCon.isCount_左控制桿 = isCount_左控制桿;
                }
            }
        }
        if (isInControlLeft_左控制桿 && isPushHandTrig && isCount_左控制桿==false)
        {
            ControlLeft_左控制桿TriggerCount++;
            isCount_左控制桿 = true; 
            
            //其他把手同步
            foreach (var handCon in handelContorllers)
            {
                if (handCon.gameObject != this.gameObject)
                {
                    handCon.ControlLeft_左控制桿TriggerCount = ControlLeft_左控制桿TriggerCount;
                    handCon.isCount_左控制桿 = isCount_左控制桿;
                }
            }
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
                checkDeviceManager.ControlLight(true, "RevLight_On_BreakLight");//////////////////////
            }
            //倒車警示異常
            if (ControlLeft_左控制桿.name.Contains(ControlLeft_左控制桿Name_ab_Sound))
            {
                checkDeviceManager.ControlLight(true, "RevLight_On_BreakSound");////////////////////////////
            }
        }
        else
        {//控制桿固定關

            oriTempRotate_ControlLeft = Mathf.MoveTowards(oriTempRotate_ControlLeft, 0, 70f * Time.deltaTime);
            checkDeviceManager.ControlLight(true, "RevLight_Off");
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
        if (isInKey_onTable && isPushHandTrig && isKey_onTable == true)
        {
            isKey_onTable = false;
            isKey_onHand = true;
            isKey_onCar = false;
            Key_onHand.SetActive(true);
            Key_onTable.SetActive(false);
        }
    }
    void GiveKeyToCar()
    {
        if (isInKey_onCar && isPushHandTrig && isKey_onHand == true)
        {
            isKey_onTable = false;
            isKey_onHand = false;
            isKey_onCar = true;
            Key_onHand.SetActive(false);
            Key_onCar.GetComponent<MeshRenderer>().enabled = true;


            if (checkDeviceManager.dashbroad.OnKeyPlugIn != null)
            {
                checkDeviceManager.dashbroad.OnKeyPlugIn();
                Key_onHand.SetActive(false);
            }
        }
    }

    void GetCanBeTakeObj()
    {
        if (isInBeTakeObj == true && isPushHandTrig)
        {
            Debug.Log("==============引擎機油尺=");
           // BeTakingObj.transform.position = this.transform.position + BeTakingObj.GetComponent<CanBeHandTakeObj>().OffsetPos;
            //BeTakingObj.transform.localEulerAngles = new Vector3(BeTakingObj.GetComponent<CanBeHandTakeObj>().oriRota.x, 
            //BeTakingObj.GetComponent<CanBeHandTakeObj>().oriRota.y,
            //this.transform.localEulerAngles.z);
            BeTakingObj.transform.SetParent(this.transform.gameObject.transform);
        }
        else
        {
            if (BeTakingObj != null)
            {
                isInBeTakeObj = false;/////////////////////////////////////
                BeTakingObj.transform.SetParent(BeTakingObj.GetComponent<CanBeHandTakeObj>().oriPareant.transform);
                BeTakingObj.transform.localPosition = BeTakingObj.GetComponent<CanBeHandTakeObj>().oriPos;
                BeTakingObj.transform.localEulerAngles = BeTakingObj.GetComponent<CanBeHandTakeObj>().oriRota;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
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
        }
        if (other.name == Key_onTable.name )
        {
            isInKey_onTable = true;
            isInKey_onHand = false;
            isInKey_onCar = false;
        }
        //右控制感(燈號)
        if (other.name == ControlRight_右控制桿.name)
        {
            isInControlRight_右控制桿 = true;
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
            //isKey_onTable = false;
            //isKey_onHand = false;
            //isKey_onCar = false;
        }
        if (other.name == ControlRight_右控制桿.name)
        {
            isInControlRight_右控制桿 = false;
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
