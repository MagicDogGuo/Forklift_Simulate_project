using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevereScale : MonoBehaviour
{
    [SerializeField]
    GameObject TargetObj;

    float TargetScale;

    float rate;

    void Start()
    {
        
    }

    void Update()
    {
        TargetScale = TargetObj.transform.localScale.y;
        rate = (float)(TargetScale / 0.8917201);
        this.transform.localScale = new Vector3(this.transform.localScale.x,1/rate, this.transform.localScale.z);
    }
}
