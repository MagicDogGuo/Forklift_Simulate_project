using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkPosTip : MonoBehaviour
{

    [SerializeField]
    GameObject OriPoint;

    [SerializeField]
    GameObject TargetPoint;

    Vector3 oriPos;
    Vector3 oriRotete;

    bool isTouchOri;
    bool isTouchTarget;

    void Start()
    {
        oriPos = this.transform.localPosition;
        oriRotete = this.transform.localEulerAngles;
        isTouchOri = true;
        isTouchTarget = false;
        
    }

    void Update()
    {
        transform.localPosition = oriPos;
        transform.localEulerAngles = oriRotete;

        if (isTouchOri)
        {
            //Debug.Log("================Green!");
            OriPoint.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 1, 0, 0.8f));
        }
        else
        {
            OriPoint.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 0.8f));
        }

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
        if (other.name == OriPoint.name)
        {
            isTouchOri = true;
        }

        if (other.name == TargetPoint.name)
        {
            isTouchTarget = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == OriPoint.name)
        {
            isTouchOri = false;
        }

        if (other.name == TargetPoint.name)
        {
            isTouchTarget = false;
        }
    }
}
