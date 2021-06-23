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

    public void Init()
    {
        _beColliderTotalAmount = 0;
    }

}
