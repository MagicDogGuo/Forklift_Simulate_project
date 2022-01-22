using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPosLimit : MonoBehaviour
{

    public static bool isInPosLimit;

    // Start is called before the first frame update
    void Start()
    {
        isInPosLimit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Forkleft")
        {
            isInPosLimit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Forkleft")
        {
            isInPosLimit = false;
        }
    }
}
