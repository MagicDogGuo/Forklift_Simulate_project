using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : MonoBehaviour
{
    float Good_Ground_Distance;
    float limeit_Ground_Distance = 0.08f;

    float Good_BackFork_Distance;
    float limeit_BackFork_Distance = 1.7f;


    bool isTouchFrokleft;

    public static bool isTooHeight = false;
    public static bool isTooFarTo後扶架 = false;



    GameObject BackForkObj;

    private void Start()
    {
        isTouchFrokleft = false;

        isTooHeight = false;
        isTooFarTo後扶架 = false;
    }

    private void Update()
    {
        if (BackForkObj == null) BackForkObj = GameObject.Find("Box106_後扶架");

        Good_Ground_Distance = this.transform.localPosition.z;
        Good_BackFork_Distance = Vector3.Distance(BackForkObj.transform.position, this.transform.position);

        // Debug.Log("Good_BackFork_Distance:" + Good_BackFork_Distance);
        Debug.Log("isTooFarTo後扶架:" + isTooFarTo後扶架+ "  isTouchFrokleft:"+ isTouchFrokleft);
        if (isTouchFrokleft)
        {
            //地面距離
            if(Good_Ground_Distance > limeit_Ground_Distance)
            {
                isTooHeight = true;
            }
            else
            {
                isTooHeight = false;
            }

            //後扶架距離
            if (Good_BackFork_Distance > limeit_BackFork_Distance)
            {
                isTooFarTo後扶架 = true;
            }
            else
            {
                isTooFarTo後扶架 = false;
            }


        }
        else
        {
            isTooFarTo後扶架 = false;
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Forkleft")
        {
            StageThreeGameManager.Instance.IsNeerGoods = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Forkleft")
        {
            StageThreeGameManager.Instance.IsNeerGoods = false;

        }
    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == "Forkleft")
        {
            isTouchFrokleft = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Forkleft")
        {
            isTouchFrokleft = false;
        }
    }
}
