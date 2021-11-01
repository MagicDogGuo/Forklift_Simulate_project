using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public bool isOnStartPoint_Forkit;
    public bool isNeedToBackStartPoint;



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Forkleft")
        {
            isOnStartPoint_Forkit = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Forkleft")
        {
            isOnStartPoint_Forkit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Forkleft")
        {
            isOnStartPoint_Forkit = false;
        }
    }

}
