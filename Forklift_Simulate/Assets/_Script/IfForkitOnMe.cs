using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class IfForkitOnMe : MonoBehaviour
{
    public bool isForkitOnRoad = false;

    void Start()
    {
        Init();
    }

    public void Init()
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
