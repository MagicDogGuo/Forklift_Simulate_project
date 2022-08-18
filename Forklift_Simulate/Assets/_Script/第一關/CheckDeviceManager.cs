using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WSMGameStudio.Vehicles;
using VRTK.Controllables.ArtificialBased;
using VRTK.GrabAttachMechanics;
using VRTK;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckDeviceManager : MonoBehaviour
{
    [SerializeField]
    WSMVehicleController _wSMVehicleController;

    [SerializeField]
    Cold_冷卻液 cold_冷卻液;

    [SerializeField]
    EngineOil engineOil;

    [SerializeField]
    BatteryWater batteryWater;

    [SerializeField]
    BrakeOil brakeOil;

    [SerializeField]
    WaterOil_液壓油 waterOil_液壓油;

    [SerializeField]
    public Clutch clutch;

    [SerializeField]
    public Brake brake;

    [SerializeField]
    public Wheels wheels;

    [SerializeField]
    public HandBrake handBrake;

    [SerializeField]
    public SafeBalt safeBalt;


    [SerializeField]
    public Tire tire;

    [SerializeField]
    public Dashbroad dashbroad;

    [SerializeField]
    public CarLight carLight;

    [SerializeField]
    public AlertLight_工作警示燈 alertLight_工作警示燈;

    [SerializeField]
    public IronPipe_白鐵蕊 ironPipe;

    [SerializeField]
    public Fork_固定銷 fork_固定銷;

    [Header("損壞部位隨機範圍")]
    [SerializeField]
    int MaxPart = 21;
    [SerializeField]
    int MinPart = 1;

    [Header("額外非測驗題目")]
    [SerializeField]
    public GameObject GasPadel;

    [SerializeField]
    Text TestTxt;

    [Header("初始UI")]
    [SerializeField]
    GameObject InitUICanvas;
    [SerializeField]
    GameObject CameraPCorVRController;

    static public GameMode_firstStage gameMode_FirstStage;
    static public bool isLetEngineOn;

    public enum GameMode_firstStage
    {
        TestMode,
        PracticeMode
    }

    Dictionary<int, List<int>> _CorrectAnswerDict;
    public Dictionary<int, List<int>> CorrectAnswerDict
    {
        get { return _CorrectAnswerDict; }
    }

    Dictionary<int, List<int>> _BreakDeviceInCorrectAnswerDict;
    public Dictionary<int, List<int>> BreakDeviceInCorrectAnswerDict
    {
        get { return _BreakDeviceInCorrectAnswerDict; }
    }


    /// <summary>
    /// 特別判斷鑰匙是否啟動引擎、超時、鑰匙歸位、安全帶歸位、大燈歸位、方向燈歸位、手煞歸位、後退檔歸位
    /// </summary>
    bool _isKeyRot90Degree;
    public bool IsKeyRot90Degree
    {
        get {return _isKeyRot90Degree; }
    }


    bool _isOverTime;
    public bool IsOverTime
    {
        get { return _isOverTime; }
    }

    bool _isNoBackKey;
    public bool IsNoBackKey
    {
        set { _isNoBackKey = value; }
        get { return _isNoBackKey; }
    }

    bool _isNoBacSetbelt;
    public bool IsNoBackSetbelt
    {
        set { _isNoBacSetbelt = value; }
        get { return _isNoBacSetbelt; }
    }

    bool _isNoBackBigLight;
    public bool IsNoBackBigLight
    {
        set { _isNoBackBigLight = value; }
        get { return _isNoBackBigLight; }
    }

    bool _isNoBackDirLight;
    public bool IsNoBackDirLight
    {
        set { _isNoBackDirLight = value; }
        get { return _isNoBackDirLight; }
    }

    bool _isNoBackHandBrake;
    public bool IsNoBackHandBrake
    {
        set { _isNoBackHandBrake = value; }
        get { return _isNoBackHandBrake; }
    }

    bool _isNoBackFrontBack;
    public bool IsNoBackFrontBack
    {
        set { _isNoBackFrontBack = value; }
        get { return _isNoBackFrontBack; }
    }

    
    /// <summary>
    /// 需要檢查的物件編號
    /// </summary>
    public enum DevicePart
    {
        Cold_冷卻液 = 1,
        EngineOil = 2,
        BatteryWater = 3,
        BrakeOil = 4,
        WaterOil_液壓油 = 5,
        Clutch = 6,
        Brake = 7,
        HandBrake = 8,
        SafeBalt = 9,
        Tire = 10,
        Screw = 11,
        Horn = 12,
        Dashbroad = 13,
        CarLight_BigLight = 14,
        CarLight_DirctLight = 15,
        CarLight_BrakeLight = 16,
        CarLight_RearLight_Alert = 17,
        AlertLight_工作警示燈 = 18,
        ironPipe = 19,
        fork_固定銷 = 20,
        鑰匙轉90度 = 99//直接失敗
    }

    private void Awake()
    {
        InitUIControl();
    }


    void InitUIControl()
    {
        isLetEngineOn = false;

        InitUICanvas.SetActive(true);
        CameraPCorVRController.SetActive(false);

        GameEventSystem.Instance.OnPushTestModeBtn = null;
        GameEventSystem.Instance.OnPushPracticeModeBtn = null;
        GameEventSystem.Instance.OnPushTestModeBtn += TestMode;
        GameEventSystem.Instance.OnPushPracticeModeBtn += PracticeMode;

    }

    void TestMode()
    {
        gameMode_FirstStage = GameMode_firstStage.TestMode;
        InitUICanvas.SetActive(false);
        CameraPCorVRController.SetActive(true);
    }

    void PracticeMode()
    {
        gameMode_FirstStage = GameMode_firstStage.PracticeMode;
        InitUICanvas.SetActive(false);
        CameraPCorVRController.SetActive(true);
    }

    void Start()
    {
        _CorrectAnswerDict = new Dictionary<int, List<int>>();
        _BreakDeviceInCorrectAnswerDict = new Dictionary<int, List<int>>();

        InitDeviceDict();

        StartCoroutine(IEInitLight());

        DecideObjectState();

        //BrakeALL();

        _isKeyRot90Degree = false;

        Invoke("DelayRigibodyIsKinemect", 2);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            SceneManager.LoadScene("TitleState");
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (KeyValuePair<int, List<int>> kvp in _CorrectAnswerDict)
            {
                string tmpeS = "";

                foreach (var i in kvp.Value)
                {
                    tmpeS += i + ",";
                }
                Debug.Log("CorrectAnswerDict: Key = " + (DevicePart)kvp.Key + " Value =" + tmpeS);
            }
              
        }

        //方向燈閃爍
        if (isTriggerR_Light)
        {
            DircLightToggle("R");
        }
        if (isTriggerL_Light)
        {
            DircLightToggle("L");
        }
        if (isTriggerR_Light_Break)
        {
            DircLightToggle("R_Front_Break");

        }
        if (isTriggerL_Light_Break)
        {
            DircLightToggle("L_Front_Break");

        }

    }

    void DelayRigibodyIsKinemect()
    {
        _wSMVehicleController.GetComponent<Rigidbody>().isKinematic = true;
    }


    void InitDeviceDict()
    {
        List<int> correctAnswer = new List<int>();
        correctAnswer.Add(1);
        for (int i = (int)DevicePart.Cold_冷卻液; i <= (int)DevicePart.fork_固定銷; i++)
        {
            _CorrectAnswerDict.Add(i, correctAnswer);
        }  
    }

    IEnumerator IEInitLight()
    {
        AudioSource[] audioChild = _wSMVehicleController.GetComponentsInChildren<AudioSource>();
        foreach (var s in audioChild)
        {
            s.enabled = false;
        }
        yield return new WaitForEndOfFrame();
        _wSMVehicleController.IsEngineOn = true;
        ControlLight(true, "RevLight_Off");
        ControlLight(true, "Dirct_Off");
        ControlLight(true, "Big_Off");
        ControlLight(true, "Brake_Off");
        yield return new WaitForEndOfFrame();
        _wSMVehicleController.IsEngineOn = false;
        foreach (var s in audioChild)
        {
            s.enabled = true;
        }
    }

    /// <summary>
    /// 測試用，全異常
    /// </summary>
    void BrakeALL()
    {
        int tmep = 0;

        cold_冷卻液.goodObj.SetActive(false);
        cold_冷卻液.badObj.SetActive(true);

        engineOil.goodObj.SetActive(false);
        engineOil.badObj.SetActive(true);

        batteryWater.goodObj.SetActive(false);
        batteryWater.badObj.SetActive(true);

        brakeOil.goodObj.SetActive(false);
        brakeOil.badObj.SetActive(true);

        waterOil_液壓油.goodObj.SetActive(false);
        waterOil_液壓油.badObj.SetActive(true);

        clutch.Obj.name = clutch.badObjName;

        brake.Obj.name = brake.goodObjName;

        handBrake.Obj.name = handBrake.badObjName;

        safeBalt.Obj.name = safeBalt.badObjName;

        TireRandomBreak(out tmep);

        ScrewRandomBreak(out tmep);

        HronBreak();

        dashbroad.OnKeyPlugIn_NoSound = null;
        dashbroad.OnKeyPlugIn_NoSound += DashBoardRandomBreak_key_NoSound;

        dashbroad.OnKeyPlugIn_HaveSound = null;
        dashbroad.OnKeyPlugIn_HaveSound += DashBoardRandomBreak_key_HaveSound;

        dashbroad.OnKeyPlugOut = null;
        dashbroad.OnKeyPlugOut += DashBoardPlugOut;


        carLight.Control_大燈拉桿.name = carLight.Control_大燈拉桿_badObjName;
        carLight.ControlRight_右控制桿.name += carLight.ControlLeft_右控制桿_ab_DirtLight_LName;
        carLight.ControlRight_右控制桿.name += carLight.ControlLeft_右控制桿_ab_DirtLight_RName;

        //剎車燈
        brake.Obj.name += brake.breakRearLightName;

        carLight.ControlLeft_左控制桿.name += carLight.ControlLeft_左控制桿_ab_LightName;
        carLight.ControlLeft_左控制桿.name += carLight.ControlLeft_左控制桿_ab_SoundName;

        alertLight_工作警示燈.goodObj.SetActive(false);
        alertLight_工作警示燈.goodObj_燈罩.SetActive(false);
        alertLight_工作警示燈.badObj.SetActive(true);
        alertLight_工作警示燈.badObj_燈罩.SetActive(true);

        ironPipe.goodObj.SetActive(false);
        ironPipe.badObj.SetActive(true);

        Fork_固定銷Bad();
    }

    /// <summary>
    /// 全對
    /// </summary>
    void GoodDeviceALL()
    {
        DisableBad();
        cold_冷卻液.goodObj.SetActive(true);
        engineOil.goodObj.SetActive(true);
        batteryWater.goodObj.SetActive(true);
        brakeOil.goodObj.SetActive(true);
        waterOil_液壓油.goodObj.SetActive(true);
        clutch.Obj.name = clutch.goodObjName;
        brake.Obj.name = brake.goodObjName;
        WheelGood();
        handBrake.Obj.name = handBrake.goodObjName;
        safeBalt.Obj.name = safeBalt.goodObjName;
        TireGood();
        ScrewGood();
        HronGood();
        dashbroad.key_onCar.GetComponent<MeshRenderer>().enabled = false;
        dashbroad.OnKeyPlugIn_NoSound += DashBoardGood_key_NoSound;
        dashbroad.OnKeyPlugIn_HaveSound += DashBoardGood_key_HaveSound;
        dashbroad.OnKeyPlugOut += DashBoardPlugOut;


        carLight.ControlRight_右控制桿.name = carLight.NormalControlRight_右控制桿Name;
        carLight.ControlLeft_左控制桿.name = carLight.NormalControlLeft_左控制桿Name;

        carLight.Control_大燈拉桿.name = carLight.Control_大燈拉桿_goodObjName;
        
        alertLight_工作警示燈.goodObj.SetActive(true);
        alertLight_工作警示燈.goodObj_燈罩.SetActive(true);
        alertLight_工作警示燈.badObj.SetActive(false);
        alertLight_工作警示燈.badObj_燈罩.SetActive(false);


        ironPipe.goodObj.SetActive(true);
        ironPipe.badObj.SetActive(false);
        Fork_固定銷Good();
    }

    void DisableBad()
    {
        cold_冷卻液.badObj.SetActive(false);
        engineOil.badObj.SetActive(false);
        batteryWater.badObj.SetActive(false);
        brakeOil.badObj.SetActive(false);
        waterOil_液壓油.badObj.SetActive(false);
    }

    int breakDashBoradNumber = 0;
    /// <summary>
    /// 選出一個位置壞掉
    /// </summary>
    /// <param name="devicePart"></param>
    void ChooseBreakDevice(DevicePart devicePart)
    {
        List<int> tempBreakpartNumber = new List<int>();
        tempBreakpartNumber.Clear();

        switch (devicePart)
        {
            case DevicePart.Cold_冷卻液:
                tempBreakpartNumber.Add(4);//過低
                cold_冷卻液.goodObj.SetActive(false);
                cold_冷卻液.badObj.SetActive(true);
                break;
            case DevicePart.EngineOil:
                tempBreakpartNumber.Add(4);//過低
                engineOil.goodObj.SetActive(false);
                engineOil.badObj.SetActive(true);
                break;
            case DevicePart.BatteryWater:
                int[] input = {3};//,4,5,6,7,8
                tempBreakpartNumber.AddRange(input);///////////////////////有1-8(複選題，只有1會錯，正極負極不會錯)
                batteryWater.goodObj.SetActive(false);
                batteryWater.badObj.SetActive(true);
                break;
            case DevicePart.BrakeOil:
                tempBreakpartNumber.Add(4);//過低
                brakeOil.goodObj.SetActive(false);
                brakeOil.badObj.SetActive(true);
                break;
            case DevicePart.WaterOil_液壓油:
                tempBreakpartNumber.Add(4);//過低
                waterOil_液壓油.goodObj.SetActive(false);
                waterOil_液壓油.badObj.SetActive(true);
                break;
            case DevicePart.Clutch:
                tempBreakpartNumber.Add(2);
                clutch.Obj.name = clutch.badObjName;
                break;
            case DevicePart.Brake:
                tempBreakpartNumber.Add(2);
                brake.Obj.name = brake.badObjName;
                break;
            case DevicePart.HandBrake:
                tempBreakpartNumber.Add(2);
                //WheelBreak();
                handBrake.Obj.name = handBrake.badObjName;
                handBrake.Obj.transform.localEulerAngles = new Vector3(0, 40, 180);//損壞位置
                break;
            case DevicePart.SafeBalt:
                tempBreakpartNumber.Add(2);
                safeBalt.Obj.name = safeBalt.badObjName;
                break;
            case DevicePart.Tire:
                int temp1 = 0;
                TireRandomBreak(out temp1);
                tempBreakpartNumber.Add(temp1);
                break;
            case DevicePart.Screw:
                int temp2 = 0;
                ScrewRandomBreak(out temp2);
                tempBreakpartNumber.Add(temp2);

                break;
            case DevicePart.Horn:
                tempBreakpartNumber.Add(2);
                HronBreak();
                break;
            case DevicePart.Dashbroad:
                breakDashBoradNumber = Random.Range(1, 3);//1=充電燈 , 2=油燈
                tempBreakpartNumber.Add(breakDashBoradNumber+2); //2開始，因為1=正常,2=異常
                dashbroad.OnKeyPlugIn_NoSound = null;
                dashbroad.OnKeyPlugIn_NoSound += DashBoardRandomBreak_key_NoSound;
                dashbroad.OnKeyPlugIn_HaveSound = null;
                dashbroad.OnKeyPlugIn_HaveSound += DashBoardRandomBreak_key_HaveSound;
                dashbroad.OnKeyPlugOut = null;
                dashbroad.OnKeyPlugOut += DashBoardPlugOut;
                break;
            case DevicePart.CarLight_BigLight:
                tempBreakpartNumber.Add(3);//壞左燈
                carLight.Control_大燈拉桿.name = carLight.Control_大燈拉桿_badObjName;
                break;
            case DevicePart.CarLight_DirctLight:
                //固定壞左側方向燈(前後)
                //int[] input5 = { 3, 4 };
                //tempBreakpartNumber.AddRange(input5);//左側
                tempBreakpartNumber.Add(3);//壞左前燈

                carLight.ControlRight_右控制桿.name += carLight.ControlLeft_右控制桿_ab_DirtLight_LName;
                //carLight.ControlRight_右控制桿.name += carLight.ControlLeft_右控制桿_ab_DirtLight_RName;
                break;
            case DevicePart.CarLight_BrakeLight:
                tempBreakpartNumber.Add(3);//壞左燈
                brake.Obj.name += brake.breakRearLightName;
                break;
            case DevicePart.CarLight_RearLight_Alert:
                tempBreakpartNumber.Add(4);//壞右燈
                carLight.ControlLeft_左控制桿.name += carLight.ControlLeft_左控制桿_ab_LightName;
                //carLight.ControlLeft_左控制桿.name += carLight.ControlLeft_左控制桿_ab_SoundName;
                break;
            case DevicePart.AlertLight_工作警示燈:
                tempBreakpartNumber.Add(2);
                alertLight_工作警示燈.goodObj.SetActive(false);
                alertLight_工作警示燈.goodObj_燈罩.SetActive(false);
                alertLight_工作警示燈.badObj.SetActive(true);
                alertLight_工作警示燈.badObj_燈罩.SetActive(true);
                break;

            case DevicePart.ironPipe:
                tempBreakpartNumber.Add(3);//只壞升降
                ironPipe.goodObj.SetActive(false);
                ironPipe.badObj.SetActive(true);
                break;
            case DevicePart.fork_固定銷:
                tempBreakpartNumber.Add(4);//只壞右邊
                Fork_固定銷Bad();
                break;
        }
        //Debug.Log("breakDevice: " + devicePart + "===tempBreakpartNumber: " + tempBreakpartNumber);

        if (_CorrectAnswerDict.ContainsKey((int)devicePart))
        {
            _CorrectAnswerDict[(int)devicePart] = tempBreakpartNumber;
        }
        else
        {
            _CorrectAnswerDict.Add((int)devicePart, tempBreakpartNumber);
        }
        //Debug.Log("========(int)devicePart=" + (int)devicePart);
        //有壞掉的部位再另外存
        _BreakDeviceInCorrectAnswerDict.Add((int)devicePart, tempBreakpartNumber);
    }

    void DecideObjectState()
    {
        GoodDeviceALL();

        DevicePart[] breakObjs = MyRondom(5);/////////////////////////////
        string s = "[BreakPart]: ";

        foreach(var dv in breakObjs)
        {
            s += dv+",";
            ChooseBreakDevice(dv);
        }
        Debug.Log(s);
        TestTxt.text = s;
    }

    /// <summary>
    ///隨機出不相同的數
    /// </summary>
    /// <param name="breakDeviceAmount"></param>
    /// <returns></returns>
    DevicePart[]  MyRondom( int breakDeviceAmount)
    {
        List<DevicePart> devicePartList = new List<DevicePart>();
        int brakepart = 0;
        int brakeNo = (int)DevicePart.Brake;//腳剎車不能壞(才能測倒退檔)

        while (devicePartList.Count < breakDeviceAmount )
        {
            brakepart = Random.Range(MinPart, MaxPart);

            if (devicePartList.Contains((DevicePart)brakepart) || brakepart == brakeNo)
            {
                continue;
            }
            devicePartList.Add((DevicePart)brakepart);
        }


        DevicePart[] devicePartArray = devicePartList.ToArray();

        return devicePartArray;

    }


    void WheelGood()
    {
        wheels.WheelObj.name = wheels.NormalWheelName;
    }
    void WheelBreak()
    {
        wheels.WheelObj.name = wheels.BreakWheelName;
        Destroy(wheels.WheelObj.transform.parent.GetComponentInParent<VRTK_InteractableObject>());
        Destroy(wheels.WheelObj.GetComponentInParent<VRTK_ArtificialRotator>());
    }

    void TireGood()
    {
        foreach(var _tire in tire.goodObj_Tire)
        {
            _tire.SetActive(true);
        }
        foreach (var _tire in tire.badObj_Tire)
        {
            _tire.SetActive(false);
        }
    }
    void TireRandomBreak(out int breakPart)
    {
        int brakeTire = Random.Range(0, 4);
        breakPart = brakeTire + 3;//+3是因為第二階段由3開始

        tire.badObj_Tire[brakeTire].SetActive(true);
        tire.goodObj_Tire[brakeTire].SetActive(false);
        Debug.Log("TireBrake: " + tire.badObj_Tire[brakeTire].name);
    }
    void ScrewGood()
    {
        //好壞輪胎裡的螺絲都換成
        foreach(var sc in tire.Screw)
        {
            sc.name = tire.NormalScrewName;
        }
        foreach (var sc in tire.ScrewInBrakeTire)
        {
            sc.name = tire.NormalScrewName;
        }
    }
    void ScrewRandomBreak(out int breakPart)
    {
        int brakeScrew = Random.Range(0, 4);////////////////////////////
        breakPart = brakeScrew + 3;//+3是因為第二階段由3開始

        //好壞輪胎裡的螺絲都換成壞的
        tire.Screw[brakeScrew].name = tire.BreakScrewName;
        tire.ScrewInBrakeTire[brakeScrew].name = tire.BreakScrewName;

        Debug.Log("ScrewBrake: " + tire.Screw[brakeScrew].name);
    }


    void HronGood()
    {
        wheels.Horn.name = wheels.NormalHornName;
    }
    void HronBreak()
    {
        wheels.Horn.name = wheels.BreakHronName;
    }

    void DashBoardGood_key_NoSound()
    {
        _wSMVehicleController.IsEngineOn = true;
        _wSMVehicleController.engineStartSFX.volume = 0;
        _wSMVehicleController.engineSFX.volume = 0;

        dashbroad.key_onCar.GetComponent<MeshRenderer>().enabled = true;
        dashbroad.OffDashBoard.SetActive(false);
        dashbroad.goodObj.SetActive(true);
        dashbroad.badObj_ChargeLight.SetActive(false);
        dashbroad.badObj_EngineOilLight.SetActive(false);
        dashbroad.badObj_WaterLight.SetActive(false);
    }

    void DashBoardGood_key_HaveSound()
    {
        _isKeyRot90Degree = true;
        _wSMVehicleController.IsEngineOn = true;
        _wSMVehicleController.engineStartSFX.Play();
        _wSMVehicleController.engineStartSFX.volume = 1;
        _wSMVehicleController.engineSFX.volume = 0.2f;

        dashbroad.key_onCar.GetComponent<MeshRenderer>().enabled = true;
        dashbroad.OffDashBoard.SetActive(false);
        dashbroad.goodObj.SetActive(true);
        dashbroad.badObj_ChargeLight.SetActive(false);
        dashbroad.badObj_EngineOilLight.SetActive(false);
        dashbroad.badObj_WaterLight.SetActive(false);
    }

    void DashBoardRandomBreak_key_NoSound()
    {
        _wSMVehicleController.IsEngineOn = true;
        _wSMVehicleController.engineStartSFX.volume = 0;
        _wSMVehicleController.engineSFX.volume = 0;


        dashbroad.key_onCar.GetComponent<MeshRenderer>().enabled = true;
        dashbroad.OffDashBoard.SetActive(false);
        dashbroad.goodObj.SetActive(false);

        switch (breakDashBoradNumber)
        {
            case 1:
                dashbroad.badObj_ChargeLight.SetActive(true);
                break;
            case 2:
                dashbroad.badObj_EngineOilLight.SetActive(true);
                break;
                //case 3: //答案裡沒有可以選他的
                //    dashbroad.badObj_WaterLight.SetActive(true);
                //    break;
        }
    }

    void DashBoardRandomBreak_key_HaveSound()
    {
        _isKeyRot90Degree = true;
        _wSMVehicleController.IsEngineOn = true;
        _wSMVehicleController.engineStartSFX.Play();
        _wSMVehicleController.engineStartSFX.volume = 1;
        _wSMVehicleController.engineSFX.volume = 0.2f;


        dashbroad.key_onCar.GetComponent<MeshRenderer>().enabled = true;
        dashbroad.OffDashBoard.SetActive(false);
        dashbroad.goodObj.SetActive(false);
    
        switch (breakDashBoradNumber)
        {
            case 1:
                dashbroad.badObj_ChargeLight.SetActive(true);
                break;
            case 2:
                dashbroad.badObj_EngineOilLight.SetActive(true);
                break;
            //case 3: //答案裡沒有可以選他的
            //    dashbroad.badObj_WaterLight.SetActive(true);
            //    break;
        }
    }

    void DashBoardPlugOut()
    {
        _wSMVehicleController.IsEngineOn = false;
        dashbroad.OffDashBoard.SetActive(true);
        dashbroad.goodObj.SetActive(false);
        dashbroad.badObj_ChargeLight.SetActive(false);
        dashbroad.badObj_EngineOilLight.SetActive(false);
        dashbroad.badObj_WaterLight.SetActive(false);
    }

    void OffDashBoard_Key()
    {
        _wSMVehicleController.IsEngineOn = false;
        dashbroad.OnKeyPlugIn_HaveSound = null;
        dashbroad.OnKeyPlugIn_NoSound = null;
        dashbroad.OnKeyPlugOut = null;
        dashbroad.key_onCar.GetComponent<MeshRenderer>().enabled = false;
        dashbroad.OffDashBoard.SetActive(true);
        dashbroad.goodObj.SetActive(false);
        dashbroad.badObj_ChargeLight.SetActive(false);
        dashbroad.badObj_EngineOilLight.SetActive(false);
        dashbroad.badObj_WaterLight.SetActive(false);
    }


    bool isTriggerR_Light = false;
    bool isTriggerL_Light = false;

    bool isTriggerR_Light_Break = false;
    bool isTriggerL_Light_Break = false;

    float countTime = 0;
    void DircLightToggle(string DirectWay)
    {
        float lightBlinkSpeed = 0.5f;

        countTime += 1 * Time.deltaTime;
        if (countTime >= lightBlinkSpeed)
        {
            countTime = 0;
            if (DirectWay == "R")
            {
                //閃爍用
                carLight.Light_右方向燈_Object005.SetActive(!carLight.Light_右方向燈_Object005.active);
                carLight.Light_右前方向燈_Object005.SetActive(!carLight.Light_右前方向燈_Object005.active);
                carLight.unLight_右方向燈_Object005.SetActive(!carLight.unLight_右方向燈_Object005.active);
                carLight.unLight_右前方向燈_Object005.SetActive(!carLight.unLight_右前方向燈_Object005.active);

                //關閉另一側燈用
                carLight.Light_左方向燈_Object005.SetActive(false);
                carLight.Light_左前方向燈_Object005.SetActive(false);
                carLight.unLight_左方向燈_Object005.SetActive(true);
                carLight.unLight_左前方向燈_Object005.SetActive(true);

            }
            if(DirectWay == "L")
            {
                carLight.Light_左方向燈_Object005.SetActive(!carLight.Light_左方向燈_Object005.active);
                carLight.Light_左前方向燈_Object005.SetActive(!carLight.Light_左前方向燈_Object005.active);
                carLight.unLight_左方向燈_Object005.SetActive(!carLight.unLight_左方向燈_Object005.active);
                carLight.unLight_左前方向燈_Object005.SetActive(!carLight.unLight_左前方向燈_Object005.active);

                carLight.Light_右方向燈_Object005.SetActive(false);
                carLight.Light_右前方向燈_Object005.SetActive(false);
                carLight.unLight_右方向燈_Object005.SetActive(true);
                carLight.unLight_右前方向燈_Object005.SetActive(true);
            }
            if (DirectWay == "R_Front_Break")
            {
                //carLight.Light_右方向燈_Object005.SetActive(!carLight.Light_右方向燈_Object005.active);
                //carLight.unLight_右方向燈_Object005.SetActive(!carLight.unLight_右方向燈_Object005.active);

                //carLight.Light_左方向燈_Object005.SetActive(false);
                //carLight.Light_左前方向燈_Object005.SetActive(false);
                //carLight.unLight_左方向燈_Object005.SetActive(false);
                //carLight.unLight_左前方向燈_Object005.SetActive(true);
            }
            if (DirectWay == "L_Front_Break")
            {
                carLight.Light_左方向燈_Object005.SetActive(!carLight.Light_左方向燈_Object005.active);
                carLight.unLight_左方向燈_Object005.SetActive(!carLight.unLight_左方向燈_Object005.active);

                carLight.Light_右方向燈_Object005.SetActive(false);
                carLight.Light_右前方向燈_Object005.SetActive(false);
                carLight.unLight_右方向燈_Object005.SetActive(true);
                carLight.unLight_右前方向燈_Object005.SetActive(true);

            }
          
        }  
        
        if(DirectWay == "Stop")
        {
            carLight.Light_左方向燈_Object005.SetActive(false);
            carLight.Light_左前方向燈_Object005.SetActive(false);
            carLight.unLight_左方向燈_Object005.SetActive(true);
            carLight.unLight_左前方向燈_Object005.SetActive(true);

            carLight.Light_右方向燈_Object005.SetActive(false);
            carLight.Light_右前方向燈_Object005.SetActive(false);
            carLight.unLight_右方向燈_Object005.SetActive(true);
            carLight.unLight_右前方向燈_Object005.SetActive(true);

        }
    }


    public void ControlLight(bool isGood, string LighType)
    {
        if (isGood)//引擎要開才有燈
        {
            if (LighType == "R_On")
            {
                carLight.ControlRight_右控制桿.transform.GetChild(0).localEulerAngles = new Vector3(-11, 185, -62);
                _wSMVehicleController.RightSinalLightsOn = true;
                _wSMVehicleController.CurrenLightControl(1);

                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                isTriggerL_Light = false;
                isTriggerR_Light = true;
                isTriggerR_Light_Break = false;
                isTriggerL_Light_Break = false;
            }
            if (LighType == "L_On")
            {
                carLight.ControlRight_右控制桿.transform.GetChild(0).localEulerAngles = new Vector3(11, 142, -62);
                _wSMVehicleController.LeftSinalLightsOn = true;
                _wSMVehicleController.CurrenLightControl(-1);

                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                isTriggerR_Light = false;
                isTriggerL_Light = true;
                isTriggerR_Light_Break = false;
                isTriggerL_Light_Break = false;

            }
            if (LighType == "Dirct_Off")
            {
                carLight.ControlRight_右控制桿.transform.GetChild(0).localEulerAngles = new Vector3(0, 165, -60);

                _wSMVehicleController.RightSinalLightsOn = false;
                _wSMVehicleController.LeftSinalLightsOn = false;
                _wSMVehicleController.CurrenLightControl(0);

                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                isTriggerR_Light = false;
                isTriggerL_Light = false;
                isTriggerR_Light_Break = false;
                isTriggerL_Light_Break = false;
                DircLightToggle("Stop");
            }
            if (LighType == "Big_On")
            {////////////////////////////////////////////////
                ////////carLight.ControlRight_右控制桿.transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(0, 0, -42);
                carLight.Control_大燈拉桿.transform.localPosition = new Vector3(-0.01001f, -4.844665e-08f, -0.00616f);

                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                _wSMVehicleController.HeadlightsOn = true;
            }
            if (LighType == "Big_Off")
            {//////////////////////////////////////////////////////////
             //carLight.ControlRight_右控制桿.transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
                carLight.Control_大燈拉桿.transform.localPosition = new Vector3(-0.006068428f, -4.844665e-08f, -0.003881158f);

                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                _wSMVehicleController.HeadlightsOn = false;
            }
            if (LighType == "Brake_On")
            {
                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                carLight.Light_右剎車燈_Object008.SetActive(true);
                carLight.Light_左剎車燈_Object008.SetActive(true);
                carLight.unLight_右剎車燈_Object008.SetActive(false);
                carLight.unLight_左剎車燈_Object008.SetActive(false);


                _wSMVehicleController._brakes = 2;    //沒有指定燈
                //_wSMVehicleController._movingBackwards = true;
            }
            if (LighType == "Brake_Off")
            {
                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                carLight.Light_右剎車燈_Object008.SetActive(false);
                carLight.Light_左剎車燈_Object008.SetActive(false);
                carLight.unLight_右剎車燈_Object008.SetActive(true);
                carLight.unLight_左剎車燈_Object008.SetActive(true);


                _wSMVehicleController._brakes = 0;    //沒有指定燈
                // _wSMVehicleController._movingBackwards = false;
            }
            if (LighType == "RevLight_On")
            {
                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                carLight.Light_右倒車燈_Box076.SetActive(true);
                carLight.Light_左倒車燈_Box076.SetActive(true);
                carLight.unLight_右倒車燈_Box076.SetActive(false);
                carLight.unLight_左倒車燈_Box076.SetActive(false);

                _wSMVehicleController._movingBackwards = true;
            }
            if (LighType == "RevLight_On_BreakLight")
            {
                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                //固定壞右邊
                carLight.Light_右倒車燈_Box076.SetActive(false);
                carLight.Light_左倒車燈_Box076.SetActive(true);
                carLight.unLight_右倒車燈_Box076.SetActive(true);
                carLight.unLight_左倒車燈_Box076.SetActive(false);

                Debug.Log("================================dsdsds");


                _wSMVehicleController._movingBackwards = true;
                _wSMVehicleController.reverseAlarmLights[0].intensity = 0;//關閉燈號，可以設定成固定壞某邊
                _wSMVehicleController.reverseAlarmLights[1].intensity = 3;//關閉燈號，可以設定成固定壞某邊
                //_wSMVehicleController.reverseAlarmLights_Materials[0].DisableKeyword("_EMISSION");// = Color.black;//關閉燈號
                //_wSMVehicleController.reverseAlarmLights_Materials[1].DisableKeyword("_EMISSION");//.color = Color.black;//關閉燈號
                //_wSMVehicleController.reverseAlarmLights_Materials[2].DisableKeyword("_EMISSION");// = Color.black;//關閉燈號
                //_wSMVehicleController.reverseAlarmLights_Materials[3].DisableKeyword("_EMISSION");//.color = Color.black;//關閉燈號
            }
            if (LighType == "RevLight_On_BreakSound")
            {
                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                _wSMVehicleController._movingBackwards = true;
                _wSMVehicleController.backUpBeeperSFX = null;//關閉聲音
            }
            if (LighType == "RevLight_Off")
            {
                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                carLight.Light_右倒車燈_Box076.SetActive(false);
                carLight.Light_左倒車燈_Box076.SetActive(false);
                carLight.unLight_右倒車燈_Box076.SetActive(true);
                carLight.unLight_左倒車燈_Box076.SetActive(true);

                _wSMVehicleController._movingBackwards = false;
            }
        }
        if (!isGood)
        {
            if (LighType == "R_On")
            {
                carLight.ControlRight_右控制桿.transform.GetChild(0).localEulerAngles = new Vector3(-11, 185, -62);
                _wSMVehicleController.RightSinalLightsOn = true;
                _wSMVehicleController.CurrenLightControl(1);

                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                isTriggerL_Light = false;
                isTriggerR_Light = true;
                isTriggerR_Light_Break = false;
                isTriggerL_Light_Break = false;
            }
            if (LighType == "L_On")
            {
                ///固定左前燈壞掉
                carLight.ControlRight_右控制桿.transform.GetChild(0).localEulerAngles = new Vector3(11, 142, -62);
                _wSMVehicleController.LeftSinalLightsOn = true;
                _wSMVehicleController.CurrenLightControl(-1);

                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                isTriggerR_Light = false;
                isTriggerL_Light = false;
                isTriggerR_Light_Break = false;
                isTriggerL_Light_Break = true;
            }
            if (LighType == "Big_On")
            {
                //固定壞左側
                //////////////// carLight.ControlRight_右控制桿.transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(0, 0, -42);
                ////////
                ///
                carLight.Control_大燈拉桿.transform.localPosition = new Vector3(-0.01001f, -4.844665e-08f, -0.00616f);

                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                carLight.Light_右大燈.SetActive(true);
                carLight.Light_左大燈.SetActive(false);
                carLight.unLight_右大燈.SetActive(false);
                carLight.unLight_左大燈.SetActive(true);

                _wSMVehicleController.HeadlightsOn = true;
                _wSMVehicleController.headlights[0].GetComponent<Light>().intensity = 0;
            }
            if (LighType == "Big_Off")
            {
                ///carLight.ControlRight_右控制桿.transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(0, 0, 0);
                //////////////////////////
                ///
                carLight.Control_大燈拉桿.transform.localPosition = new Vector3(-0.006068428f, -4.844665e-08f, -0.003881158f);

                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                carLight.Light_右大燈.SetActive(false);
                carLight.Light_左大燈.SetActive(false);
                carLight.unLight_右大燈.SetActive(true);
                carLight.unLight_左大燈.SetActive(true);

                _wSMVehicleController.HeadlightsOn = false;
                _wSMVehicleController.headlights[0].GetComponent<Light>().intensity = 3;

            }

            if (LighType == "Brake_On")
            {
                _wSMVehicleController.brakeLights[0].gameObject.SetActive(false);

                //左煞車燈壞
                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                carLight.Light_右剎車燈_Object008.SetActive(true);
                carLight.Light_左剎車燈_Object008.SetActive(false);
                carLight.unLight_右剎車燈_Object008.SetActive(false);
                carLight.unLight_左剎車燈_Object008.SetActive(true);


                _wSMVehicleController._brakes = 2;    //沒有指定燈
                //_wSMVehicleController._movingBackwards = true;
            }
            if (LighType == "Brake_Off")
            {
                if (!_wSMVehicleController.CurrentEngineOn) return;//要開引擎
                carLight.Light_右剎車燈_Object008.SetActive(false);
                carLight.Light_左剎車燈_Object008.SetActive(false);
                carLight.unLight_右剎車燈_Object008.SetActive(true);
                carLight.unLight_左剎車燈_Object008.SetActive(true);

                //StartCoroutine(DelayLight());
               // _wSMVehicleController.brakeLights[0].gameObject.SetActive(true);

                //_wSMVehicleController._brakes = 0;    //沒有指定燈
                // _wSMVehicleController._movingBackwards = false;
            }
        }
    }


    void Fork_固定銷Good()
    {
        fork_固定銷.Fork_固定銷R.name = fork_固定銷.NormalFork_固定銷RName;
        fork_固定銷.Fork_固定銷L.name = fork_固定銷.NormalFork_固定銷LName;
    }
    void Fork_固定銷Bad()
    {
        fork_固定銷.Fork_固定銷R.name = fork_固定銷.BreakFork_固定銷RName;
        fork_固定銷.Fork_固定銷R.transform.GetChild(0).gameObject.SetActive(false);
        //沒有坐左邊錯誤的
    }


    public void isOverTime(bool s)
    {
        _isOverTime = s;
    }

}
[System.Serializable]
public class Cold_冷卻液
{
    [SerializeField]
    public GameObject goodObj;
    [SerializeField]
    public GameObject badObj;
}

