using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public bool isOnEndPoint_Forkit;
    public bool isAllreadyArraivalEndPoint;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Forkleft")
        {
            isOnEndPoint_Forkit = true;
            isAllreadyArraivalEndPoint = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Forkleft")
        {
            isOnEndPoint_Forkit = false;
        }
    }

}
