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
    public enum GameMode
    {
        TestMode,
        PracticeMode
    }

    [SerializeField]
    GameObject Forkleft;

    [SerializeField]
    GameObject PipeGroupPos;
    [SerializeField]
    GameObject PipeGroup;

    [SerializeField]
    GameObject InitPlayCam;

    [SerializeField]
    GameObject InitCancvs;
    [SerializeField]
    GameObject InitCancasPos;

    [SerializeField]
    GameObject ScoreGroupCanvas;
    //[SerializeField]
    //GameObject ForkitCanvasPos;


    // 場景狀態
    MainGameStateControl m_MainGameStateController = new MainGameStateControl();
    // 獲取當前的狀態
    public MainGameStateControl.GameFlowState CurrentState
    {
        get { return m_MainGameStateController.GameState; }
    }

    GameObject _initPlayerCam;
    public GameObject InitPlayerCam
    {
        get { return _initPlayerCam; }
    }

    GameMode _gameMode;
    public GameMode GameModes
    {
        get { return _gameMode; }
        set { _gameMode = value; }
    }


    GameObject _forkleftObj;
    public GameObject ForkleftObj
    {
        get { return _forkleftObj; }
    }

    GameObject _pipeGroupObj;
    public GameObject PipeGroupObjs
    {
        get { return _pipeGroupObj; }
    }

    ScoreManager _scoreManager;
    public ScoreManager ScoreManagers
    {
        get { return _scoreManager; }
    }

    int _totalWrongScore;
    public int TotalWrongScore
    {
        get { return _scoreManager.TotalWrongAmount; }
    }

    int _isSussuesPassTest;
    public int IsSussuesPassTest //0沒過 1有過 2測驗中
    {
        get { return _isSussuesPassTest; }
    }

    GameObject _initCanvas;
    public GameObject InitCanvass
    {
        get { return _initCanvas; }
    }

    GameObject _scoreGroupCanvas;
    public GameObject ScoreGroupCanvass
    {
        get { return _scoreGroupCanvas; }
    }
    GameObject _forkitCanvasPos;
    public GameObject ForkitCanvasPoss
    {
        get { return _forkleftObj.GetComponent<ForkUI>().ForkkitCanvasPos; }
    }


    public void MainGameBegin()
    {
        _isSussuesPassTest = 2;
        _initPlayerCam = InitPlayCam;
        _scoreManager = this.GetComponent<ScoreManager>();
        _scoreGroupCanvas = ScoreGroupCanvas;
        //_forkitCanvasPos = ForkitCanvasPos;

        // 設定起始State
        m_MainGameStateController.SetState(MainGameStateControl.GameFlowState.Init, m_MainGameStateController);
    }

    public void MainGameUpdate()
    {
        m_MainGameStateController.StateUpdate();

        //判斷有無過關
        if (TotalWrongScore >= 20)
        {
            _isSussuesPassTest = 0;
        }
    }


    /// <summary>
    /// 生成主選單初始物件
    /// </summary>
   public void InstantiateInitObject()
    {
        _pipeGroupObj = Instantiate(PipeGroup, PipeGroupPos.transform);
        _initCanvas = Instantiate(InitCancvs, InitCancasPos.transform);
        //mainMenuUICanvases = Instantiate(MainGameUICanvas);
   }

    public void CreateForkkit()
    {
        _isSussuesPassTest = 2;

        _forkleftObj = GameObject.Instantiate(Forkleft); 
    }

    public void DestoryForkkit()
    {
        Destroy(_forkleftObj);
    }

    public void ExitDestoryObject()
    {
     
    }

  

}
