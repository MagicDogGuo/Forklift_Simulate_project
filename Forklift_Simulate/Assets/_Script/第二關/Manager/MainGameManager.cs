using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WSMGameStudio.Vehicles;
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
    public GameObject EndPointObj;

    [SerializeField]
    public GameObject StartPointObj;

    [SerializeField]
    public GameObject Forkleft;
    [SerializeField]
    public GameObject Forkleft_VR;

    [SerializeField]
    public Transform ForkitOriPos_start;

    [SerializeField]
    public Transform ForkitOriPos_end;


    [SerializeField]
    public GameObject PipeGroupPos;
    [SerializeField]
    public GameObject PipeGroup;

    [SerializeField]
    public GameObject InitPlayCam;

    [SerializeField]
    public GameObject InitCancvs;
    [SerializeField]
    public GameObject InitCancasPos;

    [SerializeField]
    public GameObject ScoreGroupCanvas;

    [SerializeField]
    public IfForkitOnMe IfForkitOnMe_Road;

    [SerializeField]
    public IfForkitOnMe IfForkitOnMe_RoadOutLine;
    [SerializeField]
    public GameObject WarningUI;

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

    protected GameObject _initPlayerCam;
    public GameObject InitPlayerCam
    {
        get { return _initPlayerCam; }
    }

    protected GameMode _gameMode;
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



    protected GameObject _startPointObj;
    public GameObject StartPointObjs
    {
        get { return _startPointObj; }
    }
    protected GameObject _endPointObj;
    public GameObject EndPointObjs
    {
        get { return _endPointObj; }
    }

    protected GameObject _forkleftObj;
    public GameObject ForkleftObj
    {
        get { return _forkleftObj; }
    }

    public Transform ForkitOriPoss
    {
        get { return ForkitOriPos_start; }
    }

    public Transform ForkitOriPoss_End
    {
        get { return ForkitOriPos_end; }
    }

    protected GameObject _pipeGroupObj;
    public GameObject PipeGroupObjs
    {
        get { return _pipeGroupObj; }
    }

    protected ScoreManager _scoreManager;
    public ScoreManager ScoreManagers
    {
        get { return _scoreManager; }
    }

    protected int _totalWrongScore;
    public int TotalWrongScore
    {
        get { return _scoreManager.TotalWrongAmount; }
    }

    protected int _isSussuesPassTest;
    public int IsSussuesPassTest //0沒過 1有過 2測驗中
    {
        get { return _isSussuesPassTest; }
        set { _isSussuesPassTest = value; }
    }

    protected GameObject _initCanvas;
    public GameObject InitCanvass
    {
        get { return _initCanvas; }
    }

    protected GameObject _scoreGroupCanvas;
    public GameObject ScoreGroupCanvass
    {
        get { return _scoreGroupCanvas; }
    }
    protected GameObject _forkitCanvasPos;
    public GameObject ForkitCanvasPoss
    {
        get { return _forkleftObj.GetComponent<ForkUI>().ForkkitCanvasPos; }
    }

    protected GameObject _warningUI;
    public GameObject WarningUIs
    {
        get { return _warningUI; }

    }

    public bool stepIsWrong_倒車 = false;
   


    WSMVehicleController _wSMVehicleController;

    public void MainGameBegin()
    {
        _isSussuesPassTest = 2;
        _initPlayerCam = InitPlayCam;
        _scoreManager = this.GetComponent<ScoreManager>();
        _scoreGroupCanvas = ScoreGroupCanvas;
        _warningUI = WarningUI;
        //_forkitCanvasPos = ForkitCanvasPos;
        stepIsWrong_倒車 = false;

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
        _startPointObj = Instantiate(StartPointObj, PipeGroupPos.transform);
        _endPointObj = Instantiate(EndPointObj, PipeGroupPos.transform);


        //mainMenuUICanvases = Instantiate(MainGameUICanvas);
    }

    public void CreateForkkit()
    {
        _isSussuesPassTest = 2;

        if(RecordUserDate.modeChoose == RecordUserDate.ModeChoose.PC)
        {
            _forkleftObj = GameObject.Instantiate(Forkleft, ForkitOriPos_start);

        }
        else if(RecordUserDate.modeChoose == RecordUserDate.ModeChoose.VR)
        {
            _forkleftObj = GameObject.Instantiate(Forkleft_VR, ForkitOriPos_start);

        }
        else if (RecordUserDate.modeChoose == RecordUserDate.ModeChoose.Null)
        {
            _forkleftObj = GameObject.Instantiate(Forkleft_VR, ForkitOriPos_start);

        }
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


    public void  WheelBackPos()
    {
        StartCoroutine(DelayWheelBack());
    }

    IEnumerator DelayWheelBack()
    {
        _wSMVehicleController = GameObject.FindObjectOfType<WSMVehicleController>();
        if(_wSMVehicleController!=null)_wSMVehicleController.backUpBeeperSFX.enabled = false;
        LogitechGSDK.LogiPlaySpringForce(0, 0, 50, 50);
        yield return new WaitForSeconds(1.5f);
        if (_wSMVehicleController != null) _wSMVehicleController.backUpBeeperSFX.enabled = true;

        LogitechGSDK.LogiStopSpringForce(0);

    }


}
