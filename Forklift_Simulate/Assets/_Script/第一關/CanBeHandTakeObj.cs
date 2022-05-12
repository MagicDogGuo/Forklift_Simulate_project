using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBeHandTakeObj : MonoBehaviour
{
    [HideInInspector]
    public Vector3 oriPos;
    [HideInInspector]
    public Vector3 oriRota;
    [HideInInspector]
    public GameObject oriPareant;

    public Vector3 OffsetPos;

    void Start()
    {
        oriPos = this.transform.localPosition;
        oriRota = this.transform.localEulerAngles;
        oriPareant = this.transform.parent.gameObject;
    }

    private void Update()
    {
     
    }


}
