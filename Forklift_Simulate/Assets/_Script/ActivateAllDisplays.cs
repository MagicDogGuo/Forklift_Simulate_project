using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateAllDisplays : MonoBehaviour
{
    public Text intfoText;

    void Awake()
    {
        //MultScreen();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MultScreen();
        }
    }

    void MultScreen()
    {

        Debug.Log(GetType() + "/MultScreen()/ Display.displays.Length = " + Display.displays.Length);
        intfoText.text = "当前获得屏幕数量为：" + Display.displays.Length;
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            Screen.SetResolution(Display.displays[i].renderingWidth, Display.displays[i].renderingHeight, true);
        }
    }

}