using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeForkleftView : MonoBehaviour
{
    [SerializeField]
    GameObject FrontCamera;
    [SerializeField]
    GameObject BackCamera;
    [SerializeField]
    GameObject RightCamera;
    [SerializeField]
    GameObject LeftCamera;
    [SerializeField]
    GameObject TopCameraImage;


    LogtichControl logtichControl;


    void Start()
    {
        FrontCamera.SetActive(true);
        BackCamera.SetActive(false);
        RightCamera.SetActive(false);
        LeftCamera.SetActive(false);
        logtichControl = GetComponentInParent<LogtichControl>();
    }

    void Update()
    {
        if (logtichControl.CameraFront)// || GetComponent<WSMGameStudio.Vehicles.WSMVehicleController>().BackFrontInput == 1
        {
            FrontCamera.SetActive(true);
            BackCamera.SetActive(false);
            RightCamera.SetActive(false);
            LeftCamera.SetActive(false);
        }
        if (logtichControl.CameraBack)// || GetComponent<WSMGameStudio.Vehicles.WSMVehicleController>().BackFrontInput == -1
        {
            FrontCamera.SetActive(false);
            BackCamera.SetActive(true);
            RightCamera.SetActive(false);
            LeftCamera.SetActive(false);
        }
        if (logtichControl.CameraRight)
        {
            FrontCamera.SetActive(false);
            BackCamera.SetActive(false);
            RightCamera.SetActive(true);
            LeftCamera.SetActive(false);
        }
        if (logtichControl.CameraLeft)
        {
            FrontCamera.SetActive(false);
            BackCamera.SetActive(false);
            RightCamera.SetActive(false);
            LeftCamera.SetActive(true);
        }


        if (CurrentPosLimit.isInPosLimit)
        {
            TopCameraImage.SetActive(true);
        }
        else
        {
            TopCameraImage.SetActive(false);

        }
    }
}
