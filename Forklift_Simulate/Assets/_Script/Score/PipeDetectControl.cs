using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDetectControl : MonoBehaviour
{
    [SerializeField]
    GameObject PipeGroup;

    Pipe[] pipes;
    Transform[] pipPos;

    int _beColliderTotalAmount;
    public int BeColliderTotalAmount
    {
        get { return _beColliderTotalAmount; }
    }

    void Start()
    {
        pipes = PipeGroup.GetComponentsInChildren<Pipe>();
     
        Init();

        StartCoroutine(DalayBack());
    }

    void Update()
    {
        for (int i = 0; i < pipes.Length; i++)
        {

            if (pipes[i].IsBeCollider)
            {
                _beColliderTotalAmount += 1;
                //Debug.Log("BeColliderTotalAmount: " + BeColliderTotalAmount);
            }

        }

    }

    public void Init()
    {
        _beColliderTotalAmount = 0;
    }

    public void PipeBackToOri()
    {
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].gameObject.layer = LayerMask.NameToLayer("Default");
            pipes[i].gameObject.transform.localPosition = pipes[i].oriPos;
            pipes[i].gameObject.transform.localRotation = pipes[i].oriRote;

            pipes[i].GetComponent<Pipe>().isCanUIRed = true;

            StartCoroutine(DalayBack());
            pipes[i].IsStopCollDetect = false;
            if (i == 74)
            {
                Debug.Log("=================================opip" + pipes[i].name + "   " + pipes[i].gameObject.transform.localPosition + "  " + pipes[i].oriTras.localPosition);

            }
        }
    }

   
    IEnumerator DalayBack()
    {
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }


}
