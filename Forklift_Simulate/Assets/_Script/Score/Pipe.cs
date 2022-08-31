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


    public bool isCanUIRed = true;//撞到紅色提示，給ForkleftBodyTrigger控

    private void Start()
    {
        isCanUIRed = true;

        IsStopCollDetect = false;
        _isbeCollider = false;
        oriTras = this.transform;
        oriPos = this.transform.localPosition;
        oriRote = this.transform.localRotation;
    }


    private void Update()
    {
        //if(this.transform.localPosition == oriPos)
        //{
        //    this.GetComponent<Rigidbody>().useGravity = true;
        //    this.GetComponent<BoxCollider>().isTrigger = false;
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsStopCollDetect) return;

        if(collision.gameObject.tag == "Forkleft" || collision.gameObject.tag == "Goods")
        {
            Debug.Log("22222222222222222222222222");

            _isbeCollider = true;
            IsStopCollDetect = true;
            StartCoroutine(DelayFalse());
        }
    }


    IEnumerator DelayFalse()
    {
        yield return new WaitForEndOfFrame();

        _isbeCollider = false;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1);


        this.gameObject.layer = LayerMask.NameToLayer("noCollider");//default layer跟noCollider不會有碰撞

        yield return new WaitForSeconds(1);

        ////////////////this.GetComponent<BoxCollider>().enabled = false;

    }
}
