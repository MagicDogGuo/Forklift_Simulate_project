using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;
using WSMGameStudio.Vehicles;

public class Chain : MonoBehaviour
{
    [SerializeField]
    GameObject chainObj;

    [SerializeField]
    GameObject StartObj;

    [SerializeField]
    GameObject EndObj;

    [SerializeField]
    GameObject TopChainInside;
    [SerializeField]
    GameObject DownChainInside;

    [SerializeField]
    GameObject TopChainOutside;
    [SerializeField]
    GameObject DownChainOutside;



    float chainDistance = 0.013f;

    float countTime = 0;
    [SerializeField]
    float appearTime_speed = 0.05f;

    int initStartChainCount = 6;
    int initEndChainCount = 133;

    int startChainCountMax = 111;
    int endChainCountMin = 28;

    LogtichControl  logtichControl;
    ForkliftPlayerInput forkliftPlayerInput;
    WSMVehiclePlayerInput wSMVehiclePlayerInput;


    List<GameObject> chainObjList = new List<GameObject>();

    bool isUp=false;

// Start is called before the first frame update
    void Start()
    {
        logtichControl = GetComponentInParent<LogtichControl>();
        forkliftPlayerInput = GetComponentInParent<ForkliftPlayerInput>();
        wSMVehiclePlayerInput = GetComponentInParent<WSMVehiclePlayerInput>();

        chainObjList = new List<GameObject>();

        for (int i = 0; i < initStartChainCount; i++)
        {
            int startChildCount = StartObj.transform.childCount;

            GameObject goStart = Instantiate(chainObj, new Vector3(0, startChildCount * chainDistance, 0), Quaternion.identity);

            goStart.transform.SetParent(StartObj.transform, false);
        }

        for (int i = 0; i < initEndChainCount; i++)
        {
            int endChildCount = EndObj.transform.childCount;

            GameObject goEnd = Instantiate(chainObj, new Vector3(0, endChildCount * chainDistance, 0), Quaternion.identity);

            goEnd.transform.SetParent(EndObj.transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float speedTime = appearTime_speed;
        if (logtichControl.LogitchCluthRotation > 5 || Input.GetKey(wSMVehiclePlayerInput.inputSettings.clutch))
        {
            //採吋動+踩油門
            if (logtichControl.LogitchGasRotation > 10|| Input.GetKey(wSMVehiclePlayerInput.inputSettings.acceleration))
            {
                speedTime = speedTime / 2;
            }
            else
            {
                speedTime = speedTime;
            }
        }

        if (Input.GetKey(forkliftPlayerInput.inputSettings.forksUp))//|| forkliftPlayerInput.logtichControl.ForkUp
        {
            countTime += Time.deltaTime;

            if(countTime> speedTime)
            {
                isUp = true;
                AddChain(StartObj, startChainCountMax, initStartChainCount);
                LessChain(EndObj, initEndChainCount+1, endChainCountMin + 1);

                countTime = 0;
            }
        }

        if (Input.GetKey(forkliftPlayerInput.inputSettings.forksDown))//|| forkliftPlayerInput.logtichControl.ForkDown
        {
            countTime += Time.deltaTime;

            if (countTime > speedTime)
            {
                isUp = false;
                AddChain(EndObj, initEndChainCount, endChainCountMin);
                LessChain(StartObj, startChainCountMax + 1, initStartChainCount+1);

                countTime = 0;
            }

        }

        //過高就刪除
        if (chainObjList.Count > 0)
        {
            foreach (var chainObjs in chainObjList)
            {
                if (chainObjs.transform.position.y > TopChainInside.transform.position.y)
                {
                    GameObject.Destroy(chainObjs);
                    chainObjList.Remove(chainObjs);
                }
            }

       
        }
      


    }


    void AddChain(GameObject addObj,int maxAmount,int minAmount)
    {
        int childCount = addObj.transform.childCount;
        if(childCount <= maxAmount && childCount >= minAmount)
        {
            GameObject go = Instantiate(chainObj, new Vector3(0, childCount * chainDistance, 0), Quaternion.identity);

            go.transform.SetParent(addObj.transform, false);

            chainObjList.Add(go);
        }
  

       // Debug.Log("AddchildCount:" + addObj.name + childCount);
    }

    void LessChain(GameObject lessObj, int maxAmount, int minAmount)
    {
        int childCount = lessObj.transform.childCount;

        if (childCount != 0 && childCount <= maxAmount && childCount >= minAmount)
        {
            GameObject lastChild = lessObj.transform.GetChild(childCount - 1).gameObject;

            if (lastChild.transform.position.y > DownChainInside.transform.position.y && isUp == false)// && isUp==true
            {
                Destroy(lastChild);
                chainObjList.Remove(lastChild);

            }
            else if ( isUp == true)//lastChild.transform.position.y > DownChainOutside.transform.position.y &&
            {
                Destroy(lastChild);
                chainObjList.Remove(lastChild);
            }
            Debug.Log("lastChild.transform.position.y" + lastChild.transform.position.y + "TopChainOutside.transform.position.y" + TopChainOutside.transform.position.y);
        }
        //Debug.Log("LesschildCount:" + lessObj .name+ childCount);

    }
}
