using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject TestText;

    [SerializeField]
    GameObject EndPointObj;

    [SerializeField]
    GameObject StartPointObj;

    [SerializeField]
    GameObject Forkleft;

    [SerializeField]
    Transform ForkitOriPos;

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

    [SerializeField]
    IfForkitOnMe IfForkitOnMe_Road;

    [SerializeField]
    IfForkitOnMe IfForkitOnMe_RoadOutLine;
    [SerializeField]
    GameObject WarningUI;

    //[SerializeField]
    //GameObject ForkitCanvasPos;

    //public bool isPassPractice = false;
    //public bool isPassTest = false;

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

    public bool IsForkitOnRoad
    {
        get { return IfForkitOnMe_Road.isForkitOnRoad; }
    }

    public bool IsForkitOnRoadOutLine
    {
        get { return IfForkitOnMe_RoadOutLine.isForkitOnRoad; }
    }

    public IfForkitOnMe IsForkitOnRoadObj
    {
        get { return IfForkitOnMe_Road; }
    }

    public IfForkitOnMe IsForkitOnRoadOutLineObj
    {
        get { return IfForkitOnMe_RoadOutLine; }
    }



    GameObject _startPointObj;
    public GameObject StartPointObjs
    {
        get { return _startPointObj; }
    }
    GameObject _endPointObj;
    public GameObject EndPointObjs
    {
        get { return _endPointObj; }
    }

    GameObject _forkleftObj;
    public GameObject ForkleftObj
    {
        get { return _forkleftObj; }
    }

    public Transform ForkitOriPoss
    {
        get { return ForkitOriPos; }
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
        set { _isSussuesPassTest = value; }
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

    GameObject _warningUI;
    public GameObject WarningUIs
    {
        get { return _warningUI; }

    }


    public void MainGameBegin()
    {
        _isSussuesPassTest = 2;
        _initPlayerCam = InitPlayCam;
        _scoreManager = this.GetComponent<ScoreManager>();
        _scoreGroupCanvas = ScoreGroupCanvas;
        _warningUI = WarningUI;
        //_forkitCanvasPos = ForkitCanvasPos;

        // 設定起始State
        m_MainGameStateController.SetState(MainGameStateControl.GameFlowState.Init, m_MainGameStateController);
    }

    public void MainGameUpdate()
    {
        m_MainGameStateController.StateUpdate();

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            TestText.SetActive(!TestText.active);
        }
    }


    /// <summary>
    /// 生成主選單初始物件
    /// </summary>
   public void InstantiateInitObject()
    {
        _pipeGroupObj = Instantiate(PipeGroup, PipeGroupPos.transform);
        _initCanvas = Instantiate(InitCancvs, InitCancasPos.transform);
        _startPointObj = Instantiate(StartPointObj);
        _endPointObj = Instantiate(EndPointObj);


        //mainMenuUICanvases = Instantiate(MainGameUICanvas);
    }

    public void CreateForkkit()
    {
        _isSussuesPassTest = 2;
        _forkleftObj = GameObject.Instantiate(Forkleft, ForkitOriPos); 
    }

    public void DestoryForkkit()
    {
        Destroy(_forkleftObj);
    }

    public void ExitDestoryObject()
    {
        Destroy(_pipeGroupObj);
        Destroy(_initCanvas);
        Destroy(_startPointObj);
        Destroy(_endPointObj);
    }

}
