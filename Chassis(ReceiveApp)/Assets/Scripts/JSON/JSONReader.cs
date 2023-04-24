﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*=======================================
 * This script is for reading JSON files.
 *=======================================*/

public static class JSONReader
{
    public static string LoadJson(string path) //Loading JSON.
    {
        string input = "";

        try
        {
            StreamReader reader = new StreamReader(path);
            input = reader.ReadToEnd();
            //MainController.Log(input);
            reader.Close();
        }
        catch (Exception ex)
        {
            MainController.Log($"LoadJson ERROR: {ex.Message}");
        }

        return input;
    }

    public static BundleCatalog LoadBundleCatalog(string path) //Loading bundle configs from JSON.
    {
        BundleCatalog result = null;

        try
        {
            string json = File.ReadAllText(path);

            result = JsonUtility.FromJson<BundleCatalog>(json);
        }
        catch (Exception ex)
        {
            MainController.Log($"LoadBundleCatalog ERROR: {ex.Message}");
        }

        return result;
    }

    public static ConfigCatalog LoadConfigCatalog(string path)
    {
        ConfigCatalog result = null;

        try
        {
            string json = File.ReadAllText(path);

            result = JsonUtility.FromJson<ConfigCatalog>(json);
        }
        catch (Exception ex)
        {
            MainController.Log($"LoadConfigCatalog ERROR: {ex.Message}");
        }

        return result;
    }
}