[System.Serializable]
public class EngineOil
{
    [SerializeField]
    public GameObject goodObj;
    [SerializeField]
    public GameObject badObj;
}

[System.Serializable]
public class BatteryWater
{
    [SerializeField]
    public GameObject goodObj;
    [SerializeField]
    public GameObject badObj;
}
[System.Serializable]
public class BrakeOil
{
    [SerializeField]
    public GameObject goodObj;
    [SerializeField]
    public GameObject badObj;
}

[System.Serializable]
public class WaterOil_液壓油
{
    [SerializeField]
    public GameObject goodObj;
    [SerializeField]
    public GameObject badObj;
}

[System.Serializable]
public class Clutch
{
    [SerializeField]
    public GameObject Obj;
    [SerializeField]
    public string goodObjName = "CluthPadel_nor";
    [SerializeField]
    public string badObjName="CluthPadel_ab";
}


[System.Serializable]
public class Brake
{
    [SerializeField]
    public GameObject Obj;
    [SerializeField]
    public string goodObjName = "BrakePadel_nor";
    [SerializeField]
    public string badObjName= "BrakePadel_ab";
    [SerializeField]
    public string breakRearLightName = "BreakRearLight";
}


[System.Serializable]
public class Wheels
{
    [SerializeField]
    public GameObject WheelObj;
    [SerializeField]
    public string BreakWheelName = "_abWheel";
    [SerializeField]
    public string NormalWheelName = "_normalWheel";

