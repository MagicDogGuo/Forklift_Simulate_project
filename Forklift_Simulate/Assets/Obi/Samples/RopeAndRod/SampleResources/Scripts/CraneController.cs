using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class CraneController : MonoBehaviour {

	ObiRopeCursor[] cursor;
	ObiRope rope01;
    ObiRope rope02;

    [SerializeField]
    GameObject starLoadObj;
    [SerializeField]
    GameObject endLoadobj;


	// Use this for initialization
	void Start () {
		cursor = GetComponentsInChildren<ObiRopeCursor>();
		rope01 = cursor[0].GetComponent<ObiRope>();
        rope02 = cursor[1].GetComponent<ObiRope>();

        //////////////////////////////////////////////////////////////////////
        //cursor[0].ChangeLength(10f);//start
        //cursor[1].ChangeLength(9.75f);//end
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("rope.restLength01:" + rope01.restLength);

        Debug.Log("rope02.restLength " + rope02.restLength);
        if (Input.GetKey(KeyCode.W)){
            //if (rope.restLength > 6.5f)
            //////////////////////////////////////////////////////////////////////
            cursor[0].ChangeLength(rope01.restLength - 1f * Time.deltaTime);
            cursor[1].ChangeLength(rope01.restLength + 1f * Time.deltaTime);

            starLoadObj.transform.localPosition = new Vector3(starLoadObj.transform.localPosition.x,
                                                          starLoadObj.transform.localPosition.y + 1 * Time.deltaTime,
                                                          starLoadObj.transform.localPosition.z);
            endLoadobj.transform.localPosition = new Vector3(endLoadobj.transform.localPosition.x,
                                                        endLoadobj.transform.localPosition.y - 1 * Time.deltaTime,
                                                        endLoadobj.transform.localPosition.z);

        }

        if (Input.GetKey(KeyCode.S)){


            //////////////////////////////////////////////////////////////////////
            cursor[0].ChangeLength(rope01.restLength + 1f * Time.deltaTime);
            cursor[1].ChangeLength(rope01.restLength - 1f * Time.deltaTime);
            starLoadObj.transform.localPosition = new Vector3(starLoadObj.transform.localPosition.x,
                                                          starLoadObj.transform.localPosition.y - 1 * Time.deltaTime,
                                                          starLoadObj.transform.localPosition.z);
            endLoadobj.transform.localPosition = new Vector3(endLoadobj.transform.localPosition.x,
                                                        endLoadobj.transform.localPosition.y + 1 * Time.deltaTime,
                                                        endLoadobj.transform.localPosition.z);

        }

        if (Input.GetKey(KeyCode.A)){
			transform.Rotate(0,Time.deltaTime*15f,0);
		}

		if (Input.GetKey(KeyCode.D)){
			transform.Rotate(0,-Time.deltaTime*15f,0);
		}
	}
}
