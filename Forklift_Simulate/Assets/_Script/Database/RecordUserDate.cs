using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//放在TitleState
public class RecordUserDate : MonoBehaviour
{
    List<string[]> Testlist = new List<string[]>();

    public static ModeChoose modeChoose;

    public enum ModeChoose
    {
        Null,
        VR,
        PC
    }


    void Start()
    {

        UserDatabase.ID = -1;
        UserDatabase.Name = "默認";
        UserDatabase.FinishTime = "默認";
        UserDatabase.PassResult = "默認";
        UserDatabase.WrongAmount = -1;
        UserDatabase.WrongContent = null;

        //讀取
        //Testlist =  saveAndLoad.ReadCsv(path);   
        //Debug.Log(Testlist[0][0]);

    }

    public static void RecordUserData_FirstState(string PassResult, int WrongAmount, string[] ss_userDataRow)
    {
        SaveAndLoad saveAndLoad = new SaveAndLoad();

        //string FirstRow = "座號,姓名,測驗日期,測驗結果,錯誤題數,錯誤題目";
        string nowTime = DateTime.Now + "";
        string userDateForString = "";
        string userDataRow_WrongContent = "";

    

        UserDatabase.FinishTime = nowTime;
        UserDatabase.PassResult = PassResult;
        UserDatabase.WrongAmount = WrongAmount;
        UserDatabase.WrongContent = ss_userDataRow;


        //對入所有欄位
        UserDataRow userDataRow = new UserDataRow(UserDatabase.ID, UserDatabase.Name,
                                                    UserDatabase.FinishTime, UserDatabase.PassResult,
                                                    UserDatabase.WrongAmount, UserDatabase.WrongContent);
        
        foreach (var s in userDataRow.WrongContent)
        {
            userDataRow_WrongContent += s + ",";
        }
        userDateForString = userDataRow.ID + "," +
                            userDataRow.Name + "," +
                            userDataRow.FinishTime + "," +
                            userDataRow.PassResult + "," +
                            userDataRow.WrongAmount + "," +
                            userDataRow_WrongContent;


        //路徑
        var path = Application.streamingAssetsPath + "/One/Record_first.txt";


        ///寫入
        string[] ss = new string[] {userDateForString };
        saveAndLoad.WriteCsv(ss, path);


        //測試
        List<string[]> Testlist = new List<string[]>();
        Testlist = saveAndLoad.ReadCsv(path);
        Debug.Log(Testlist[0][0]);
    }
    public static void RecordUserData_SecondState(string PassResult, int WrongAmount, string[] ss_userDataRow)
    {
        SaveAndLoad saveAndLoad = new SaveAndLoad();

        //string FirstRow = "座號,姓名,測驗日期,測驗結果,錯誤題數,錯誤題目";
        string nowTime = DateTime.Now + "";
        string userDateForString = "";
        string userDataRow_WrongContent = "";



        UserDatabase.FinishTime = nowTime;
        UserDatabase.PassResult = PassResult;
        UserDatabase.WrongAmount = WrongAmount;
        UserDatabase.WrongContent = ss_userDataRow;


        //對入所有欄位
        UserDataRow userDataRow = new UserDataRow(UserDatabase.ID, UserDatabase.Name,
                                                    UserDatabase.FinishTime, UserDatabase.PassResult,
                                                    UserDatabase.WrongAmount, UserDatabase.WrongContent);

        foreach (var s in userDataRow.WrongContent)
        {
            userDataRow_WrongContent += s + ",";
        }
        userDateForString = userDataRow.ID + "," +
                            userDataRow.Name + "," +
                            userDataRow.FinishTime + "," +
                            userDataRow.PassResult + "," +
                            userDataRow.WrongAmount + "," +
                            userDataRow_WrongContent;


        //路徑
        var path = Application.streamingAssetsPath + "/Two/Record_second.txt";


        ///寫入
        string[] ss = new string[] { userDateForString };
        saveAndLoad.WriteCsv(ss, path);


        //測試
        List<string[]> Testlist = new List<string[]>();
        Testlist = saveAndLoad.ReadCsv(path);
        Debug.Log(Testlist[0][0]);
    }

    public static void RecordUserData_ThirdState(string PassResult, int WrongAmount, string[] ss_userDataRow)
    {
        SaveAndLoad saveAndLoad = new SaveAndLoad();

        //string FirstRow = "座號,姓名,測驗日期,測驗結果,錯誤題數,錯誤題目";
        string nowTime = DateTime.Now + "";
        string userDateForString = "";
        string userDataRow_WrongContent = "";



        UserDatabase.FinishTime = nowTime;
        UserDatabase.PassResult = PassResult;
        UserDatabase.WrongAmount = WrongAmount;
        UserDatabase.WrongContent = ss_userDataRow;


        //對入所有欄位
        UserDataRow userDataRow = new UserDataRow(UserDatabase.ID, UserDatabase.Name,
                                                    UserDatabase.FinishTime, UserDatabase.PassResult,
                                                    UserDatabase.WrongAmount, UserDatabase.WrongContent);

        foreach (var s in userDataRow.WrongContent)
        {
            userDataRow_WrongContent += s + ",";
        }
        userDateForString = userDataRow.ID + "," +
                            userDataRow.Name + "," +
                            userDataRow.FinishTime + "," +
                            userDataRow.PassResult + "," +
                            userDataRow.WrongAmount + "," +
                            userDataRow_WrongContent;


        //路徑
        var path = Application.streamingAssetsPath + "/Three/Record_thrid.txt";


        ///寫入
        string[] ss = new string[] { userDateForString };
        saveAndLoad.WriteCsv(ss, path);


        //測試
        List<string[]> Testlist = new List<string[]>();
        Testlist = saveAndLoad.ReadCsv(path);
        Debug.Log(Testlist[0][0]);
    }

}
