using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsEndPoint : MonoBehaviour
{
    public bool isOnEndPoint_Goods;
    public bool isAllreadyArraivalEndPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goods")
        {
            isOnEndPoint_Goods = true;
            isAllreadyArraivalEndPoint = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Goods")
        {
            isOnEndPoint_Goods = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Goods")
        {
            isOnEndPoint_Goods = false;
        }
    }
}
