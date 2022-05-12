using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsShelf : MonoBehaviour
{

    public static bool isTouchShelf = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Forkleft")
        {
            isTouchShelf = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Forkleft")
        {
            isTouchShelf = false;
        }
    }

}