    [SerializeField]
    public GameObject Horn;
    [SerializeField]
    public string BreakHronName = "_abHorn";
    [SerializeField]
    public string NormalHornName = "_normalHorn";
}

[System.Serializable]
public class HandBrake
{
    [SerializeField]
    public GameObject Obj;
    [SerializeField]
    public string goodObjName= "HandBrekePipe_nor";
    [SerializeField]
    public string badObjName= "HandBrekePipe_ab";
}

[System.Serializable]
public class SafeBalt
{
    [SerializeField]
    public GameObject Obj;
    [SerializeField]
    public string goodObjName = "SafeBalt_nor";
    [SerializeField]
    public string badObjName = "SafeBalt_ab";
}



[System.Serializable]
public class Tire
{
    [SerializeField]
    public GameObject[] goodObj_Tire;
    [SerializeField]
    public GameObject[] badObj_Tire;

    [SerializeField]
    public GameObject[] Screw;
    [SerializeField]
    public GameObject[] ScrewInBrakeTire;
    [SerializeField]
    public string NormalScrewName = "_norScrew";
    [SerializeField]
    public string BreakScrewName = "_abScrew";
}

[System.Serializable]
public class Dashbroad
{
    public GameObject key_onCar;
    [HideInInspector]
    public UnityAction OnKeyPlugIn_NoSound;
    [HideInInspector]
    public UnityAction OnKeyPlugIn_HaveSound;
    [HideInInspector]
    public UnityAction OnKeyPlugOut;


