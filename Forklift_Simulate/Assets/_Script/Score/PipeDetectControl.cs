using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDetectControl : MonoBehaviour
{
    [SerializeField]
    GameObject PipeGroup;

    Pipe[] pipes;

    int _beColliderTotalAmount;
    public int BeColliderTotalAmount
    {
        get { return _beColliderTotalAmount; }
    }

    void Start()
    {
        pipes = PipeGroup.GetComponentsInChildren<Pipe>();
    }

    void Update()
    {
        for (int i =0; i < pipes.Length; i++)
        {

            if (pipes[i].IsBeCollider)
            {
                _beColliderTotalAmount += pipes[i].BeColliderAmount;
            }
 

        }
        Debug.Log("BeColliderTotalAmount: " + BeColliderTotalAmount);

    }

}
