using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkleftBodyTrigger : MonoBehaviour
{
  
    public enum Dir
    {
        R,
        L,
        B
    }

    public Dir dirSide;

    [HideInInspector]
    public bool isTriggerOn = false;

    float countTriggerDisappear;

    void Start()
    {

        isTriggerOn = false;
      
    }


    // Update is called once per frame
    void Update()
    {
        //倒數兩秒回到false
        if (isTriggerOn)
        {
            countTriggerDisappear += Time.deltaTime;
        }
        if (countTriggerDisappear > 2)
        {
            isTriggerOn = false;
            countTriggerDisappear = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Pipe>() != null
           && other.GetComponent<Pipe>().isCanUIRed)
        {
            isTriggerOn = true;
            other.GetComponent<Pipe>().isCanUIRed = false;
            Debug.Log(" other.GetComponent<Pipe>().isCanUIRed "+other.GetComponent<Pipe>().isCanUIRed);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Pipe>() != null) isTriggerOn = false;
    }
}
