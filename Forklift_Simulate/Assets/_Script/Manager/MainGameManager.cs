using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{

    static MainGameManager m_Instance;
    public static MainGameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType(typeof(MainGameManager)) as MainGameManager;

                if (m_Instance == null)
                {
                    var gameObject = new GameObject(typeof(MainGameManager).Name);
                    m_Instance = gameObject.AddComponent(typeof(MainGameManager)) as MainGameManager;
                }
            }
            return m_Instance;
        }
    }

    [SerializeField]
    GameObject forkleft;


    // 場景狀態
    MainGameStateControl m_MainGameStateController = new MainGameStateControl();
    // 獲取當前的狀態
    public MainGameStateControl.GameFlowState CurrentState
    {
        get { return m_MainGameStateController.GameState; }
    }

    GameObject _forkleftObj;
    public GameObject ForkleftObj
    {
        get { return _forkleftObj; }
    }

    public void MainGameBegin()
    {
        _forkleftObj = forkleft;
        // 設定起始State
        m_MainGameStateController.SetState(MainGameStateControl.GameFlowState.Init, m_MainGameStateController);
      

    }

    public void MainGameUpdate()
    {
        m_MainGameStateController.StateUpdate();
    }


    /// <summary>
    /// 生成主選單初始物件
    /// </summary>
   public void InstantiateInitObject()
   {
        //mainMenuUICanvases = Instantiate(MainGameUICanvas);
     
    }

    public void ExitDestoryObject()
    {
     
    }

  

}
