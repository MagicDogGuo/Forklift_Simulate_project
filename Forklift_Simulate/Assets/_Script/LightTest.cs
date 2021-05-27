using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTest : MonoBehaviour
{
    [SerializeField]
    Material R_light;

    Color R_light_em;
    // Start is called before the first frame update
    void Start()
    {
        R_light_em = R_light.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            R_light.SetColor("_EmissionColor", new Color(0, 0, 0, 0));
            //R_light.DisableKeyword("_EMISSION");
        }

        if (Input.GetKey(KeyCode.U))
        {
            R_light.SetColor("_EmissionColor", R_light_em);

            //R_light.EnableKeyword("_EMISSION");
        }
    }
}
