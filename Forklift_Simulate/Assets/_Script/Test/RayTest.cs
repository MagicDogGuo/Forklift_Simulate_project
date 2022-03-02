using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class RayTest : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineR;

    [SerializeField]
    GameObject HandCubeObj;

    void Start()
    {

    }

    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            lineR.SetPosition(0, Camera.main.transform.position );//+ new Vector3(1, 0, 0)
            lineR.SetPosition(1, ray.GetPoint(hit.distance));

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(hit.collider.name);
                HandCubeObj.transform.position = hit.point;
            }

        }


    }
 
    /// <summary>
    /// Raycast a ray from touch up or mouse button up and test whether hit transform,
    /// if hit transform return it or return null.
    /// [isCheckHitUI] Whether check ray hit ui transform,
    /// if hit ui just return null.
    /// </summary>
    public  Transform Raycast(bool isCheckHitUI = true)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (isCheckHitUI)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return null;
                }
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (isCheckHitUI)
                {
                    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        return null;
                    }
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform);
                // or hit.collider.transform;
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                lineR.SetPosition(0, Camera.main.transform.position+new Vector3(1,0,0)) ;
                lineR.SetPosition(1, ray.GetPoint(hit.distance));

                if (hit.collider.GetComponent<Button>() != null) hit.collider.GetComponent<Button>().onClick.Invoke();
                                
                return hit.transform;
            }
        }

       
        return null;
    }



    
}
