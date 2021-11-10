using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntiComp : MonoBehaviour
{
    [SerializeField]
    Button TestModeBtn;

    [SerializeField]
    Button PraticeModeBtn;

    void Start()
    {
        TestModeBtn.onClick.AddListener(GameEventSystem.Instance.OnPushTestModeBtn);
        PraticeModeBtn.onClick.AddListener(GameEventSystem.Instance.OnPushPracticeModeBtn);

        GetComponent<Canvas>().worldCamera = GameObject.Find("InitCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameEventSystem.Instance.OnPushTestModeBtn();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameEventSystem.Instance.OnPushPracticeModeBtn();
        }
    }
}
