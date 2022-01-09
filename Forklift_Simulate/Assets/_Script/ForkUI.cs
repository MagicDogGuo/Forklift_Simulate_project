using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForkUI : MonoBehaviour
{
    [SerializeField]
    Text GearTxt;

    [SerializeField]
    Text SpeedTxt;

    public GameObject ForkkitCanvasPos;

    private WSMGameStudio.Vehicles.WSMVehicleController _WSMVehicleController;
    LogtichControl logtichControl;

    GameObject startTipUI;


    // Start is called before the first frame update
    void Start()
    {
        _WSMVehicleController = GetComponent<WSMGameStudio.Vehicles.WSMVehicleController>();
        logtichControl = GetComponent<LogtichControl>();

        startTipUI = GameObject.Instantiate(MainGameManager.Instance.WarningUIs, MainGameManager.Instance.ForkitCanvasPoss.transform);
        startTipUI.GetComponentInChildren<Text>().text = "時間8分鐘，請按辦理單位所提供之無負載堆高機，於規定時間內依規定路線前進、倒車及停車等動作。";
    }

    // Update is called once per frame
    void Update()
    {
        GearTxt.text = "Gear: " + _WSMVehicleController.CurrentGear;
        SpeedTxt.text = "Speed" + (int)_WSMVehicleController.CurrentSpeed;

        if (logtichControl.CheckEnterUI || Input.GetKey(KeyCode.Return))
        {
            Destroy(startTipUI);
        }


    }
}