    [SerializeField]
    public GameObject OffDashBoard;
    [SerializeField]
    public GameObject goodObj;
    [SerializeField]
    public GameObject badObj_ChargeLight;
    [SerializeField]
    public GameObject badObj_EngineOilLight;
    [SerializeField]
    public GameObject badObj_WaterLight;
}

[System.Serializable]
public class CarLight
{
    [SerializeField]
    public GameObject ControlRight_右控制桿;
    [SerializeField]
    public string NormalControlRight_右控制桿Name = "_norControlRight_右控制桿";
    //[SerializeField]
    //public string ControlLeft_右控制桿_ab_BigLightName = "ControlLeft_右控制桿_ab_BigLight";
    [SerializeField]
    public string ControlLeft_右控制桿_ab_DirtLight_RName= "ControlLeft_右控制桿_ab_DirtLight_R";
    [SerializeField]
    public string ControlLeft_右控制桿_ab_DirtLight_LName= "ControlLeft_右控制桿_ab_DirtLight_L";


    [SerializeField]
    public GameObject ControlLeft_左控制桿;
    [SerializeField]
    public string NormalControlLeft_左控制桿Name = "_norControlLeft_左控制桿";
    [SerializeField]
    public string ControlLeft_左控制桿_ab_LightName = "ControlLeft_左控制桿_ab_Light";
    [SerializeField]
    public string ControlLeft_左控制桿_ab_SoundName = "ControlLeft_左控制桿_ab_Sound";

