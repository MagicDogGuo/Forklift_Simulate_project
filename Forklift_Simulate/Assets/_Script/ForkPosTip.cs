using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkPosTip : MonoBehaviour
{

    [SerializeField]
    GameObject TargetPoint;

    Vector3 oriPos;
    Vector3 oriRotete;


    bool isTouchTarget;

    void Start()
    {
        oriPos = this.transform.localPosition;
        oriRotete = this.transform.localEulerAngles;
        isTouchTarget = false;
    }

    void Update()
    {
        transform.localPosition = oriPos;
        transform.localEulerAngles = oriRotete;
        if (isTouchTarget)
        {
            Debug.Log("================Red!");
            TargetPoint.GetComponent<Renderer>().material.SetColor("_Color",new Color(1,0,0,0.8f));
        }
        else
        {
            TargetPoint.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 0.8f));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == TargetPoint.name)
        {
            Debug.Log("other.nam"+other.name);
            isTouchTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == TargetPoint.name)
        {
            isTouchTarget = false;
        }
    }
}
