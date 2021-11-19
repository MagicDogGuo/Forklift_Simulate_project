using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCam : MonoBehaviour
{
    [SerializeField]
    GameObject TrackObj;

    [SerializeField]
    GameObject CameraObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraObj.transform.position = TrackObj.transform.position;
        CameraObj.transform.rotation = TrackObj.transform.rotation;

    }
}
