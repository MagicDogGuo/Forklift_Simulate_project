using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class QuestionUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] YesNoQuestion;

    [SerializeField]
    QuestionContent[] questionContent;

    QuestionUIComp _QuestionUIComp;

    int currentQuestion;

    List<int> currentAnwerList;

    Dictionary<int, List<int>> AnswerQuestionDict;


    void Start()
    {
        currentAnwerList = new List<int>();


        _QuestionUIComp = this.GetComponent<QuestionUIComp>();
        _QuestionUIComp.ConfirmBtn.onClick.AddListener(() => OnPushConfirmBtn());

        for (int i = 0; i < _QuestionUIComp.QuestionNumberButtons.Length; i++)
        {
            int temp = i;
            _QuestionUIComp.QuestionNumberButtons[temp].QuestionChooseButton.onClick.AddListener(() => OnPushQuestionChooseButtons(temp));
        }


        AnswerQuestionDict = new Dictionary<int, List<int>>();

        _QuestionUIComp.QuestionNumberButtons[0].QuestionChooseButton.onClick.Invoke();

    }


    private void Update()
    {
       // Debug.Log("currentQuestion" + currentQuestion);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (KeyValuePair<int, List<int>> kvp in AnswerQuestionDict)
            {
                string tmpeS = "";

                foreach (var i in kvp.Value)
                {
                    tmpeS += i + ",";
                }
                Debug.Log("AnswerQuestionDict: Key = " + (CheckDeviceManager.DevicePart)(kvp.Key+1) + " Value =" + tmpeS);
            }
        }

        //更新那些題目已完成UI

        foreach (KeyValuePair<int, List<int>> kvp in AnswerQuestionDict)
        {
            for (int j = 0; j < _QuestionUIComp.QuestionNumberButtons.Length; j++)
            {
                if (j == kvp.Key && kvp.Value != null)//////////////////////有答題且答案不為-1(沒回答完整) 
                {
                    //更換作答的按鈕
                    _QuestionUIComp.QuestionNumberButtons[j].QuestionChooseButton.GetComponent<Image>().sprite =
                        _QuestionUIComp.QuestionNumberButtons[j].QuestionIsAnswerSprite;
                }
            }
        }

    }

    void OnPushQuestionChooseButtons(int i)
    {
        currentQuestion = i;
        _QuestionUIComp.IconImg.sprite = questionContent[i].IconSprtie;
        _QuestionUIComp.TitleTxt.text = questionContent[i].TitleTxt;

        ////////////////////////選題目區/////////////////////////////        
        //所有按鈕外框刪除
        for (int j = 0; j < _QuestionUIComp.QuestionNumberButtons.Length; j++)
        {
            if (_QuestionUIComp.QuestionNumberButtons[j].QuestionChooseButton.transform.childCount > 0)
            {
                Destroy(_QuestionUIComp.QuestionNumberButtons[j].QuestionChooseButton.transform.GetChild(0).gameObject);
            }
        }
        //選中產生外框
        Instantiate(_QuestionUIComp.OutlineUIObj, _QuestionUIComp.QuestionNumberButtons[i].QuestionChooseButton.transform);


        //////////////////////作答區////////////////////////////

        //刪除作答區原有的物件
        int oriChildLengh = _QuestionUIComp.ChooseZone01.transform.childCount;
        for (int j = 0; j < oriChildLengh; j++)
        {
            Destroy(_QuestionUIComp.ChooseZone01.transform.GetChild(j).gameObject);
        }

        //新增作答區物件
        int newChildLengh = YesNoQuestion.Length;
        for (int j = 0; j < newChildLengh; j++)
        {
            int tmep = j;

            GameObject go = Instantiate(YesNoQuestion[tmep], _QuestionUIComp.ChooseZone01.transform);
            go.GetComponentInChildren<Button>().onClick.AddListener(
                () => OnPushYesNoQuestion((int)YesNoQuestion[tmep].GetComponent<AnswerButton>().answerNumber,
                                            YesNoQuestion[tmep].GetComponent<AnswerButton>().ButtonType));
            go.GetComponentInChildren<Text>().text = YesNoQuestion[tmep].GetComponent<AnswerButton>().AnswerDiscribe;
        }
    }

    void OnPushConfirmBtn()
    {
        //先記錄答題(後記錄會影響到下一題)
        Debug.Log("currentAnwer:" + currentAnwerList);
        Debug.Log("currentQuestion" + currentQuestion);

        HashSet<int> _tempCurrentAnwerHash = new HashSet<int>(currentAnwerList);//去除重複數值
        List<int> _tempCurrentAnwerList = new List<int>();
        _tempCurrentAnwerList.Clear();
        _tempCurrentAnwerList.AddRange(_tempCurrentAnwerHash.ToArray()); // 複製list要用這種方式!


        //紀錄答題到Dict
        if (AnswerQuestionDict.ContainsKey(currentQuestion))
        {
            AnswerQuestionDict[currentQuestion] = _tempCurrentAnwerList;
        }
        else
        {
            Debug.Log(_tempCurrentAnwerList.Count);
            AnswerQuestionDict.Add(currentQuestion, _tempCurrentAnwerList);

        }

        //沒填答案時
        if (_tempCurrentAnwerList.Count < 1)
        {
            Debug.Log("答案為空!");
            return;
        }

        //再跳到下一題
        //第一層的確認
        if (_tempCurrentAnwerList.Contains(1))
        {//正常
            Debug.Log("===currentQuestion:"+ currentQuestion);
            _QuestionUIComp.QuestionNumberButtons[currentQuestion + 1].QuestionChooseButton.onClick.Invoke();

        }
        else if (_tempCurrentAnwerList.Contains(2))
        {//異常

            //子題目長度
            int subQuestionLength = questionContent[currentQuestion].SubQuestChooseBtnObj.Length;

            if (subQuestionLength == 0)
            {//沒有子題目
                _QuestionUIComp.QuestionNumberButtons[currentQuestion + 1].QuestionChooseButton.onClick.Invoke();
            }
            else
            {//有子題目

                _tempCurrentAnwerList.Clear();
                Debug.Log("==currentAnwer:" + _tempCurrentAnwerList);
                //紀錄答題到Dict
                if (AnswerQuestionDict.ContainsKey(currentQuestion))
                {
                    AnswerQuestionDict[currentQuestion] = _tempCurrentAnwerList;
                }
                else
                {
                    AnswerQuestionDict.Add(currentQuestion, _tempCurrentAnwerList);
                }

                //刪除作答區原有的物件
                int oriChildLengh = _QuestionUIComp.ChooseZone01.transform.childCount;
                for (int i = 0; i < oriChildLengh; i++)
                {
                    Destroy(_QuestionUIComp.ChooseZone01.transform.GetChild(i).gameObject);
                }
                //新增作答區物件(子問題)
                int newChildLengh = subQuestionLength;
                for (int i = 0; i < newChildLengh; i++)
                {
                    GameObject goSub = Instantiate(questionContent[currentQuestion].SubQuestChooseBtnObj[i], _QuestionUIComp.ChooseZone01.transform);
                    int temp = i;
                    goSub.GetComponentInChildren<Button>().onClick.AddListener(
                        () => OnPushSubQuestionBtn((int)goSub.GetComponent<AnswerButton>().answerNumber,
                                                        goSub.GetComponent<AnswerButton>().ButtonType));
                    goSub.GetComponentInChildren<Text>().text = goSub.GetComponent<AnswerButton>().AnswerDiscribe;
                }
            }
        }

        //第二層子題目的確認
        if (_tempCurrentAnwerList.Count > 0)
        {
            if (_tempCurrentAnwerList[0] > 2)
            {
                Debug.Log("=---------------------");

                _QuestionUIComp.QuestionNumberButtons[currentQuestion + 1].QuestionChooseButton.onClick.Invoke();

            }

        }

        currentAnwerList.Clear();
    }

    void OnPushSubQuestionBtn(int j, AnswerButton.ButtonSelectType buttonSelectType)/////////////////////////////
    {
        if(buttonSelectType == AnswerButton.ButtonSelectType.MultiSelect_btn)
        {
            if (currentAnwerList.Count < 1)
            {
                currentAnwerList.Add(j);

            }
            else
            {
                Debug.Log("currentAnwerList[0]: "+currentAnwerList[0]);
                int[] array = currentAnwerList.ToArray();
                //從3開始
                foreach (int listItem in array)
                {
                    if (j != listItem)
                    { //不重複加入相同數字
                        currentAnwerList.Add(j);
                    }

                }
            }
        }
   
    }

    void OnPushYesNoQuestion(int j , AnswerButton.ButtonSelectType buttonSelectType )
    {
        if(buttonSelectType == AnswerButton.ButtonSelectType.SigleSelect_btn)
        {
            if (currentAnwerList.Count < 1)
            {
                currentAnwerList.Add(j);

            }
            else
            {
                currentAnwerList.Clear();
                currentAnwerList.Add(j);

            }
        }
     
   
        //1=正常
        //2=異常
    }
}

[System.Serializable]
public class QuestionContent
{
    [Header("-------------------------------------------")]
    [SerializeField]
    public int QuestionID;

    [SerializeField]
    public Sprite IconSprtie;

    [SerializeField]
    public string TitleTxt;

    [SerializeField]
    public GameObject[] SubQuestChooseBtnObj;
}
