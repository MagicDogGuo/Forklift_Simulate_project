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


    // Start is called before the first frame update
    void Start()
    {
        _WSMVehicleController = GetComponent<WSMGameStudio.Vehicles.WSMVehicleController>();

    }

    // Update is called once per frame
    void Update()
    {
        GearTxt.text = "Gear: " + _WSMVehicleController.CurrentGear;
        SpeedTxt.text = "Speed" + (int)_WSMVehicleController.CurrentSpeed;
    }
}
