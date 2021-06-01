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

        for (int i = 0; i < pipes.Length; i++)
        {

        }
    }

    void Update()
    {
        for (int i =0; i < pipes.Length; i++)
        {

            if (pipes[i].IsBeCollider)
            {
                _beColliderTotalAmount += 1;
                Debug.Log("BeColliderTotalAmount: " + BeColliderTotalAmount);

            }

        }

    }

}
