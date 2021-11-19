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

    [Header("AllChoose")]
    public Button[] QuestionChooseButtons;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
