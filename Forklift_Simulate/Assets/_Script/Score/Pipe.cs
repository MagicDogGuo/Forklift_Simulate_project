using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    int _beColliderAmount;
    public int BeColliderAmount
    {
        get { return _beColliderAmount; }
    }

    bool _isbeCollider;
    public bool IsBeCollider
    {
        get { return _isbeCollider; }
    }

    private void Start()
    {
        _isbeCollider = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Forkleft")
        {
            _beColliderAmount++;
            _isbeCollider = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Forkleft")
        {
            _isbeCollider = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Forkleft")
        {
            _isbeCollider = false;
        }
    }
}
