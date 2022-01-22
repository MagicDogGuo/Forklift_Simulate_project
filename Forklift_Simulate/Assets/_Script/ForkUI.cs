using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForkUI : MonoBehaviour
{
    [SerializeField]
    GameObject ColliderUI_撞擊漸層R;
    [SerializeField]
    GameObject ColliderUI_撞擊漸層L;
    [SerializeField]
    GameObject ColliderUI_撞擊漸層B;
    [SerializeField]
    ForkleftBodyTrigger[] forkleftBodyTrigger;
    

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
        startTipUI.GetComponent<WarningUIAudio>().AS.clip = startTipUI.GetComponent<WarningUIAudio>().StartTipAudioClip;
        startTipUI.GetComponent<WarningUIAudio>().AS.Stop();
        startTipUI.GetComponent<WarningUIAudio>().AS.Play();


        ColliderUI_撞擊漸層R.SetActive(false) ;
        ColliderUI_撞擊漸層L.SetActive(false);
        ColliderUI_撞擊漸層B.SetActive(false);

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


        foreach(var triggers in forkleftBodyTrigger)
        {
            if (triggers.isTriggerOn)
            {
                switch (triggers.dirSide)
                {
                    case ForkleftBodyTrigger.Dir.R:
                        ColliderUI_撞擊漸層R.SetActive(true);
                        break;
                    case ForkleftBodyTrigger.Dir.L:
                        ColliderUI_撞擊漸層L.SetActive(true);
                        break;
                    case ForkleftBodyTrigger.Dir.B:
                        ColliderUI_撞擊漸層B.SetActive(true);
                        break;
                }
            }
            else
            {
                switch (triggers.dirSide)
                {
                    case ForkleftBodyTrigger.Dir.R:
                        ColliderUI_撞擊漸層R.SetActive(false);
                        break;
                    case ForkleftBodyTrigger.Dir.L:
                        ColliderUI_撞擊漸層L.SetActive(false);
                        break;
                    case ForkleftBodyTrigger.Dir.B:
                        ColliderUI_撞擊漸層B.SetActive(false);
                        break;
                }
            }
        }

    }
}
