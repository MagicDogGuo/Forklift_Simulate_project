using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTW : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (JoyStickHelper.GetButton(JoyStickHelper.JoyStickButtonCode.Button05))
        {
            Debug.Log("===Button05");
        }
        Debug.Log("===05");
    }
}
