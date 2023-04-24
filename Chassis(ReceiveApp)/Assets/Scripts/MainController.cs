using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*=======================================================
 * This script is for basic management of application(s).
 *=======================================================*/

public static class MainController
{
    /*=======================================
     * The start of a space for an variables. 
     *=======================================*/
    public const string LOCAL_LOG_FILE = "ChassisReceive.log"; //LOG file name.
    /*=====================================
     * The end of a space for an variables. 
     *=====================================*/

    public static void Log(string msg) //add to LOG file.
    {
        DateTime dt = DateTime.Now;
        msg = dt.ToString() + " " + msg;

        try
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), LOCAL_LOG_FILE); //Defining path to file and its name.

            if (!File.Exists(path))
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine(msg);
                    writer.Close();

                    Debug.Log($"Create LOG file : {path} MESSAGE : {msg}");
                }
            }
            else
            {
                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine(msg);
                    writer.Close();

                    Debug.Log($"Add to LOG file : {path} MESSAGE : {msg}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"PolohaSend LOG exists error : {Application.persistentDataPath} /PolohaSend.log \n + {ex.Message}");
        }

    }
}
