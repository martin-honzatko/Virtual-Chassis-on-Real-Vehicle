using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.XR;

/*=================================================================
 * This script is for genererating of a UDP message and its sending.
 * In a version 2 of the application was added ability to 
 * change positioning information to zeros by voice commands. 
 *=================================================================*/

public class UDPGeneration : MonoBehaviour
{
    /*=======================================
     * The start of a space for an variables. 
     *=======================================*/
    public GameObject UDPCommGameObj;
    private GameObject myDisplay;
    private GameObject configMan;

    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    public string dataString = "UDP is real.";
    private float myTime;
    private double speed;
    DateTime now;
    DateTime next;

    private Vector3 myPos;
    private Quaternion myQuaternion;
    /*=====================================
     * The end of a space for an variables. 
     *=====================================*/

    void Start()
    {
        SetConfiguration();
        myDisplay = GameObject.FindGameObjectWithTag("Display");

        myPos.x = (float)UDPCommGameObj.transform.position.x;
        myPos.y = (float)UDPCommGameObj.transform.position.y;
        myPos.z = (float)UDPCommGameObj.transform.position.z;

        myQuaternion.x = (float)UDPCommGameObj.transform.rotation.x;
        myQuaternion.y = (float)UDPCommGameObj.transform.rotation.y;
        myQuaternion.z = (float)UDPCommGameObj.transform.rotation.z;

        Debug.developerConsoleVisible = false; //Deactivating dev console (some errors are not exactly errors and we don't need to see them)

        /*==========Create keywords for keyword recognizer==========*/
        keywords.Add("go zero", () => //Keyword to set position to zero.
        {
            try
            {
                InputTracking.Recenter(); //Changing position & rotation to 0,y,0 & 0,0,0
            }
            catch (Exception ex)
            {
                //MainController.Log($"Error: {ex.Message}");
                Debug.Log($"Error: {ex.Message}");
            }

        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start(); //Start a recognition of keywords.

        if (UDPCommGameObj == null)
        {
            //MainController.Log("ERROR UDPGEN: UDPSender is required. Self-destructing.");
            Destroy(this);
            myTime = Time.time;
        }
        now = DateTime.Now;
        next = DateTime.Now.AddMilliseconds(speed);
    }
    private void SetConfiguration() //Method to set configuration.
    {
        configMan = GameObject.FindGameObjectWithTag("Configuration");
        speed = configMan.GetComponent<ConfigManager>().speed;
    }

#if !UNITY_EDITOR
    async void Update()
    {
        dataString = "x: " + UDPCommGameObj.transform.position.x.ToString("0.00") + "y: " + UDPCommGameObj.transform.position.y.ToString("0.00") + "z: " + UDPCommGameObj.transform.position.z.ToString("0.00") + "a: " + UDPCommGameObj.transform.rotation.eulerAngles.x.ToString("0.00") + "b: " + UDPCommGameObj.transform.rotation.eulerAngles.y.ToString("0.00") + "c: " + UDPCommGameObj.transform.rotation.eulerAngles.z.ToString("0.00") + "t: " + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

        if (dataString != null)
        {
            UDPCommunication comm = UDPCommGameObj.GetComponent<UDPCommunication>();
            if (next < DateTime.Now)
            {
			    await comm.SendMessage(dataString);
                next = DateTime.Now.AddMilliseconds(speed);
            }
        }
    }
#endif

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        /* if the keyword recognized is in our dictionary, call that Action. */
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
