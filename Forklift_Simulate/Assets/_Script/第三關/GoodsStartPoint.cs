using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsStartPoint : MonoBehaviour
{
    public bool isOnStartPoint_Goods;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goods")
        {
            isOnStartPoint_Goods = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Goods")
        {
            isOnStartPoint_Goods = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Goods")
        {
            isOnStartPoint_Goods = false;
        }
    }
}
