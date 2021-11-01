using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRoad : MonoBehaviour
{
    public bool isForkitOnRoad = false;

    void Start()
    {
        isForkitOnRoad = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Forkleft")
        {
            isForkitOnRoad = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Forkleft")
        {
            isForkitOnRoad = false;
        }
    }

}
