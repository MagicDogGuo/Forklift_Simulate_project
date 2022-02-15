using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigScene : MonoBehaviour
{
    bool isBig = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBig = !isBig;

            if (isBig)
            {
                this.transform.localScale = new Vector3(10, 10, 10);
            }
            else
            {
                this.transform.localScale = Vector3.one;

            }
        }
    }
}
