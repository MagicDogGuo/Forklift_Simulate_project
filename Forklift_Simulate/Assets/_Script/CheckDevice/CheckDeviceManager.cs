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
        CarLight_DirctLight = 15
    }

    void Start()
    {
        DecideObjectState();
    }

    void DecideObjectState()
    {
        cold_冷卻液.goodObj.SetActive(true);
        engineOil.goodObj.SetActive(true);
        batteryWater.goodObj.SetActive(true);
        brakeOil.goodObj.SetActive(true);
        waterOil_液壓油.goodObj.SetActive(true);
        clutch.Obj.name = clutch.goodObjName;
        brake.Obj.name = brake.goodObjName;
        handBrake.Obj.name = handBrake.goodObjName;
        TireGood();
        HronGood();
        dashbroad.key.GetComponent<MeshRenderer>().enabled = false;
        dashbroad.OnKeyPlugIn += DashBoardGood_key;

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
        }  
    }


    DevicePart MyRondom()
    {
        DevicePart devicePart = (DevicePart)Random.Range(13, 14);
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

    void ScrewRandomBreak()
    {
        int brakeScrew = Random.Range(0, 4);

        tire.Screw[brakeScrew].name += tire.BreakScrewName;
        Debug.Log("ScrewBrake: " + tire.Screw[brakeScrew].name);
    }


    void HronGood()
    {
        wheels.Horn.name += wheels.NormalHornName;
    }
    void HronBreak()
    {
        wheels.Horn.name += wheels.BreakHronName;
    }

    void DashBoardGood_key()
    {
        _wSMVehicleController.IsEngineOn = true;
        dashbroad.key.GetComponent<MeshRenderer>().enabled = true;
        dashbroad.OffDashBoard.SetActive(false);
        dashbroad.goodObj.SetActive(true);
        dashbroad.badObj_ChargeLight.SetActive(false);
        dashbroad.badObj_EngineOilLight.SetActive(false);
        dashbroad.badObj_WaterLight.SetActive(false);
    }
    void DashBoardRandomBreak_key()
    {
        _wSMVehicleController.IsEngineOn = true;
        dashbroad.key.GetComponent<MeshRenderer>().enabled = true;
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
        dashbroad.key.GetComponent<MeshRenderer>().enabled = false;
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
        }
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
    public string goodObjName;
    [SerializeField]
    public string badObjName;
}


[System.Serializable]
public class Brake
{
    [SerializeField]
    public GameObject Obj;
    [SerializeField]
    public string goodObjName;
    [SerializeField]
    public string badObjName;
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
    public string goodObjName;
    [SerializeField]
    public string badObjName;
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
    public string BreakScrewName = "_abScrew";
}

[System.Serializable]
public class Dashbroad
{
    public GameObject key;
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
public class Fork
{
    [SerializeField]
    public GameObject goodObj;
    [SerializeField]
    public GameObject badObj;
}