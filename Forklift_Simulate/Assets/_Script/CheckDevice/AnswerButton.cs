using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnswerButton : MonoBehaviour
{
    public enum AnswerNumber
    {
        第一皆段_正常 = 1,
        第一皆段_異常 = 2,
        第二皆段_異常01 = 3,
        第二皆段_異常02 = 4,
        第二皆段_異常03 = 5,
        第二皆段_異常04 = 6,
        第二皆段_異常05 = 7,
        第二皆段_異常06 = 8,
        第二皆段_異常07 = 9,
        第二皆段_異常08 = 10, // 異常狀況最多只有到8種(電池清潔液)
    }

    public enum ButtonSelectType
    {
        MultiSelect_btn = 1,
        SigleSelect_btn = 2
    }


    public AnswerNumber answerNumber;
    public ButtonSelectType ButtonType;

    public Sprite IsCheckSprite;
    public Sprite IsNoCheckSprite;

    public string AnswerDiscribe;

    [HideInInspector]
    public bool IsCheck;

    [HideInInspector]
    public int SelectCount;

    private void Start()
    {
        IsCheck = false;
        IsNoSelectAction();
        SelectCount = 0;
        this.GetComponentInChildren<Button>().onClick.AddListener(OnPushThisButton);
        this.GetComponentInChildren<Text>().fontSize = 80;
        this.GetComponentInChildren<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
        this.GetComponentInChildren<Text>().verticalOverflow = VerticalWrapMode.Overflow;

    }


    void OnPushThisButton()
    {
        SelectCount++;

        if (ButtonType == ButtonSelectType.MultiSelect_btn)
        {
            if (SelectCount % 2 == 1)
            {
                IsSelectAction();
            }
            else
            {
                IsNoSelectAction();
            }
        }
        else if (ButtonType == ButtonSelectType.SigleSelect_btn)
        {
            if (SelectCount % 2 == 1)
            {
                IsSelectAction();
                AnswerButton[] answerButtons = FindObjectsOfType<AnswerButton>();
                foreach (var answerBtn in answerButtons)
                {

                    if (answerBtn.gameObject.name != this.gameObject.name &&
                        answerBtn.IsCheck == true)
                    {
                        answerBtn.IsNoSelectAction();
                        answerBtn.SelectCount++;//不按按鈕
                        //answerBtn.GetComponentInChildren<Button>().onClick.Invoke();//會影響到計分邏輯所以不用此方法
                    }
                }
            }
            else
            {
                IsNoSelectAction();
            }  
        }
    }

    void IsSelectAction()
    {
        IsCheck = true;
        this.GetComponentInChildren<Image>().sprite = IsCheckSprite;
        this.GetComponentInChildren<Image>().SetNativeSize();
        this.GetComponentInChildren<Image>().GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(70, -45);
    }

    void IsNoSelectAction()
    {
        IsCheck = false;
        this.GetComponentInChildren<Image>().sprite = IsNoCheckSprite;
        this.GetComponentInChildren<Image>().SetNativeSize();
        this.GetComponentInChildren<Image>().GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(50, -50);


    }
}
