using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
public class QuestionUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject TestTxtObj;

    [SerializeField]
    GameObject CameraRigObj;

    [SerializeField]
    GameObject UseTipImage;
    [SerializeField]
    GameObject StartInfoImage;
    [SerializeField]
    GameObject AnswerBackImage;
    [SerializeField]
    GameObject AllChoosBackImage;
    [SerializeField]
    GameObject ResultBackImage;
    [SerializeField]
    Text ResultBack_TimeTxt;


    [SerializeField]
    Button RestartBtn;
    [SerializeField]
    Button checkAnswerBtn;

    [SerializeField]
    CheckDeviceManager checkDeviceManager;

    [SerializeField]
    Text KeyText;


    [SerializeField]
    GameObject[] YesNoQuestion;
    [SerializeField]
    Text TimeTxt;

    [SerializeField]
    QuestionContent[] questionContent;

    [SerializeField]
    GameObject ResultBackImageObj;
    [SerializeField]
    Text ResultContentText;
    [SerializeField]
    Text PassOFailText;


    QuestionUIComp _QuestionUIComp;

    int currentQuestion;

    List<int> currentAnwerList;

    Dictionary<int, List<int>> AnswerQuestionDict;

    AudioSource DiscribeAudioSouce;


    Vector3 CanvasRightPos = new Vector3(2.63f,1.94f,2.3f);
    Vector3 CanvasLeftPos = new Vector3(-2.63f, 1.94f, 2.3f);


    void Start()
    {
        DiscribeAudioSouce = this.GetComponent<AudioSource>();

        currentAnwerList = new List<int>();


        _QuestionUIComp = this.GetComponent<QuestionUIComp>();
        _QuestionUIComp.ConfirmBtn.onClick.AddListener(() => OnPushConfirmBtn());

        for (int i = 0; i < _QuestionUIComp.QuestionNumberButtons.Length; i++)
        {
            int temp = i;          
            _QuestionUIComp.QuestionNumberButtons[temp].QuestionChooseButton.onClick.AddListener(() => OnPushQuestionChooseButtons(temp));            
        }


        AnswerQuestionDict = new Dictionary<int, List<int>>();


        RestartBtn.onClick.AddListener(OnPushRestartBtn);
        checkAnswerBtn.onClick.AddListener(OnPushCheckAnswerBtn);
        UseTipImage.GetComponentInChildren<Button>().onClick.AddListener(OnPushUseTipImageConfirmBtn);
        StartInfoImage.GetComponentInChildren<Button>().onClick.AddListener(OnPushStartInfoConfirmBtn);
        ResultBackImage.GetComponentInChildren<Button>().onClick.AddListener(OnPushResultBackConfirmBtn);

        UseTipImage.SetActive(true);
        StartInfoImage.SetActive(false);
        AnswerBackImage.SetActive(false);
        AllChoosBackImage.SetActive(false);
        ResultBackImage.SetActive(false);



        isTimeUp = false;
    }


    private void Update()
    {
        //測試Debug UI
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TestTxtObj.SetActive(!TestTxtObj.active);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        //UI移動
        if (CameraRigObj.transform.localPosition.x < -1)
        {
            this.GetComponent<RectTransform>().anchoredPosition = CanvasLeftPos;
        }
        else if (CameraRigObj.transform.localPosition.x >= -1)
        {
            this.GetComponent<RectTransform>().anchoredPosition = CanvasRightPos;

        }

        //計時
        if (AnswerBackImage.active == true && AllChoosBackImage.active == true)
        {
            CountTime(900);
        }
        Clock();


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
                Debug.Log("AnswerQuestionDict: Key = " + (CheckDeviceManager.DevicePart)(kvp.Key) + " Value =" + tmpeS);
            }
        }

        //更新那些題目已完成UI
        foreach (KeyValuePair<int, List<int>> kvp in AnswerQuestionDict)
        {
            for (int j = 0; j < _QuestionUIComp.QuestionNumberButtons.Length; j++)
            {
                if (j+1 == kvp.Key && kvp.Value.Count > 0 )//////////////////////有答題且數量大於0(有答完整) ，J+1因為Question由1開始
                {
                    if (kvp.Value.Contains(1))
                    {//作答是正常
                     //更換作答的按鈕
                        _QuestionUIComp.QuestionNumberButtons[j].QuestionChooseButton.GetComponent<Image>().sprite =
                        _QuestionUIComp.QuestionNumberButtons[j].QuestionIsAnswerSprite;
                        _QuestionUIComp.QuestionNumberButtons[j].QuestionChooseButton.GetComponent<Image>().color = Color.white;

                    }
                    else
                    {//作答是異常
                     //更換作答的按鈕
                        _QuestionUIComp.QuestionNumberButtons[j].QuestionChooseButton.GetComponent<Image>().sprite =
                           _QuestionUIComp.QuestionNumberButtons[j].QuestionIsNoAnswerSprite;
                        _QuestionUIComp.QuestionNumberButtons[j].QuestionChooseButton.GetComponent<Image>().color = Color.red;
                    }

                
                }
            }
        }


        //測驗模式發動引擎，直接結算
        if(CheckDeviceManager.isLetEngineOn == true)
        {
            CheckDeviceManager.isLetEngineOn = false;
            OnPushCheckAnswerBtn();
        }

    }
    void OnPushLastQuestionBtn()
    {
        _QuestionUIComp.ConfirmBtn.gameObject.SetActive(false);
        _QuestionUIComp.ChooseZone01.SetActive(false);
        _QuestionUIComp.InfoTxt.gameObject.SetActive(true);
    }

    void OnPushRestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnPushCheckAnswerBtn()
    {
        //鑰匙
        if (HandelContorller.isKey_onTable == false)
        {
            checkDeviceManager.IsNoBackKey = true;
            //KeyText.enabled = true;
        }
        else
        {
            checkDeviceManager.IsNoBackKey = false;
            //KeyText.enabled = false;
        }

        //安全帶
        GameObject SeatBeltLong = GameObject.Find("seat belt_belt");
        if (SeatBeltLong.GetComponent<MeshRenderer>().enabled == true) checkDeviceManager.IsNoBackSetbelt = true;
        else checkDeviceManager.IsNoBackSetbelt = false;

        //大燈
        if (HandelContorller.isBackBigLight == false) checkDeviceManager.IsNoBackBigLight = true;
        else checkDeviceManager.IsNoBackBigLight = false;

        //方向燈
        if (HandelContorller.isBackDirLight == false) checkDeviceManager.IsNoBackDirLight = true;
        else checkDeviceManager.IsNoBackDirLight = false;


        //手煞
        if (HandelContorller.isBackHandbrake == false) checkDeviceManager.IsNoBackHandBrake = true;
        else checkDeviceManager.IsNoBackHandBrake = false;


        //後退檔
        if (HandelContorller.isBackFrontBack == false) checkDeviceManager.IsNoBackFrontBack = true;
        else checkDeviceManager.IsNoBackFrontBack = false;


        CheckAnswer();
        UseTipImage.SetActive(false);
        StartInfoImage.SetActive(false);
        AnswerBackImage.SetActive(false);
        AllChoosBackImage.SetActive(false);
        ResultBackImage.SetActive(true);

    }

    void OnPushUseTipImageConfirmBtn()
    {
        UseTipImage.SetActive(false);
        StartInfoImage.SetActive(true);
        AnswerBackImage.SetActive(false);
        AllChoosBackImage.SetActive(false);
        ResultBackImage.SetActive(false);
    }

    void OnPushStartInfoConfirmBtn()
    {
        //第一題開始
        _QuestionUIComp.QuestionNumberButtons[0].QuestionChooseButton.onClick.Invoke();

        UseTipImage.SetActive(false);
        StartInfoImage.SetActive(false);
        AnswerBackImage.SetActive(true);
        AllChoosBackImage.SetActive(true);
        ResultBackImage.SetActive(false);
    }

    void OnPushResultBackConfirmBtn()
    {
        UseTipImage.SetActive(false);
        StartInfoImage.SetActive(false);
        AnswerBackImage.SetActive(true);
        AllChoosBackImage.SetActive(true);
        ResultBackImage.SetActive(false);
    }

 
    /// <summary>
    /// 對答案
    /// </summary>
    void CheckAnswer()
    {
        Dictionary<int, List<int>> _CorrectAnswerDict = checkDeviceManager.CorrectAnswerDict;
        Dictionary<int, List<int>> _AnswerQuestionDict = AnswerQuestionDict;

        //壞掉的部位
        Dictionary<int, List<int>> _BreakDeviceInCorrectAnswerDict = checkDeviceManager.BreakDeviceInCorrectAnswerDict;
        //有答對且壞掉的部位
        Dictionary<int, List<int>> _MatchAnswerBreakDeviceDict = new Dictionary<int, List<int>>();
        //作答後跟答案不符合的題目
        Dictionary<int, List<int>> _UnMatchCorrectAnswerDict = new Dictionary<int, List<int>>();
        

        //正解有但作答裡沒有的key
        var keys_CorrectAnswerDictHasThat_AnswerQuestionDictDoesNot = _CorrectAnswerDict.Keys.Except(_AnswerQuestionDict.Keys);


        //記錄存檔
        List<string> ss_userDataRowList =new List<string>();
        string[] ss_userDataRowArray ;
        string PassResult;
        int WrongAmount;


        //Debug.Log("---" + _CorrectAnswerDict.Keys.SequenceEqual(_AnswerQuestionDict.Keys)+"____"+ keys_CorrectAnswerDictHasThat_AnswerQuestionDictDoesNot.GetType());

        //foreach(var i in keys_CorrectAnswerDictHasThat_AnswerQuestionDictDoesNot)
        //{
        //    Debug.Log("_" + i + "__"+ _CorrectAnswerDict[i]);
        //}

        //檢視所有答案對錯
        foreach (KeyValuePair<int, List<int>> kvp in _CorrectAnswerDict)
        {
            if (_AnswerQuestionDict.ContainsKey(kvp.Key))
            {
                List<int> list1 = new List<int> (_AnswerQuestionDict[kvp.Key]);
                List<int> list2 = new List<int>(kvp.Value);
                List<int> firstNotSecond = list1.Except(list2).ToList();
                List<int> secondNotFirst = list2.Except(list1).ToList();
                if (firstNotSecond.Count > 0 || secondNotFirst.Count >0)
                {//有錯的選項
                    string s = "";

                    if (firstNotSecond.Count > 0)
                    {
                        foreach (var i in firstNotSecond)
                        {
                            s += i + ",";
                        }
                    }
                    else if(secondNotFirst.Count > 0)
                    {
                        foreach (var i in secondNotFirst)
                        {
                            s += i + ",";
                        }
                    }
                    //儲存答錯的內容
                    _UnMatchCorrectAnswerDict.Add(kvp.Key, _AnswerQuestionDict[kvp.Key]);

                    Debug.Log((CheckDeviceManager.DevicePart)kvp.Key + "_答案錯:"+s);

                    //Debug.Log("kvp.Key:" + kvp.Key + "_AnswerQuestionDict[kvp.Key][0]:" + _AnswerQuestionDict[kvp.Key][0] + " kvp.Value:" + kvp.Value[0]);
                }
                else
                {//選項對
                    Debug.Log((CheckDeviceManager.DevicePart)kvp.Key+"_答案對");

                    //刪除在答錯清單裡的key
                    if (_UnMatchCorrectAnswerDict.ContainsKey(kvp.Key))
                    {
                        Debug.Log("===========================================================");
                        _UnMatchCorrectAnswerDict.Remove(kvp.Key);
                    }

                    //有答對且有在部位損壞清單裡，就加入有答對損壞部位Dict
                    if (_BreakDeviceInCorrectAnswerDict.ContainsKey(kvp.Key))
                    {
                        //儲存答對且有在部位損壞清單的內容
                        _MatchAnswerBreakDeviceDict.Add(kvp.Key, _AnswerQuestionDict[kvp.Key]);
                    }
                }
            }
            else
            {
                //沒答先全部存是錯的
                _UnMatchCorrectAnswerDict.Add(kvp.Key, new List<int>());

                Debug.Log("_AnswerQuestionDict沒有kvp.Key:" + kvp.Key);
            }    
        }

        //顯示UI
        //排序
        Dictionary<int, List<int>> _BreakDeviceInCorrectAnswerDictSort = new Dictionary<int, List<int>>();
        foreach (KeyValuePair<int, List<int>> author in _BreakDeviceInCorrectAnswerDict.OrderBy(key => key.Key))
        {
            _BreakDeviceInCorrectAnswerDictSort.Add(author.Key, author.Value);
        }
        string resultContent = "異常項目:\n";
        foreach (KeyValuePair<int, List<int>> kvp in _BreakDeviceInCorrectAnswerDictSort)
        {
            if (questionContent.Length >0)
            {
                if (_MatchAnswerBreakDeviceDict.ContainsKey(kvp.Key))
                {//有答對的損壞部位
                    resultContent += "<color=green>" + kvp.Key + "." + questionContent[kvp.Key - 1].TitleTxt + "</color>\n";
                }
                else
                {//沒有答對的損壞部位
                    resultContent += "<color=red>" + kvp.Key + "." + questionContent[kvp.Key - 1].TitleTxt + "</color>\n";
                }
            }
          

        }


        //ResultContentText.text = resultContent;


        //判斷有無通過
        string unPassNO ="\n總共錯誤:"+ _UnMatchCorrectAnswerDict.Count + "題\n答錯的題目:\n";
        //判斷有無讓鑰匙轉90度啟動引擎
        if (checkDeviceManager.IsKeyRot90Degree)
        {//未通過
            unPassNO += "<color=red>A.誤觸引擎</color>\n";
        }
        Debug.Log("checkDeviceManager.IsOverTime"+ checkDeviceManager.IsOverTime);

        if (checkDeviceManager.IsOverTime)
        {
            unPassNO += "<color=red>B.超時</color>\n";
        }

        if (checkDeviceManager.IsNoBackKey)
        {
            unPassNO += "<color=red>C.鑰匙未歸位</color>\n";
        }
        if (checkDeviceManager.IsNoBackSetbelt)
        {
            unPassNO += "<color=red>D.安全帶未歸位</color>\n";

        }
        if (checkDeviceManager.IsNoBackBigLight)
        {
            unPassNO += "<color=red>E.大燈未歸位</color>\n";

        }
        if (checkDeviceManager.IsNoBackDirLight)
        {
            unPassNO += "<color=red>F.方向燈未歸位</color>\n";

        }
        if (checkDeviceManager.IsNoBackHandBrake)
        {
            unPassNO += "<color=red>G.手剎車未歸位</color>\n";

        }
        if (checkDeviceManager.IsNoBackFrontBack)
        {
            unPassNO += "<color=red>H.排檔未歸位</color>\n";

        }


        foreach (KeyValuePair<int, List<int>> kvp in _UnMatchCorrectAnswerDict)
        {//與正解不合的題目
            
            string userAns = "";
            string currnetAns = "";

            for (int i = 0; i < _UnMatchCorrectAnswerDict.Count; i++)
            {
                //user的答案
                userAns = "";
                if (_UnMatchCorrectAnswerDict[kvp.Key].Count < 1)
                {
                    userAns += "未作答";
                }
                else
                {
                    if (_UnMatchCorrectAnswerDict[kvp.Key][0] == 1)
                    {
                        userAns += "正常";
                    }
                    else if(_UnMatchCorrectAnswerDict[kvp.Key][0] == 2)
                    {
                        userAns += "異常";
                    }
                    else
                    {                        
                        foreach(var j in _UnMatchCorrectAnswerDict[kvp.Key])
                        {
                            int subAnswerNo = j - 3;
                            string subAnswerDiscript = 
                                questionContent[kvp.Key - 1].SubQuestChooseBtnObj[subAnswerNo].GetComponent<AnswerButton>().AnswerDiscribe;

                            if (_UnMatchCorrectAnswerDict[kvp.Key].Count > 1) {
                                if (_UnMatchCorrectAnswerDict[kvp.Key].IndexOf(j) == (_UnMatchCorrectAnswerDict[kvp.Key].Count - 1)) userAns += subAnswerDiscript;
                                else userAns += subAnswerDiscript + " ";
                            } 
                            else userAns += subAnswerDiscript;
                        }
                    }
                }
                //正確的答案
                currnetAns = ""; 
                if (_CorrectAnswerDict[kvp.Key][0] == 1)
                {
                    currnetAns += "正常";
                }
                else if (_CorrectAnswerDict[kvp.Key][0] == 2)
                {
                    currnetAns += "異常";
                }
                else
                {
                    foreach (var j in _CorrectAnswerDict[kvp.Key])
                    {
                        int subAnswerNo = j - 3;
                        string subAnswerDiscript =
                            questionContent[kvp.Key - 1].SubQuestChooseBtnObj[subAnswerNo].GetComponent<AnswerButton>().AnswerDiscribe;

                        if (_CorrectAnswerDict[kvp.Key].Count > 1) {
                            if(_CorrectAnswerDict[kvp.Key].IndexOf(j) == (_CorrectAnswerDict[kvp.Key].Count-1)) currnetAns += subAnswerDiscript;
                            else currnetAns += subAnswerDiscript + " ";
                        } 
                        else currnetAns += subAnswerDiscript;
                    }
                }
                             
            }

            string wrongRow = kvp.Key + "." + questionContent[kvp.Key - 1].TitleTxt + "，<color=red>您的答案:" + userAns + "</color> / <color=green>正確答案:" + currnetAns + "</color>\n";
            unPassNO += wrongRow;
            
            ss_userDataRowList.Add(wrongRow.Replace("<color=red>","").
                Replace("</color>","").Replace(" <color=green>","").Replace("\n",""));

        }
        //調整文字框大小
        ResultContentText.GetComponent<RectTransform>().sizeDelta = new Vector2(1481.89f, 86 * (_UnMatchCorrectAnswerDict.Count + 4));

       
        if (_UnMatchCorrectAnswerDict.Count < 3 &&
            !checkDeviceManager.IsKeyRot90Degree &&
             !checkDeviceManager.IsOverTime &&
              !checkDeviceManager.IsNoBackKey &&
               !checkDeviceManager.IsNoBackSetbelt &&
                !checkDeviceManager.IsNoBackDirLight &&
                !checkDeviceManager.IsNoBackHandBrake &&
                !checkDeviceManager.IsNoBackFrontBack &&
            !checkDeviceManager.IsOverTime)//與答案不符合的題目數量
        {
            //PassOFailText.text = "<color=green>通過!</color>" + unPassNO;
            ResultContentText.text = "<color=green>通過!</color>" + unPassNO;
            PassResult = "通過";
        }
        else
        {
            //PassOFailText.text = "<color=red>未通過!</color> " + unPassNO;
            ResultContentText.text = "<color=red>未通過!</color> " + unPassNO;
            ResultContentText.GetComponent<RectTransform>().sizeDelta = new Vector2(1481.89f, 86 * (_UnMatchCorrectAnswerDict.Count + 4)+86);
            PassResult = "未通過";
        }

        //存檔
        WrongAmount = _UnMatchCorrectAnswerDict.Count;
        ss_userDataRowArray = ss_userDataRowList.ToArray();
        RecordUserDate.RecordUserData_FirstState(PassResult, WrongAmount, ss_userDataRowArray);
    }

    IEnumerator DalayPlayAS()
    {
        DiscribeAudioSouce.Stop();
        yield return new WaitForSeconds(0.7f);
        DiscribeAudioSouce.Play();
    }

    void OnPushQuestionChooseButtons(int i)////////////////////////////////////////////////////////////
    {
     
        _QuestionUIComp.ConfirmBtn.gameObject.SetActive(true);


        _QuestionUIComp.ChooseZone01.SetActive(true);
        _QuestionUIComp.InfoTxt.gameObject.SetActive(false);

        currentAnwerList.Clear();
        currentQuestion = i;
        _QuestionUIComp.IconImg.sprite = questionContent[i].IconSprtie;
        _QuestionUIComp.TitleTxt.text = questionContent[i].TitleTxt;
        DiscribeAudioSouce.clip = questionContent[i].DiscririonAuioClip;
        StartCoroutine(DalayPlayAS());

        ////////////////////////選題目區/////////////////////////////        
        //所有按鈕外框刪除
        for (int j = 0; j < _QuestionUIComp.QuestionNumberButtons.Length; j++)
        {

            if (_QuestionUIComp.QuestionNumberButtons[j].QuestionChooseButton.transform.childCount > 0)
            {
                Destroy(_QuestionUIComp.QuestionNumberButtons[j].QuestionChooseButton.transform.GetChild(0).gameObject);
                _QuestionUIComp.QuestionNumberButtons[j].QuestionChooseButton.interactable = true;

            }
        }
        //選中產生外框
        Instantiate(_QuestionUIComp.OutlineUIObj, _QuestionUIComp.QuestionNumberButtons[i].QuestionChooseButton.transform);
        _QuestionUIComp.QuestionNumberButtons[i].QuestionChooseButton.interactable = false;

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

        //已作答過的選項
        StartCoroutine(IEDelayDoAnsweredQuestion(i));

        //動畫
        StartCoroutine(ChangeAnim());
    }

    /// <summary>
    /// 填上已做答過的題目
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    IEnumerator IEDelayDoAnsweredQuestion(int i)
    {
        yield return new WaitForEndOfFrame();
        foreach (KeyValuePair<int, List<int>> kvp in AnswerQuestionDict)
        {
            if (i + 1 == kvp.Key)
            {
                string s = "";
                foreach (var v in kvp.Value)
                {
                    s += v + ",";
                }

                Debug.Log("做過這題 答案是填:" + s);
                if (kvp.Value.Count > 0)//有答案
                {
                    if (kvp.Value[0] < 3)
                    {//只有第一皆的答案
                        if (kvp.Value[0] == 1)
                        {//正常
                            int InstanceDoneNewChildLengh = _QuestionUIComp.ChooseZone01.transform.childCount;
                            for (int j = 0; j < InstanceDoneNewChildLengh; j++)
                            {
                                if (j + 1 == kvp.Value[0])//對應的按鈕
                                {
                                    _QuestionUIComp.ChooseZone01.transform.GetChild(j).GetComponentInChildren<Button>().onClick.Invoke();
                                }
                            }
                        }
                        else if (kvp.Value[0] == 2)
                        {//異常
                            int InstanceDoneNewChildLengh = _QuestionUIComp.ChooseZone01.transform.childCount;
                            for (int j = 0; j < InstanceDoneNewChildLengh; j++)
                            {
                                if (j + 1 == kvp.Value[0])//對應的按鈕
                                {
                                    _QuestionUIComp.ChooseZone01.transform.GetChild(j).GetComponentInChildren<Button>().onClick.Invoke();
                                }
                            }
                        }

                    }
                    else if (kvp.Value[0] >= 3)
                    {//有第二階的答案(就一定是異常)
                        int InstanceDoneNewChildLengh = _QuestionUIComp.ChooseZone01.transform.childCount;
                        for (int j = 0; j < InstanceDoneNewChildLengh; j++)
                        {
                            if (j + 1 == 2)//一定是異常
                            {
                                _QuestionUIComp.ChooseZone01.transform.GetChild(j).GetComponentInChildren<Button>().onClick.Invoke();
                            }
                        }
                        //每次重製，暫存第二階答案
                        _tempReocrd第二階答案List = new List<int>();
                        _tempReocrd第二階答案List.Clear();
                        _tempReocrd第二階答案List = kvp.Value;
                    }
                }
            
            }
        }
    }

    IEnumerator ChangeAnim()
    {
        yield return new WaitForEndOfFrame();
        float scale = 1;
        while (scale > 0.01)
        {
            scale -= 0.1f;
            yield return new WaitForSeconds(0.01f);
            _QuestionUIComp.AnswerBackImage.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
        }
        yield return new WaitForSeconds(0.4f);
        _QuestionUIComp.AnswerBackImage.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

    }

    void OnPushConfirmBtn()
    {
        //先記錄答題(後記錄會影響到下一題)
        //Debug.Log("currentAnwer:" + currentAnwerList);
        //Debug.Log("currentQuestion" + currentQuestion);

        HashSet<int> _tempCurrentAnwerHash = new HashSet<int>(currentAnwerList);//去除重複數值
        List<int> _tempCurrentAnwerList = new List<int>();
        _tempCurrentAnwerList.Clear();
        _tempCurrentAnwerList.AddRange(_tempCurrentAnwerHash.ToArray()); // 複製list要用這種方式!
        _tempCurrentAnwerList.Sort();

        int QuestionID = currentQuestion + 1;

        //紀錄答題到Dict
        if (AnswerQuestionDict.ContainsKey(QuestionID))
        {
            AnswerQuestionDict[QuestionID] = _tempCurrentAnwerList;
        }
        else
        {
            Debug.Log(_tempCurrentAnwerList.Count);
            AnswerQuestionDict.Add(QuestionID, _tempCurrentAnwerList);

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
            if(QuestionID != 20) _QuestionUIComp.QuestionNumberButtons[currentQuestion + 1].QuestionChooseButton.onClick.Invoke();
            if(QuestionID == 20 ) OnPushLastQuestionBtn();

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
                //清空後的list紀錄答題到Dict
                if (AnswerQuestionDict.ContainsKey(QuestionID))
                {
                    AnswerQuestionDict[QuestionID] = _tempCurrentAnwerList;
                }
                else
                {
                    AnswerQuestionDict.Add(QuestionID, _tempCurrentAnwerList);
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
                _QuestionUIComp.TitleTxt.text += "異常";
                StartCoroutine(IEDelayDoSubAnsweredQuestion(QuestionID));

              
            }
        }

        //第二層子題目的確認
        if (_tempCurrentAnwerList.Count > 0)
        {
            if (_tempCurrentAnwerList[0] > 2 )
            {
                Debug.Log("=---------------------");
                if( QuestionID != 20) _QuestionUIComp.QuestionNumberButtons[currentQuestion + 1].QuestionChooseButton.onClick.Invoke();
            }
            if (QuestionID == 20)
            {
                OnPushLastQuestionBtn();
            }
        }
        currentAnwerList.Clear();
    }

    List<int> _tempReocrd第二階答案List;
    IEnumerator IEDelayDoSubAnsweredQuestion(int i)
    {
        yield return new WaitForEndOfFrame();


        if (_tempReocrd第二階答案List!=null)//有做過題目的
        {
            string s = "";
            foreach (var v in _tempReocrd第二階答案List)
            {
                s += v + ",";
            }

            Debug.Log("做過這題 答案是填:" + s);

            if (_tempReocrd第二階答案List.Count > 0)//有作答了
            {
                if (_tempReocrd第二階答案List[0] >= 3)
                {//第二階的答案
                    foreach (var subAnswer in _tempReocrd第二階答案List)
                    {
                        int InstanceDoneNewChildLengh = _QuestionUIComp.ChooseZone01.transform.childCount;
                        for (int j = 0; j < InstanceDoneNewChildLengh; j++)
                        {
                            if (j == (subAnswer-3))//一定是異常，-3因為從3開始算
                            {
                                _QuestionUIComp.ChooseZone01.transform.GetChild(j).GetComponentInChildren<Button>().onClick.Invoke();
                            }
                        }
                    }
                }
            }
                 
        }
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
                if (currentAnwerList.Contains(j))
                {
                    Debug.Log("======================Remove:" + j + currentAnwerList.Remove(j));
                    currentAnwerList.Remove(j);

                }
                else
                {
                    currentAnwerList.Add(j);
                }
            }
        }
        if (buttonSelectType == AnswerButton.ButtonSelectType.SigleSelect_btn)
        {
            if (currentAnwerList.Count < 1)
            {//沒作答過
                currentAnwerList.Add(j);
            }
            else
            {//有作答過
                if (currentAnwerList.Contains(j))
                {
                    Debug.Log("======================Clear");
                    currentAnwerList.Clear();
                }
                else
                {
                    currentAnwerList.Clear();
                    currentAnwerList.Add(j);
                }


            }
        }
    }

    void OnPushYesNoQuestion(int j , AnswerButton.ButtonSelectType buttonSelectType )
    {
        if (buttonSelectType == AnswerButton.ButtonSelectType.MultiSelect_btn)
        {
            if (currentAnwerList.Count < 1)
            {
                currentAnwerList.Add(j);

            }
            else
            {
                if (currentAnwerList.Contains(j))
                {
                    Debug.Log("======================Remove:" + j + currentAnwerList.Remove(j));
                    currentAnwerList.Remove(j);

                }
                else
                {
                    currentAnwerList.Add(j);
                }
            }
        }
        if (buttonSelectType == AnswerButton.ButtonSelectType.SigleSelect_btn)
        {
            if (currentAnwerList.Count < 1)
            {//沒作答過
                currentAnwerList.Add(j);
            }
            else
            {//有作答過
                if(currentAnwerList.Contains(j))
                {
                    Debug.Log("======================Clear");
                    currentAnwerList.Clear();
                }
                else
                {
                    currentAnwerList.Clear();
                    currentAnwerList.Add(j);
                }


            }
        }

        Debug.Log("currentAnwerList.Count" + currentAnwerList.Count);
     
   
        //1=正常
        //2=異常
    }


    private float m_Timer;
    private int m_Hour;//時
    private int m_Minute;//分
    private int m_Second;//秒
    bool isTimeUp;

    /// <summary>
    /// 計時
    /// </summary>
    /// <param name="limitSec"></param>
    void CountTime(int limitSec)
    {
         m_Timer += Time.deltaTime;
        if (m_Timer > limitSec && isTimeUp == false)
        {
            isTimeUp = true;
            checkDeviceManager.isOverTime(true);
        }
        else if(m_Timer <= limitSec)
        {
            checkDeviceManager.isOverTime(false);
        }
    }

    void Clock()
    {
        m_Second = (int)m_Timer;
        if (m_Second > 59.0f)
        {
            m_Second = (int)(m_Timer - (m_Minute * 60) -(m_Hour * 3600));
        }
        m_Minute = (int)(m_Timer / 60);
        if (m_Minute > 59.0f)
        {
            m_Minute = (int)(m_Minute - (m_Hour * 60));
        }
        m_Hour =(int) m_Timer / 3600;
        if (m_Hour >= 24.0f)
        {
            m_Timer = 0;
        }

        //UI
        TimeTxt.text = string.Format("經過時間：{0:d2}:{1:d2}:{2:d2}", m_Hour, m_Minute, m_Second);
        ResultBack_TimeTxt.text = string.Format("總共用時：{0:d2}:{1:d2}:{2:d2}", m_Hour, m_Minute, m_Second);
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
    public AudioClip DiscririonAuioClip;

    [SerializeField]
    public GameObject[] SubQuestChooseBtnObj;
}
