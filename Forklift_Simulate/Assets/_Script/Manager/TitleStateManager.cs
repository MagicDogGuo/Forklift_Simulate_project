using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleStateManager : MonoBehaviour
{
    [SerializeField]
    GameObject InitInputObj;
    [SerializeField]
    GameObject ModeChooseObj;


    [SerializeField]
    Button ConfirmBtn;

    [SerializeField]
    InputField input_座號;

    [SerializeField]
    InputField input_姓名;


    [SerializeField]
    Button VRBtn;
    [SerializeField]
    Button PCBtn;

    void Start()
    {
        InitInputObj.SetActive(true);
        ModeChooseObj.SetActive(false);


        ConfirmBtn.onClick.AddListener(OnPushConfirmBtn);

        input_座號.onEndEdit.AddListener(OnEndinput_座號);
        input_姓名.onEndEdit.AddListener(OnEndinput_姓名);

        VRBtn.onClick.AddListener(OnPushVRBtn);
        PCBtn.onClick.AddListener(OnPushPCBtn);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnPushConfirmBtn()
    {
        if (ModeChooseObj.active == true)
        {
            int sceneIndex_FirstStageTest = SceneUtility.GetBuildIndexByScenePath("_scene/FirstStageTest");
            int sceneIndex_MainGameState = SceneUtility.GetBuildIndexByScenePath("_scene/MainGameState");
            if (sceneIndex_FirstStageTest == -1)//沒有在bulid setting打勾
            {
                SceneManager.LoadScene("MainGameState");
            }
            else if (sceneIndex_MainGameState == -1)
            {
                SceneManager.LoadScene("FirstStageTest");
            }
            else
            {
                Debug.Log("沒有下一關");
            }

        }
        if (InitInputObj.active == true)
        {
            InitInputObj.SetActive(false);
            ModeChooseObj.SetActive(true);
        }
      

    }


    void OnEndinput_座號(string s)
    {
        UserDatabase.ID = int.Parse(s);
    }

    void OnEndinput_姓名(string s)
    {
        UserDatabase.Name = s;
    }

    void OnPushVRBtn()
    {
        RecordUserDate.modeChoose = RecordUserDate.ModeChoose.VR;
        OnPushConfirmBtn();
    }

    void OnPushPCBtn()
    {
        RecordUserDate.modeChoose = RecordUserDate.ModeChoose.PC;
        OnPushConfirmBtn();

    }
}
