using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] YesNoQuestion;

    [SerializeField]
    QuestionContent[] questionContent;    

    QuestionUIComp _QuestionUIComp;

    int currentQuestion;

    int currentAnwer = -1;

    Dictionary<int, int> AnswerQuestionDict;

    // Start is called before the first frame update
    void Start()
    {
        _QuestionUIComp = this.GetComponent<QuestionUIComp>();
        _QuestionUIComp.ConfirmBtn.onClick.AddListener(() => OnPushConfirmBtn());

        for (int i = 0;i< _QuestionUIComp.QuestionChooseButtons.Length; i++)
        {
            int temp = i;
            _QuestionUIComp.QuestionChooseButtons[temp].onClick.AddListener(()=>OnPushQuestionChooseButtons(temp));
        }


        AnswerQuestionDict = new Dictionary<int, int>();
    }


    private void Update()
    {
        Debug.Log("currentQuestion" + currentQuestion);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (KeyValuePair<int, int> kvp in AnswerQuestionDict)
                Debug.Log("Key = "+ kvp.Key + " Value ="+ kvp.Value);
        }
    }

    void OnPushQuestionChooseButtons(int i)
    {
        currentQuestion = i;
        _QuestionUIComp.IconImg.sprite = questionContent[i].IconSprtie;
        _QuestionUIComp.TitleTxt.text = questionContent[i].TitleTxt;

        //按鈕圖片恢復
        for (int j = 0; j < _QuestionUIComp.QuestionChooseButtons.Length; j++)
        {
            _QuestionUIComp.QuestionChooseButtons[j].GetComponent<Image>().sprite =
             _QuestionUIComp.QuestionChooseButtons[j].GetComponent<Button>().spriteState.highlightedSprite;
        }
        //更換選中的按鈕
        _QuestionUIComp.QuestionChooseButtons[i].GetComponent<Image>().sprite =
            _QuestionUIComp.QuestionChooseButtons[i].GetComponent<Button>().spriteState.pressedSprite;
      

        //刪除作答區原有的物件
        int oriChildLengh = _QuestionUIComp.ChooseZone01.transform.childCount;
        for(int j = 0; j < oriChildLengh; j++)
        {
            Destroy(_QuestionUIComp.ChooseZone01.transform.GetChild(j).gameObject);
        }

        //新增作答區物件
        int newChildLengh = YesNoQuestion.Length;
        for(int j = 0; j < newChildLengh; j++)
        {
            GameObject go = Instantiate(YesNoQuestion[j], _QuestionUIComp.ChooseZone01.transform);
            int temp = j;
            go.GetComponentInChildren<Button>().onClick.AddListener(() => OnPushYesNoQuestion(temp));
        }
    }

    void OnPushConfirmBtn()
    {
        //先記錄答題
        Debug.Log("currentAnwer:" + currentAnwer);
        Debug.Log("currentQuestion" + currentQuestion);
        
        if (AnswerQuestionDict.ContainsKey(currentQuestion))
        {
            AnswerQuestionDict[currentQuestion] = currentAnwer;
        }
        else
        {
            AnswerQuestionDict.Add(currentQuestion, currentAnwer);
        }

        //再跳到下一題
        //第一層的確認
        if (currentAnwer == 0)
        {//正常
            _QuestionUIComp.QuestionChooseButtons[currentQuestion + 1].onClick.Invoke();

        }
        else if (currentAnwer == 1)
        {//異常

            //子題目長度
            int subQuestionLength = questionContent[currentQuestion].SubQuestChooseBtnObj.Length;

            if (subQuestionLength == 0)
            {//沒有子題目
                _QuestionUIComp.QuestionChooseButtons[currentQuestion + 1].onClick.Invoke();
            }
            else
            {//有子題目
                currentAnwer = -1;

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
                    goSub.GetComponentInChildren<Button>().onClick.AddListener(() => OnPushSubQuestionBtn(temp));
                }
            }
        }

        //第二層子題目的確認
        if(currentAnwer > 1)
        {
            _QuestionUIComp.QuestionChooseButtons[currentQuestion + 1].onClick.Invoke();
        }

     
    }

    void OnPushSubQuestionBtn(int i)
    {
        //從1開始(異常代表1)
        currentAnwer = 1 + (i+1);
    }

    void OnPushYesNoQuestion(int j)
    {
        
        if (j == 0)
        {//正常
            currentAnwer = 0;
        }
        else if (j == 1)
        {//異常
            currentAnwer = 1;
        }
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
