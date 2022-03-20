using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToFrontToSeeObj : MonoBehaviour
{
    [SerializeField]
    public Vector3 offsetVect;

    [HideInInspector]
    public Transform OriParentTans;
    [HideInInspector]
    public Vector3 OriLocalPos;
    [HideInInspector]
    public Vector3 OriLocalRot;



    Vector3 RotVect = new Vector3(0, 0, 30);


    // Start is called before the first frame update
    void Start()
    {
        OriParentTans = this.transform.parent;
        OriLocalPos = this.transform.localPosition;
        OriLocalRot = this.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.gameObject.name != OriParentTans.name)
        {
            this.transform.Rotate(RotVect * Time.deltaTime);
        }
    }
}
