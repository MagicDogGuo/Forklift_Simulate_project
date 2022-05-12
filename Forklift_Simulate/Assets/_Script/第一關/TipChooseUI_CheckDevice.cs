using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TipChooseUI_CheckDevice : MonoBehaviour
{
    public Button[] stateBtn;

    public Text TitleTxt;

    [SerializeField]
    public  Button closeBtn;

    // Start is called before the first frame update
    void Start()
    {
      

        if (RecordUserDate.modeChoose == RecordUserDate.ModeChoose.VR)
        {
            this.GetComponentInChildren<VRTK.VRTK_UICanvas>().enabled = true;
        }
        else if (SceneManager.GetActiveScene().name == "FirstStage")
        {
            this.GetComponentInChildren<VRTK.VRTK_UICanvas>().enabled = true;

        }
        else
        {
            this.GetComponentInChildren<VRTK.VRTK_UICanvas>().enabled = false;

        }
     

        GetComponentInChildren<Canvas>().worldCamera =Camera.main;
        closeBtn.onClick.AddListener(OnPushCloseBtn);
    }

    void Update()
    {
        
    }

    void OnPushCloseBtn()
    {
        Destroy(this.gameObject, 0.5f);
    }
}
