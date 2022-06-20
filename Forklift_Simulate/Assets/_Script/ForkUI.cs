using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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


        //判斷現在是在哪一關
        if(SceneManager.GetActiveScene().name == "MainGameState")
        {
            startTipUI = GameObject.Instantiate(MainGameManager.Instance.WarningUIs, MainGameManager.Instance.ForkitCanvasPoss.transform);
            startTipUI.GetComponentInChildren<Text>().text = "時間8分鐘，請按辦理單位所提供之無負載堆高機，於規定時間內依規定路線前進、倒車及停車等動作。";
            startTipUI.GetComponent<WarningUIAudio>().AS.clip = startTipUI.GetComponent<WarningUIAudio>().StartTipAudioClip_StageTwo;
            startTipUI.GetComponent<WarningUIAudio>().AS.Stop();
            startTipUI.GetComponent<WarningUIAudio>().AS.Play();
        }
        else if (SceneManager.GetActiveScene().name == "ThridStage")
        {
            startTipUI = GameObject.Instantiate(StageThreeGameManager.Instance.WarningUIs, StageThreeGameManager.Instance.ForkitCanvasPoss.transform);
            startTipUI.GetComponentInChildren<Text>().text = "時間15分鐘，按辦理單位所提供之無負載堆高機，於規定時間內依規定路線分別以前進及倒車方式，完成起動、裝載、卸貨及停車等動作。";
            startTipUI.GetComponent<WarningUIAudio>().AS.clip = startTipUI.GetComponent<WarningUIAudio>().StartTipAudioClip_StageThree;
            startTipUI.GetComponent<WarningUIAudio>().AS.Stop();
            startTipUI.GetComponent<WarningUIAudio>().AS.Play();
        }


       

        if(RecordUserDate.modeChoose == RecordUserDate.ModeChoose.PC)
        {

            ColliderUI_撞擊漸層R.SetActive(false);
            ColliderUI_撞擊漸層L.SetActive(false);
            ColliderUI_撞擊漸層B.SetActive(false);
        }


    }


    // Update is called once per frame
    void Update()
    {
        //VR模式自動關閉前述提示
        if(RecordUserDate.modeChoose == RecordUserDate.ModeChoose.VR || RecordUserDate.modeChoose == RecordUserDate.ModeChoose.Null)
        {
            if (startTipUI != null)
            {
                if (!startTipUI.GetComponent<WarningUIAudio>().AS.isPlaying)
                {
                    Destroy(startTipUI);
                }
            }
        }
           

        GearTxt.text = "Gear: " + _WSMVehicleController.CurrentGear;
        SpeedTxt.text = "Speed" + (int)_WSMVehicleController.CurrentSpeed;

        //PC
        if (RecordUserDate.modeChoose == RecordUserDate.ModeChoose.PC)
        {
            if (logtichControl.CheckEnterUI || Input.GetKey(KeyCode.Return))
            {
                Destroy(startTipUI);
            }
        }

        if (RecordUserDate.modeChoose != RecordUserDate.ModeChoose.PC)
        {
            return;
        }

        //螢幕撞擊提示
        foreach (var triggers in forkleftBodyTrigger)
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
