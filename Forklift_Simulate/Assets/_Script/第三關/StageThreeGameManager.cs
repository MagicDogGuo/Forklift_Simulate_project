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


    ScoreManager _scoreManagers;
    public new ScoreManager ScoreManagers
    {
        get { return _scoreManagers; }
    }

    GameObject _pipeGroupObj;
    public new GameObject PipeGroupObjs
    {
        get { return _pipeGroupObj; }
    }

    GameObject _startPointObj;
    public new GameObject StartPointObjs
    {
        get { return _startPointObj; }
    }

    GameObject _forkleftObj;
    public new GameObject ForkleftObj
    {
        get { return _forkleftObj; }
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


    public bool IsNeerGoods=false;


    // 場景狀態
    MainGameStateControl m_MainGameStateController = new MainGameStateControl();

    // Start is called before the first frame update
    void Start()
    {
        _scoreGroupCanvas = ScoreGroupCanvas;

        m_MainGameStateController.SetState(MainGameStateControl.GameFlowState.Init_stageThree, m_MainGameStateController);
       
        IsNeerGoods = false;
        //InstantiateInitObject_stageThree();

    }

    // Update is called once per frame
    void Update()
    {
        m_MainGameStateController.StateUpdate();

        _scoreManagers.ScoreUpdate();
        //Debug.Log("====IsForkitTouchShelf：" + IsForkitTouchShelf);

    }
    public  void InstantiateInitObject_stageThree()
    {
        _pipeGroupObj = Instantiate(PipeGroup, PipeGroupPos.transform);
        //_startPointObj = Instantiate(StartPointObj, PipeGroupPos.transform);
        //_endPointObj = Instantiate(EndPointObj, PipeGroupPos.transform);
        _scoreManagers = this.GetComponent<ScoreManager>();

        _forkleftObj = GameObject.Instantiate(Forkleft, ForkitOriPos_start);
       
        _scoreManagers.Init();

    }



}
