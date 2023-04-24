using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using TMPro;
#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Networking.Connectivity;
using Windows.Networking;
#endif

/*======================================================================================================
 *This script is giving us ability to send/receive a UDP packets to/from a server. 
 *But in this use case (this app) we are using just an receive option.
 *We are using this to receive positioning informations from an another HoloLens 2 device (on real vehicle)
 *to project a virtual chasiss on it.
 *======================================================================================================*/

public class UDPReceive : MonoBehaviour
{
    /*=======================================
    * The start of a space for an variables. 
    * ======================================*/
    public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();

    private GameObject myDisplay;
    private GameObject configMan;
    [Header("Variables")]
    private string message;

    private string port;
    private string externalIP;

    public float myX = 0;
    public float myY = 0;
    public float myZ = 0;
    public float myA = 0;
    public float myB = 0;
    public float myC = 0;

    private string[] sStrings;
    //private string showMsg = "Message";
    /*=====================================
     * The end of a space for an variables. 
     * ====================================*/

#if !UNITY_EDITOR
    DatagramSocket socket;
#endif

#if !UNITY_EDITOR
    void Start() //Method for establishing a  connection an remote server.
    {
        SetConfiguration();
        StartConnection();
    }

    async void StartConnection()
    {
        //MainController.Log("Waiting for a connection...");
        Debug.Log("Waiting for a connection...");

        socket = new DatagramSocket();
        socket.MessageReceived += Socket_MessageReceived;

        //HostName IP = null;
        try
        {       
            await socket.BindServiceNameAsync(port);
        }
        catch (Exception e)
        {
            //MainController.Log($"UDP ERROR: {e.ToString()}");
            //MainController.Log(SocketError.GetStatus(e.HResult).ToString());
            Debug.Log(e.ToString());
            Debug.Log(SocketError.GetStatus(e.HResult).ToString());
            return;
        }

        //MainController.Log("exit start");
    }

    public async System.Threading.Tasks.Task SendMessage(string message) //Method to send a UDP packets.
    {
        using (var stream = await socket.GetOutputStreamAsync(new Windows.Networking.HostName(externalIP), port))
        {
            using (var writer = new Windows.Storage.Streams.DataWriter(stream))
            {
                var data = Encoding.UTF8.GetBytes(message);

                writer.WriteBytes(data);
                await writer.StoreAsync();
                //MainController.Log("Sent: " + message);
            }
        }
    }
#else
    void Start()
    {
    }
#endif

    void Update()
    {
        myDisplay = GameObject.FindGameObjectWithTag("Display");

        while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();
        }

        //myDisplay.GetComponent<TextMeshPro>().text = showMsg; //To make received packets seenable.

        //try
        //{

        //}
        //catch (Exception ex)
        //{
        //    MainController.Log($"Error: {ex.Message}");
        //}
    }

    private void SetConfiguration() //Method to set configuration.
    {
        configMan = GameObject.FindGameObjectWithTag("Configuration");
        externalIP = configMan.GetComponent<ConfigManager>().externalIP;
        port = configMan.GetComponent<ConfigManager>().port;
    }

#if !UNITY_EDITOR
    private async void Socket_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender, Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args) //Method to receive a UDP packets.
    {
        try
        {
            //MainController.Log("GOT MESSAGE: ");
            //Read the message that was received from the UDP echo client.
            Stream streamIn = args.GetDataStream().AsStreamForRead();
            StreamReader reader = new StreamReader(streamIn);
            message = await reader.ReadLineAsync();

            sStrings = message.Split(";"[0]); //Translation of an received message to a better readable version in case that we want to see received packets.
            //showMsg = "X:" + float.Parse(sStrings[0]).ToString("0.00") + "Y:" + float.Parse(sStrings[1]).ToString("0.00") + "Z:" + float.Parse(sStrings[2]).ToString("0.00") + "\nA:" + float.Parse(sStrings[3]).ToString("0.00") + "B:" + float.Parse(sStrings[4]).ToString("0.00") + "C:" + float.Parse(sStrings[5]).ToString("0.00"); //A better readable version of a received packets.

            myX = float.Parse(sStrings[0]); //Creating floats for a use in a Transformace script.
            myY = float.Parse(sStrings[1]);
            myZ = float.Parse(sStrings[2]);
            myA = float.Parse(sStrings[3]);
            myB = float.Parse(sStrings[4]);
            myC = float.Parse(sStrings[5]);
          
            //MainController.Log("MESSAGE: " + showMsg);

            if (ExecuteOnMainThread.Count == 0)
            {
                ExecuteOnMainThread.Enqueue(() =>
                {
                    //myDisplay.GetComponent<TextMeshPro>().text = showMsg; //To make received packets seenable.
                });
            }
        }
        catch (Exception ex)
        {
            MainController.Log($"Error: {ex.Message}");
        }
    }
#endif
}
