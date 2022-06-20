using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class IntiComp : MonoBehaviour
{
    [SerializeField]
    Button TestModeBtn;

    [SerializeField]
    Button PraticeModeBtn;

    [SerializeField]
    Button BackTitleBtn;

    //[SerializeField]//改到ResetVRPosition
    //InputField SetX;
    //[SerializeField]
    //InputField SetY;
    //[SerializeField]
    //InputField SetZ;

    //GameObject VRTK_SDKManagerObj;

    void Start()
    {
        //關閉VR模式
        XRSettings.enabled = false;

        TestModeBtn.onClick.AddListener(GameEventSystem.Instance.OnPushTestModeBtn);
        PraticeModeBtn.onClick.AddListener(GameEventSystem.Instance.OnPushPracticeModeBtn);
        BackTitleBtn.onClick.AddListener(OnPushBackTitleBtn);

        GetComponent<Canvas>().worldCamera = GameObject.Find("InitCamera").GetComponent<Camera>();

        //VRTK_SDKManagerObj = GameObject.Find("[VRTK_SDKManager]");
        //SetX.text = VRTK_SDKManagerObj.GetComponent<ResetVRPosition>().OriDriveForkleftPos.x + "";
        //SetY.text = VRTK_SDKManagerObj.GetComponent<ResetVRPosition>().OriDriveForkleftPos.y + "";
        //SetZ.text = VRTK_SDKManagerObj.GetComponent<ResetVRPosition>().OriDriveForkleftPos.z + "";
        //SetX.text = ResetVRPosition.OriDriveForkleftPos.x + "";
        //SetY.text = ResetVRPosition.OriDriveForkleftPos.y + "";
        //SetZ.text = ResetVRPosition.OriDriveForkleftPos.z + "";

    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;

        if (Cursor.visible==false) Cursor.visible = true;
        //VRTK_SDKManagerObj.GetComponent<ResetVRPosition>().OriDriveForkleftPos =
        //    new Vector3(float.Parse(SetX.text), float.Parse(SetY.text), float.Parse(SetZ.text));
        //ResetVRPosition.OriDriveForkleftPos =
        //    new Vector3(float.Parse(SetX.text), float.Parse(SetY.text), float.Parse(SetZ.text));

        if (Input.GetKeyDown(KeyCode.N))
        {
            GameEventSystem.Instance.OnPushTestModeBtn();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameEventSystem.Instance.OnPushPracticeModeBtn();
        }
    }

    void OnPushBackTitleBtn()
    {
        SceneManager.LoadScene("TitleState");
    }

    private void OnDestroy()
    {
        if (Cursor.visible == true) Cursor.visible = false;
    }

    private void OnDisable()
    {
        if (Cursor.visible == true) Cursor.visible = false;

    }
}
