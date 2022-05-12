using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningUIComp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
        if(GameObject.Find("Camera_UI")!=null)GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("Camera_UI").GetComponent<Camera>();
       
        if (GameObject.Find("PlayerCamera") != null) GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
