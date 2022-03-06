using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorHand : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MirrorHandFuc();
    }

    void MirrorHandFuc()
    {
        Transform modelTransform = this.transform;
        if (modelTransform != null)
        {
            modelTransform.localScale = new Vector3(modelTransform.localScale.x * -1f, modelTransform.localScale.y, modelTransform.localScale.z);
        }
    }
}