    [Header("大燈拉桿")]
    public GameObject Control_大燈拉桿;
    [SerializeField]
    public string Control_大燈拉桿_goodObjName = "ControlLeft_右控制桿_nor_BigLight";
    [SerializeField]
    public string Control_大燈拉桿_badObjName = "ControlLeft_右控制桿_ab_BigLight";

    [Header("有/無發光燈泡")]
    public GameObject unLight_左方向燈_Object005;
    public GameObject unLight_右方向燈_Object005;
    public GameObject unLight_左前方向燈_Object005;
    public GameObject unLight_右前方向燈_Object005;
    public GameObject unLight_左剎車燈_Object008;
    public GameObject unLight_右剎車燈_Object008;
    public GameObject unLight_左倒車燈_Box076;
    public GameObject unLight_右倒車燈_Box076;
    public GameObject unLight_左大燈;
    public GameObject unLight_右大燈;


    public GameObject Light_左方向燈_Object005;
    public GameObject Light_右方向燈_Object005;
    public GameObject Light_左前方向燈_Object005;
    public GameObject Light_右前方向燈_Object005;
    public GameObject Light_左剎車燈_Object008;
    public GameObject Light_右剎車燈_Object008;
    public GameObject Light_左倒車燈_Box076;
    public GameObject Light_右倒車燈_Box076;
    public GameObject Light_左大燈;
    public GameObject Light_右大燈;


