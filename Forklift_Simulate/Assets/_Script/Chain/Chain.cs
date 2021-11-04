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

    // Start is called before the first frame update
    void Start()
    {
        logtichControl = GetComponentInParent<LogtichControl>();
        forkliftPlayerInput = GetComponentInParent<ForkliftPlayerInput>();
        wSMVehiclePlayerInput = GetComponentInParent<WSMVehiclePlayerInput>();

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

        if (Input.GetKey(forkliftPlayerInput.inputSettings.forksUp) || forkliftPlayerInput.logtichControl.ForkUp)
        {
            countTime += Time.deltaTime;

            if(countTime> speedTime)
            {
                AddChain(StartObj, startChainCountMax, initStartChainCount);
                LessChain(EndObj, initEndChainCount+1, endChainCountMin + 1);

                countTime = 0;
            }
        }

        if (Input.GetKey(forkliftPlayerInput.inputSettings.forksDown) || forkliftPlayerInput.logtichControl.ForkDown)
        {
            countTime += Time.deltaTime;

            if (countTime > speedTime)
            {
                AddChain(EndObj, initEndChainCount, endChainCountMin);
                LessChain(StartObj, startChainCountMax + 1, initStartChainCount+1);

                countTime = 0;
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
        }
  

        Debug.Log("AddchildCount:" + addObj.name + childCount);
    }

    void LessChain(GameObject lessObj, int maxAmount, int minAmount)
    {
        int childCount = lessObj.transform.childCount;

        if (childCount != 0&& childCount <= maxAmount && childCount >= minAmount)
        {
            GameObject lastChild = lessObj.transform.GetChild(childCount - 1).gameObject;
            Destroy(lastChild);
        }
        Debug.Log("LesschildCount:" + lessObj .name+ childCount);

    }
}
