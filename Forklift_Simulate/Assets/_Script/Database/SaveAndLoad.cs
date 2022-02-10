using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;
using System.Text;


//https://zhangbbsday.github.io/2019/11/03/csv-read-and-write/
public class SaveAndLoad
{

    static string fileName = "";


    private StreamReader Read(string path)
    {
        if (path == null)
            return null;
        path += fileName;
        if (!File.Exists(path))
            File.CreateText(path);
        return new StreamReader(path);
    }

    /// <summary>
    /// load
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public List<string[]> ReadCsv(string path)
    {
        List<string[]> list = new List<string[]>();
        string line;
        StreamReader stream = Read(path);
        while ((line = stream.ReadLine()) != null)
        {
            list.Add(line.Split(','));
        }
        stream.Close();
        stream.Dispose();
        return list;
    }


    private StreamWriter Write(string path)
    {
        if (path == null)
            return null;
        path += fileName;
        if (!File.Exists(path))
            File.CreateText(path);
        return new StreamWriter(path,true); //true = append
    }

    /// <summary>
    /// save
    /// </summary>
    /// <param name="strs"></param>
    /// <param name="path"></param>
    public void WriteCsv(string[] strs, string path)
    {
        StreamWriter stream = Write(path);
        for (int i = 0; i < strs.Length; i++)
        {
            if (strs[i] != null)
                stream.WriteLine($"{strs[i]}");//{(i + 1).ToString()},
        }
        stream.Close();
        stream.Dispose();
    }

  
}
