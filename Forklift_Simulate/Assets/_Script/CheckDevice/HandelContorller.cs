using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandelContorller : MonoBehaviour
{
    public bool isPushHandTrig;

    [SerializeField]
    GameObject TipUIObj;
    [SerializeField]
    GameObject TipChooseUIObj;
    [SerializeField]
    AudioClip HornSound;
    [SerializeField]
    GameObject keyOnHand;

    AudioSource _audioSourse;


    float pushDegree = -20;
    float unPushDegree = 0;

    CheckDeviceManager checkDeviceManager;

    string CluthPadelName_good;
    string CluthPadelName_bad;
    bool isInCluthPadel;
    GameObject CluthPadel;
    float oriTemp_Cluth = 0;

    string BrakePadelName_good;
    string BrakePadelName_bad;
    bool isInBrakePadel;
    GameObject BrakePadel;
    float oriTemp_Brake = 0;

    string HandBrakePadelName_good;
    string HandBrakePadelName_bad;
    bool isInHandBrakePadel;
    GameObject HandBrakePadel;
    float oriTemp_HandBrake = 0;

    string ScrewBreakKeyWord;
    bool isInScrewBreak;
    GameObject BrakeScrew;
    GameObject TipObj;

    string HronBreakKeyWord;
    string HronNormalKeyWord;
    bool isInHorn;
    GameObject Hron;

    bool isInKey;
    GameObject Key;

    bool isInControlRight_右控制桿;
    GameObject ControlRight_右控制桿;
    GameObject TipChooseObj;

    void Start()
    {
        checkDeviceManager = GameObject.FindObjectOfType<CheckDeviceManager>();
        _audioSourse = this.GetComponent<AudioSource>();

        isInCluthPadel = false;
        isInBrakePadel = false;

        CluthPadelName_good = checkDeviceManager.clutch.goodObjName;
        CluthPadelName_bad = checkDeviceManager.clutch.badObjName;

        BrakePadelName_good = checkDeviceManager.brake.goodObjName;
        BrakePadelName_bad = checkDeviceManager.brake.badObjName;

        HandBrakePadelName_good = checkDeviceManager.handBrake.goodObjName;
        HandBrakePadelName_bad = checkDeviceManager.handBrake.badObjName;

        ScrewBreakKeyWord = checkDeviceManager.tire.BreakScrewName;

        HronBreakKeyWord = checkDeviceManager.wheels.BreakHronName;
        HronNormalKeyWord = checkDeviceManager.wheels.NormalHornName;

        Key = checkDeviceManager.dashbroad.key;
        ControlRight_右控制桿 = checkDeviceManager.carLight.ControlRight_右控制桿;
    }

    void Update()
    {
        PadelControl(CluthPadel, isInCluthPadel,"C");
        PadelControl(BrakePadel, isInBrakePadel, "B");
        PadelControl(HandBrakePadel, isInHandBrakePadel, "H");
        BreakScrewControl();
        HornControl();
        ControlRight_右控制桿Control();
    }
    

    void PadelControl(GameObject padel,bool isInPadel,string type)
    {
        if (padel != null)
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
            }      
        }


        void CluthMove()
        {
            if (isInPadel && isPushHandTrig)
            {
                oriTemp_Cluth = Mathf.MoveTowards(oriTemp_Cluth, pushDegree, 70f * Time.deltaTime);
            }
            else
            {
                oriTemp_Cluth = Mathf.MoveTowards(oriTemp_Cluth, 0, 70f * Time.deltaTime);
            }
            padel.transform.localEulerAngles = new Vector3(oriTemp_Cluth, 0, 0);

        }

        void BrakeMove()
        {
            if (isInPadel && isPushHandTrig)
            {
                oriTemp_Brake = Mathf.MoveTowards(oriTemp_Brake, pushDegree, 70f * Time.deltaTime);
            }
            else
            {
                oriTemp_Brake = Mathf.MoveTowards(oriTemp_Brake, 0, 70f * Time.deltaTime);
            }
            padel.transform.localEulerAngles = new Vector3(oriTemp_Brake, 0, 0);

        }

        void HandBrakeMove()
        {
            if (isInPadel && isPushHandTrig)
            {
                oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, pushDegree, 70f * Time.deltaTime);
            }
            else
            {
                oriTemp_HandBrake = Mathf.MoveTowards(oriTemp_HandBrake, 0, 70f * Time.deltaTime);
            }
            padel.transform.localEulerAngles = new Vector3(0, -oriTemp_HandBrake, 0);

        }
    }


    void BreakScrewControl()
    {
        if (BrakeScrew != null)
        {
            if (isInScrewBreak && isPushHandTrig)
            {
                if (TipObj == null)
                {
                    TipObj = Instantiate(TipUIObj, BrakeScrew.transform);
                    TipObj.GetComponentInChildren<Text>().text = "螺絲鬆脫!";
                }   
            }
            else
            { 
                Destroy(TipObj);
            }
        }
    }
  

    void HornControl()
    {
        if (Hron != null)
        {
            if (isInHorn && isPushHandTrig)
            {
                _audioSourse.clip = HornSound;

                if (!_audioSourse.isPlaying)
                {
                    _audioSourse.Stop();
                    _audioSourse.Play();
                }
            }
            else
            {
                _audioSourse.Stop();
            }
        }
    }

    void ControlRight_右控制桿Control()
    {
        if (isInControlRight_右控制桿 && isPushHandTrig)
        {
            if (TipChooseObj == null)
            {
                TipChooseObj = Instantiate(TipChooseUIObj, ControlRight_右控制桿.transform);
                TipChooseObj.transform.localRotation = new Quaternion(0, 180, 0,0);
                Button[] btn = TipChooseObj.GetComponent<TipChooseUI_CheckDevice>().stateBtn;
                btn[0].GetComponentInChildren<Text>().text = "右方向燈開";
                btn[1].GetComponentInChildren<Text>().text = "左方向燈開";
                btn[2].GetComponentInChildren<Text>().text = "方向燈關";
                btn[3].GetComponentInChildren<Text>().text = "大燈開";
                btn[4].GetComponentInChildren<Text>().text = "大燈關";

                btn[0].onClick.AddListener(() => checkDeviceManager.ControlLight(true, "R_On"));
                btn[1].onClick.AddListener(() => checkDeviceManager.ControlLight(true, "L_On"));
                btn[2].onClick.AddListener(() => checkDeviceManager.ControlLight(true, "Dirct_Off"));
                btn[3].onClick.AddListener(() => checkDeviceManager.ControlLight(true, "Big_On"));
                btn[4].onClick.AddListener(() => checkDeviceManager.ControlLight(true, "Big_Off"));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == CluthPadelName_good)
        {
            isInCluthPadel = true;
            CluthPadel = other.gameObject;
        }

        if (other.name == BrakePadelName_good)
        {
            isInBrakePadel = true;
            BrakePadel = other.gameObject;
        }
        if (other.name == HandBrakePadelName_good)
        {
            isInHandBrakePadel = true;
            HandBrakePadel = other.gameObject;
        }
        if (other.name.Contains(ScrewBreakKeyWord))
        {
            isInScrewBreak = true;
            BrakeScrew = other.gameObject;
        }
        if (other.name.Contains(HronNormalKeyWord))
        {
            isInHorn = true;
            Hron = other.gameObject;
        }
        if (other.name == Key.name)
        {
            isInKey = true;
            if (checkDeviceManager.dashbroad.OnKeyPlugIn != null)
            {
                checkDeviceManager.dashbroad.OnKeyPlugIn();
                keyOnHand.SetActive(false);
            }
        }
        if (other.name == ControlRight_右控制桿.name)
        {
            isInControlRight_右控制桿 = true;
        }

     }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == CluthPadelName_good)
        {
            isInCluthPadel = false;
        }
        if (other.name == BrakePadelName_good)
        {
            isInBrakePadel = false;
        }
        if (other.name == HandBrakePadelName_good)
        {
            isInHandBrakePadel = false;
        }
        if (other.name.Contains(ScrewBreakKeyWord))
        {
            isInScrewBreak = false;
        }
        if (other.name.Contains(HronNormalKeyWord))
        {
            isInHorn = false;
        }
        if (other.name == Key.name)
        {
            isInKey = false;
        }
        if (other.name == ControlRight_右控制桿.name)
        {
            isInControlRight_右控制桿 = false;
        }
    }
}
