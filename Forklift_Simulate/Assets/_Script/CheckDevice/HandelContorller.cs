using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandelContorller : MonoBehaviour
{
    public bool isPushHandTrig;//把手的開關

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

    bool isKey_onHand;
    bool isKey_onTable;
    bool isKey_onCar;
    GameObject Key_onCar;



    bool isInControlRight_右控制桿;
    GameObject ControlRight_右控制桿;
    GameObject TipChooseObj;

    bool isInControlLeft_左控制桿;
    GameObject ControlLeft_左控制桿;
    float oriTemp_ControlLeft = 0;

    string forkNameR_good;
    string forkNameR_bad;
    bool isInfork_固定銷R;
    GameObject fork_固定銷R;
    string forkNameL_good;
    string forkNameL_bad;
    bool isInfork_固定銷L;
    GameObject fork_固定銷L;

    string fork固定銷CurrentName;

    void Start()
    {
         Key_onHand.SetActive(false);


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

        Key_onCar = checkDeviceManager.dashbroad.key_onCar;
        ControlRight_右控制桿 = checkDeviceManager.carLight.ControlRight_右控制桿;

        ControlLeft_左控制桿 = checkDeviceManager.carLight.ControlLeft_左控制桿;

        forkNameR_good = checkDeviceManager.fork_固定銷.NormalFork_固定銷RName;
        forkNameR_bad = checkDeviceManager.fork_固定銷.BreakFork_固定銷RName;
        fork_固定銷R = checkDeviceManager.fork_固定銷.Fork_固定銷R;

        forkNameL_good = checkDeviceManager.fork_固定銷.NormalFork_固定銷LName;
        forkNameL_bad = checkDeviceManager.fork_固定銷.BreakFork_固定銷LName;
        fork_固定銷L = checkDeviceManager.fork_固定銷.Fork_固定銷L;

    }

    void Update()
    {
        PadelControl(CluthPadel, isInCluthPadel, "C");
        PadelControl(BrakePadel, isInBrakePadel, "B");
        PadelControl(HandBrakePadel, isInHandBrakePadel, "H");
        BreakScrewControl();
        HornControl();
        ControlRight_右控制桿Control();
        ControlLeft_左控制桿Control();
        Controlfork_固定銷();
        GetKeyOnTable();
    }


    void PadelControl(GameObject padel, bool isInPadel, string type)
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
                checkDeviceManager.ControlLight(true, "Brake_On");
            }
            else
            {
                oriTemp_Brake = Mathf.MoveTowards(oriTemp_Brake, 0, 70f * Time.deltaTime);
                checkDeviceManager.ControlLight(true, "Brake_Off");

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
                TipChooseObj.transform.localRotation = new Quaternion(0, 180, 0, 0);
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

    void ControlLeft_左控制桿Control()
    {
        if (isInControlLeft_左控制桿 && isPushHandTrig)
        {
            //Debug.Log("在左");
            oriTemp_ControlLeft = Mathf.MoveTowards(oriTemp_ControlLeft, pushDegree, 70f * Time.deltaTime);
            checkDeviceManager.ControlLight(true, "RevLight_On");
        }
        else
        {
            //Debug.Log("沒在左");

            oriTemp_ControlLeft = Mathf.MoveTowards(oriTemp_ControlLeft, 0, 70f * Time.deltaTime);
            checkDeviceManager.ControlLight(true, "RevLight_Off");

        }
        ControlLeft_左控制桿.transform.localEulerAngles = new Vector3(0, 0, -oriTemp_ControlLeft);
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
        if (isKey_onTable && isPushHandTrig)
        {
            Key_onHand.SetActive(true);
            Key_onTable.SetActive(false);

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
        if (other.name == Key_onCar.name)
        {
            isKey_onTable = false;
            isKey_onHand = false;
            isKey_onCar = true;

            if (checkDeviceManager.dashbroad.OnKeyPlugIn != null)
            {
                checkDeviceManager.dashbroad.OnKeyPlugIn();
                Key_onHand.SetActive(false);
            }
        }
        if (other.name == Key_onTable.name)
        {
            isKey_onTable = true;
            isKey_onHand = false;
            isKey_onCar = false;


        }
        if (other.name == ControlRight_右控制桿.name)
        {
            isInControlRight_右控制桿 = true;
        }
        if (other.name == ControlLeft_左控制桿.name)
        {
            isInControlLeft_左控制桿 = true;
        }
        if(other.name == forkNameR_good|| other.name == forkNameR_bad)
        {
            isInfork_固定銷R = true;
            fork固定銷CurrentName = other.name;
        }
        if (other.name == forkNameL_good || other.name == forkNameL_bad)
        {
            isInfork_固定銷L = true;
            fork固定銷CurrentName = other.name;
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
    }
}
