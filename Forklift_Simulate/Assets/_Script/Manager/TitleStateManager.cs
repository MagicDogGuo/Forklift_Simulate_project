using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleStateManager : MonoBehaviour
{

    [SerializeField]
    Button ConfirmBtn;

    [SerializeField]
    InputField input_座號;

    [SerializeField]
    InputField input_姓名;

    void Start()
    {
        ConfirmBtn.onClick.AddListener(OnPushConfirmBtn);

        input_座號.onEndEdit.AddListener(OnEndinput_座號);
        input_姓名.onEndEdit.AddListener(OnEndinput_姓名);


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


    void OnEndinput_座號(string s)
    {
        UserDatabase.ID = int.Parse(s);
    }

    void OnEndinput_姓名(string s)
    {
        UserDatabase.Name = s;
    }
}
