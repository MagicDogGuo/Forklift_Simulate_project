using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pipe : MonoBehaviour
{

    bool _isbeCollider;
    public bool IsBeCollider
    {
        get { return _isbeCollider; }
    }

    bool isStopCollDetect;

    private void Start()
    {
        isStopCollDetect = false;
        _isbeCollider = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isStopCollDetect) return;

        if(collision.gameObject.tag == "Forkleft")
        {
            _isbeCollider = true;
            isStopCollDetect = true;
            StartCoroutine(DelayFalse());
        }
    }


    IEnumerator DelayFalse()
    {
        yield return new WaitForEndOfFrame();
        _isbeCollider = false;

    }
}
