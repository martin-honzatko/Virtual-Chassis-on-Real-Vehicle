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

/*=================================================================================================================
 * This script is for creating of UDP connection to the MSSQL server and its management.
 * In a version 2 of the application was added ability to change positioning iformation to zeros by voice commands.
 * In a version 2.1 of the application was added ability to change configuration from config .json file. 
 * Repaired in a version 2.2 
 *=================================================================================================================*/

public class UDPCommunication : MonoBehaviour
{
    /*=======================================
     * The start of a space for an variables. 
     * ======================================*/
    public readonly static Queue<Action> ExecuteOnMainThread = new Queue<Action>();
    private GameObject myDisplay;
    private GameObject configMan;

    private string port;
    private string externalIP;

    private string message;
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
            //MainController.Log($"Error: {ex.Message}");
        }
    }
#endif
}
