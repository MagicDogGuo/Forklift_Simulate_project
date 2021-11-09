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

    public Transform oriTras;
    public Vector3 oriPos;
    public Quaternion oriRote;

    public bool IsStopCollDetect;

    private void Start()
    {
        IsStopCollDetect = false;
        _isbeCollider = false;
        oriTras = this.transform;
        oriPos = this.transform.localPosition;
        oriRote = this.transform.localRotation;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (IsStopCollDetect) return;

        if(collision.gameObject.tag == "Forkleft")
        {
            _isbeCollider = true;
            IsStopCollDetect = true;
            StartCoroutine(DelayFalse());
        }
    }


    IEnumerator DelayFalse()
    {
        yield return new WaitForEndOfFrame();
        _isbeCollider = false;

    }
}
