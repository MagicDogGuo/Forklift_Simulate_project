using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Examples.Utilities;
public class VRTK_SDKManager_SetPos : MonoBehaviour
{
    GameObject forkliftCustom04_ChainOk_VRObj;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<VRTK_SDKManager>().enabled = false;
#if UNITY_EDITOR
        this.GetComponent<VRTKExample_FixSetup>().enabled = false;
#endif
        this.GetComponent<ResetVRPosition>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (forkliftCustom04_ChainOk_VRObj == null)
            forkliftCustom04_ChainOk_VRObj = GameObject.Find("forkliftCustom04_ChainOk_VR(Clone)");

        if(forkliftCustom04_ChainOk_VRObj!=null&&
             this.GetComponent<VRTK_SDKManager>().enabled == false)
        {
            this.transform.SetParent(forkliftCustom04_ChainOk_VRObj.transform);
            this.transform.localPosition = new Vector3(0.01f, 0, 0);
            this.GetComponent<VRTK_SDKManager>().enabled = true;
#if UNITY_EDITOR
            this.GetComponent<VRTKExample_FixSetup>().enabled = true;
#endif

            this.GetComponent<ResetVRPosition>().enabled = true;
        }
    
    }
}
