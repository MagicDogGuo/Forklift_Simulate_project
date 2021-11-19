using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WSMGameStudio.Vehicles;

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
    public Tire tire;

    [SerializeField]
    public Dashbroad dashbroad;

    [SerializeField]
    public CarLight carLight;

    [SerializeField]
    public IronPipe_白鐵蕊 ironPipe;

    [SerializeField]
    public Fork_固定銷 fork_固定銷;

    [Header("額外非測驗題目")]
    [SerializeField]
    public GameObject GasPadel;

    enum DevicePart
    {
        Cold_冷卻液 = 1,
        EngineOil = 2,
        BatteryWater = 3,
        BrakeOil=4,
        WaterOil_液壓油=5,
        Clutch = 6,
        Brake = 7,
        Wheels = 8,////////還沒做
        HandBrake = 9,
        Tire = 10,
        Screw = 11,
        Horn = 12,
        Dashbroad = 13,
        CarLight_BigLight = 14,
        CarLight_DirctLight = 15,
        CarLight_BrakeLight = 16,
        CarLight_RearLight = 17,
        RearMoveAlert = 18,
        ironPipe = 19,
        fork_固定銷 = 20
    }

    private void Awake()
    {

    }

    void Start()
    {
        StartCoroutine(IEInitLight());

        DecideObjectState();

        //BrakeALL();
    }

    IEnumerator IEInitLight()
    {
        AudioSource[] audioChild = _wSMVehicleController.GetComponentsInChildren<AudioSource>();
        foreach(var s in audioChild)
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
        //brake.Obj.name = brake.badObjName;

        handBrake.Obj.name = handBrake.badObjName;
      
        TireRandomBreak();
       
        ScrewRandomBreak();
      
        HronBreak();
        
        dashbroad.OnKeyPlugIn = null;
        dashbroad.OnKeyPlugIn += DashBoardRandomBreak_key;

        carLight.ControlRight_右控制桿.name += carLight.ControlLeft_右控制桿_ab_BigLightName;
        carLight.ControlRight_右控制桿.name += carLight.ControlLeft_右控制桿_ab_DirtLight_LName;
        carLight.ControlRight_右控制桿.name += carLight.ControlLeft_右控制桿_ab_DirtLight_RName;
        
        //剎車燈
        brake.Obj.name += brake.breakRearLightName;

        carLight.ControlLeft_左控制桿.name += carLight.ControlLeft_左控制桿_ab_LightName;
        carLight.ControlLeft_左控制桿.name += carLight.ControlLeft_左控制桿_ab_SoundName;

        ironPipe.goodObj.SetActive(false);
        ironPipe.badObj.SetActive(true);

        Fork_固定銷Bad();
        
    }

    void DisableBad()
    {
        cold_冷卻液.badObj.SetActive(false);
        engineOil.badObj.SetActive(false);
        batteryWater.badObj.SetActive(false);
        brakeOil.badObj.SetActive(false);
        waterOil_液壓油.badObj.SetActive(false);
    }

    void DecideObjectState()
    {
        DisableBad();
        cold_冷卻液.goodObj.SetActive(true);
        engineOil.goodObj.SetActive(true);
        batteryWater.goodObj.SetActive(true);
        brakeOil.goodObj.SetActive(true);
        waterOil_液壓油.goodObj.SetActive(true);
        clutch.Obj.name = clutch.goodObjName;
        brake.Obj.name = brake.goodObjName;
        handBrake.Obj.name = handBrake.goodObjName;
        TireGood();
        ScrewGood();
        HronGood();
        dashbroad.key_onCar.GetComponent<MeshRenderer>().enabled = false;
        dashbroad.OnKeyPlugIn += DashBoardGood_key;

        carLight.ControlRight_右控制桿.name = carLight.NormalControlRight_右控制桿Name;
        carLight.ControlLeft_左控制桿.name = carLight.NormalControlLeft_左控制桿Name;

        ironPipe.goodObj.SetActive(true);
        Fork_固定銷Good();

        DevicePart breakObj01 = MyRondom();
        Debug.Log(breakObj01);
        switch (breakObj01)
        {        
            case DevicePart.Cold_冷卻液:
                cold_冷卻液.goodObj.SetActive(false);
                cold_冷卻液.badObj.SetActive(true);
                break;
            case DevicePart.EngineOil:
                engineOil.goodObj.SetActive(false);
                engineOil.badObj.SetActive(true);
                break;
            case DevicePart.BatteryWater:
                batteryWater.goodObj.SetActive(false);
                batteryWater.badObj.SetActive(true);
                break;
            case DevicePart.BrakeOil:
                brakeOil.goodObj.SetActive(false);
                brakeOil.badObj.SetActive(true);
                break;
            case DevicePart.WaterOil_液壓油:
                waterOil_液壓油.goodObj.SetActive(false);
                waterOil_液壓油.badObj.SetActive(true);
                break;
            case DevicePart.Clutch:
                clutch.Obj.name = clutch.badObjName;
                break;
            case DevicePart.Brake:
                brake.Obj.name = brake.badObjName;
                break;
            case DevicePart.HandBrake:
                handBrake.Obj.name = handBrake.badObjName;
                break;
            case DevicePart.Tire:
                TireRandomBreak();
                break;
            case DevicePart.Screw:
                ScrewRandomBreak();
                break;
            case DevicePart.Horn:
                HronBreak();
                break;
            case DevicePart.Dashbroad:
                dashbroad.OnKeyPlugIn = null;
                dashbroad.OnKeyPlugIn += DashBoardRandomBreak_key;
                break;

            case DevicePart.CarLight_BigLight:
                carLight.ControlRight_右控制桿.name += carLight.ControlLeft_右控制桿_ab_BigLightName;
                break;
            case DevicePart.CarLight_DirctLight:
                //隨機壞一個 or 兩個一起壞?/////////////////////////////////////////
                carLight.ControlRight_右控制桿.name += carLight.ControlLeft_右控制桿_ab_DirtLight_LName;
                carLight.ControlRight_右控制桿.name += carLight.ControlLeft_右控制桿_ab_DirtLight_RName;
                break;
            case DevicePart.CarLight_BrakeLight:
                brake.Obj.name += brake.breakRearLightName;
                break;
            case DevicePart.CarLight_RearLight:
                carLight.ControlLeft_左控制桿.name += carLight.ControlLeft_左控制桿_ab_LightName;
                break;
            case DevicePart.RearMoveAlert:
                carLight.ControlLeft_左控制桿.name += carLight.ControlLeft_左控制桿_ab_SoundName;
                break;

            case DevicePart.ironPipe:
                ironPipe.goodObj.SetActive(false);
                ironPipe.badObj.SetActive(true);
                break;
            case DevicePart.fork_固定銷:
                Fork_固定銷Bad();
                break;
        }  
    }


    DevicePart MyRondom()
    {
        DevicePart devicePart = (DevicePart)Random.Range(12, 13);
        return devicePart;
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
    void TireRandomBreak()
    {
        int brakeTire = Random.Range(0, 4);

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
    void ScrewRandomBreak()
    {
        int brakeScrew = Random.Range(0, 4);

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

    void DashBoardGood_key()
    {
        _wSMVehicleController.IsEngineOn = true;
        dashbroad.key_onCar.GetComponent<MeshRenderer>().enabled = true;
        dashbroad.OffDashBoard.SetActive(false);
        dashbroad.goodObj.SetActive(true);
        dashbroad.badObj_ChargeLight.SetActive(false);
        dashbroad.badObj_EngineOilLight.SetActive(false);
        dashbroad.badObj_WaterLight.SetActive(false);
    }
    void DashBoardRandomBreak_key()
    {
        _wSMVehicleController.IsEngineOn = true;
        dashbroad.key_onCar.GetComponent<MeshRenderer>().enabled = true;
        dashbroad.OffDashBoard.SetActive(false);
        dashbroad.goodObj.SetActive(false);

        int breakDashBorad = Random.Range(1, 4);
        switch (breakDashBorad)
        {
            case 1:
                dashbroad.badObj_ChargeLight.SetActive(true);
                break;
            case 2:
                dashbroad.badObj_EngineOilLight.SetActive(true);
                break;
            case 3:
                dashbroad.badObj_WaterLight.SetActive(true);
                break;
        }
    }
    void OffDashBoard_Key()
    {
        _wSMVehicleController.IsEngineOn = false;
        dashbroad.OnKeyPlugIn = null;
        dashbroad.key_onCar.GetComponent<MeshRenderer>().enabled = false;
        dashbroad.OffDashBoard.SetActive(true);
        dashbroad.goodObj.SetActive(false);
        dashbroad.badObj_ChargeLight.SetActive(false);
        dashbroad.badObj_EngineOilLight.SetActive(false);
        dashbroad.badObj_WaterLight.SetActive(false);
    }


    public void ControlLight(bool isGood, string LighType)
    {
        if (isGood)
        {
            if (LighType == "R_On")
            {
                _wSMVehicleController.RightSinalLightsOn = true;
                _wSMVehicleController.CurrenLightControl(1);
            }
            if (LighType == "L_On")
            {
                _wSMVehicleController.LeftSinalLightsOn = true;
                _wSMVehicleController.CurrenLightControl(-1);
            }
            if (LighType == "Dirct_Off")
            {
                _wSMVehicleController.RightSinalLightsOn = false;
                _wSMVehicleController.LeftSinalLightsOn = false;
                _wSMVehicleController.CurrenLightControl(0);
            }
            if (LighType == "Big_On")
            {
                _wSMVehicleController.HeadlightsOn = true;
            }
            if (LighType == "Big_Off")
            {
                _wSMVehicleController.HeadlightsOn = false;
            }
            if (LighType == "Brake_On")
            {
                _wSMVehicleController._brakes = 2;    //沒有指定燈
                //_wSMVehicleController._movingBackwards = true;
            }
            if (LighType == "Brake_Off")
            {
                _wSMVehicleController._brakes = 0;    //沒有指定燈
                // _wSMVehicleController._movingBackwards = false;
            }
            if (LighType == "RevLight_On")
            {
                _wSMVehicleController._movingBackwards = true;
            }
            if (LighType == "RevLight_On_BreakLight")
            {
                _wSMVehicleController._movingBackwards = true;
                _wSMVehicleController.reverseAlarmLights[0].intensity = 0;//關閉燈號，可以設定成固定壞某邊
                _wSMVehicleController.reverseAlarmLights[1].intensity = 0;//關閉燈號，可以設定成固定壞某邊
                _wSMVehicleController.reverseAlarmLights_Materials[0].DisableKeyword("_EMISSION");// = Color.black;//關閉燈號
                _wSMVehicleController.reverseAlarmLights_Materials[1].DisableKeyword("_EMISSION");//.color = Color.black;//關閉燈號
                _wSMVehicleController.reverseAlarmLights_Materials[2].DisableKeyword("_EMISSION");// = Color.black;//關閉燈號
                _wSMVehicleController.reverseAlarmLights_Materials[3].DisableKeyword("_EMISSION");//.color = Color.black;//關閉燈號
            }
            if (LighType == "RevLight_On_BreakSound")
            {
                _wSMVehicleController._movingBackwards = true;
                _wSMVehicleController.backUpBeeperSFX = null;//關閉聲音
            }
            if (LighType == "RevLight_Off")
            {
                _wSMVehicleController._movingBackwards = false;
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
        //沒有坐左邊錯誤的
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
    public GameObject goodObj_Circle;
    [SerializeField]
    public  GameObject badObj_Circle;
  
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
    public UnityAction OnKeyPlugIn;

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
    [SerializeField]
    public string ControlLeft_右控制桿_ab_BigLightName = "ControlLeft_右控制桿_ab_BigLight";
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