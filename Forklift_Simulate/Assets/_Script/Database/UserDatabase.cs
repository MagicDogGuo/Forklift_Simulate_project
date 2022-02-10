using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDatabase : MonoBehaviour
{
    public static int ID { get; set; }
    public static string Name { get; set; }
    public static string FinishTime { get; set; }
    public static string PassResult { get; set; }
    public static int WrongAmount { get; set; }
    public static string[] WrongContent { get; set; }
}


public class UserDataRow
{
    public  int ID { get; set; }
    public  string Name { get; set; }
    public  string FinishTime { get; set; }
    public  string PassResult { get; set; }
    public  int WrongAmount { get; set; }
    public  string[] WrongContent { get; set; }

    public UserDataRow(int id, string name, string finishTime, string passResult, int wrongAmount, string[] wrongContent)
    {
        ID = id;
        Name = name;
        FinishTime = finishTime;
        PassResult = passResult;
        WrongAmount = wrongAmount;
        WrongContent = wrongContent;
    }
}
