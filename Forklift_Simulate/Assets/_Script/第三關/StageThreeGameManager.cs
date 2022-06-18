using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageThreeGameManager : MainGameManager
{
    static StageThreeGameManager m_Instance;
    public static StageThreeGameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType(typeof(StageThreeGameManager)) as StageThreeGameManager;

                if (m_Instance == null)
                {
                    var gameObject = new GameObject(typeof(StageThreeGameManager).Name);
                    m_Instance = gameObject.AddComponent(typeof(StageThreeGameManager)) as StageThreeGameManager;
                }
            }
            return m_Instance;
        }
    }

    //[SerializeField]
    //GameObject StartPointObj;

    //[SerializeField]
    //GameObject PipeGroupPos;
    //[SerializeField]
    //GameObject PipeGroup;

    //[SerializeField]
    //GameObject Forkleft;
    //[SerializeField]
    //GameObject Forkleft_VR;

    //[SerializeField]
    //Transform ForkitOriPos_start;

    //[SerializeField]
    //public GameObject ScoreGroupCanvas;

    [SerializeField]
    public GameObject EndPointObj_Goods;

    [SerializeField]
    public GameObject StartPointObj_Goods;

    [SerializeField]
    public GameObject EndPointObj_Goods02;



    //GameObject _pipeGroupObj;
    //public new GameObject PipeGroupObjs
    //{
    //    get { return _pipeGroupObj; }
    //}

    //GameObject _startPointObj;
    //public new GameObject StartPointObjs
    //{
    //    get { return _startPointObj; }
    //}

    //GameObject _forkleftObj;
    //public new GameObject ForkleftObj
    //{
    //    get { return _forkleftObj; }
    //}


    //GameObject _scoreGroupCanvas;
    //public GameObject ScoreGroupCanvass
    //{
    //    get { return _scoreGroupCanvas; }
    //}

    //GameObject _forkitCanvasPos;
    //public GameObject ForkitCanvasPoss
    //{
    //    get { return _forkleftObj.GetComponent<ForkUI>().ForkkitCanvasPos; }
    //}

    public bool IsForkitTouchShelf
    {
        get { return GoodsShelf.isTouchShelf; }
    }

    public bool IsGoodsTooHeight
    {
        get { return Goods.isTooHeight; }
    }

    public bool IsTooFarTo後扶架
    {
        get { return Goods.isTooFarTo後扶架; }
    }

    public bool IsTouchGroundLine
    {
        get { return Goods.isTouchGroundLine; }
    }

    public bool IsGoodsTouchFloor
    {
        get { return Goods.isGoodsTouchFloor; }
    }


    protected GameObject _startPointObj_Goods;
    public GameObject StartPointObj_Goodss
    {
        get { return _startPointObj_Goods; }
    }
    protected GameObject _endPointObj_Goods;
    public GameObject EndPointObj_Goodss
    {
        get { return _endPointObj_Goods; }
    }

    protected GameObject _endPointObj_Goods02;
    public GameObject EndPointObj_Goods02s
    {
        get { return _endPointObj_Goods02; }
    }



    public bool IsNeerGoods=false;


    // 場景狀態
    MainGameStateControl m_MainGameStateController = new MainGameStateControl();

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        _isSussuesPassTest = 2;
        _initPlayerCam = InitPlayCam;
        _scoreManager = this.GetComponent<ScoreManager>();
        _scoreGroupCanvas = ScoreGroupCanvas;
        _warningUI = WarningUI;

        m_MainGameStateController.SetState(MainGameStateControl.GameFlowState.Init_stageThree, m_MainGameStateController);
       
        IsNeerGoods = false;
        //InstantiateInitObject_stageThree();



        //預設練習模式///////////////////////////////
        //_gameMode = GameMode.PracticeMode;

        //MainGameBegin();
    }

    // Update is called once per frame
    void Update()
    {
        m_MainGameStateController.StateUpdate();

        //Debug.Log("====IsForkitTouchShelf：" + IsForkitTouchShelf);

    }
    public  void InstantiateInitObject_stageThree()
    {
        _pipeGroupObj = Instantiate(PipeGroup, PipeGroupPos.transform);
        _initCanvas = Instantiate(InitCancvs, InitCancasPos.transform);
        _startPointObj = StartPointObj;
        //_endPointObj = Instantiate(EndPointObj, PipeGroupPos.transform);
        _startPointObj_Goods = StartPointObj_Goods;
        _endPointObj_Goods = EndPointObj_Goods;
        _endPointObj_Goods02 = EndPointObj_Goods02;


        _scoreManager = this.GetComponent<ScoreManager>();

        //_forkleftObj = GameObject.Instantiate(Forkleft, ForkitOriPos_start);
       


    }



}
