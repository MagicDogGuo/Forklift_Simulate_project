using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestionUIComp : MonoBehaviour
{
    [Header("Answer")]
    public Image IconImg;

    public Text TitleTxt;

    public GameObject ChooseZone01;

    public GameObject ChooseZone02;

    public Button ConfirmBtn;

    public Text InfoTxt;

    public Image AnswerBackImage;

    [Header("AllChoose")]
    public GameObject OutlineUIObj;

    public QuestionNumberBtuuon[] QuestionNumberButtons;



    void Start()
    {
        
    }
  
}

[System.Serializable]
public class QuestionNumberBtuuon
{
    public Button QuestionChooseButton;
    public Sprite QuestionIsNoAnswerSprite;
    public Sprite QuestionIsAnswerSprite;


}