using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBeHandTakeObj : MonoBehaviour
{
    [HideInInspector]
    public Vector3 oriPos;

    public Vector3 OffsetPos;

    void Start()
    {
        oriPos = this.transform.localPosition;
    }

 
}