    //[SerializeField]
    //public GameObject goodObj_BigLight;
    //[SerializeField]
    //public GameObject badOb_BigLightj;

    //[SerializeField]
    //public GameObject goodObj_DirtLight;
    //[SerializeField]
    //public GameObject badObj_DirtLight;

    //[SerializeField]
    //public GameObject goodObj_BrakeLight;
    //[SerializeField]
    //public GameObject badObj_BrakeLight;

    //[SerializeField]
    //public GameObject goodObj_BackLight;
    //[SerializeField]
    //public GameObject badObj_BackLight;
}

[System.Serializable]
public class WarmingSound
{
    [SerializeField]
    public GameObject goodObj;
    [SerializeField]
    public GameObject badObj;
}

[System.Serializable]
public class AlertLight_工作警示燈
{
    [SerializeField]
    public GameObject goodObj;
    [SerializeField]
    public GameObject goodObj_燈罩;
    [SerializeField]
    public GameObject badObj;
    [SerializeField]
    public GameObject badObj_燈罩;
}


[System.Serializable]
public class IronPipe_白鐵蕊
{
    [SerializeField]
    public GameObject goodObj;
    [SerializeField]
    public GameObject badObj;
}

[System.Serializable]
public class Fork_固定銷
{
    [SerializeField]
    public GameObject Fork_固定銷R;
    [SerializeField]
    public string BreakFork_固定銷RName = "abNormal_固定銷R_Group039";
    [SerializeField]
    public string NormalFork_固定銷RName = "Normal_固定銷R_Group039";

    [SerializeField]
    public GameObject Fork_固定銷L;
    [SerializeField]
    public string BreakFork_固定銷LName = "abNormal_固定銷L_Group039";
    [SerializeField]
    public string NormalFork_固定銷LName = "Normal_固定銷L_Group039";
}