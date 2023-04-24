using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

/*=======================================================================
 * This script is for loading the configuration from the file.
 *=======================================================================*/

public class ConfigManager : MonoBehaviour
{
    /*=======================================
     * The start of a space for an variables. 
     *=======================================*/
    private GameObject myDisplay;

    public string port;
    public string externalIP;
    /*=====================================
     * The end of a space for an variables. 
     *=====================================*/

    void Start()
    {
        try
        {
            myDisplay = GameObject.FindGameObjectWithTag("Display");
            ConfigCatalog configCatalog = JSONReader.LoadConfigCatalog(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "chassisReceive.json")); //Loading configuration file.

            if (configCatalog == null) //Controling the whether the file exists or not.
            {
                //MainController.Log("Configuration not available");

                myDisplay.GetComponent<TextMeshPro>().text = "Configuration not available! The chassisReceive.json file is not in the picture directory or the file is damaged.";
            }

            else
            {
                SetConfiguration(configCatalog); //Setting configs from the file.
                //MainController.Log("Loaded network configuration.");
            }
        }
        catch (Exception ex)
        {
            MainController.Log($"Network configuration ERROR: {ex.Message}");
        }
    }

    void Update()
    {
        
    }

    private void SetConfiguration(ConfigCatalog configCatalog) //Method to set configuration.
    {
        externalIP = configCatalog.ip;
        port = configCatalog.port;
    }
}
