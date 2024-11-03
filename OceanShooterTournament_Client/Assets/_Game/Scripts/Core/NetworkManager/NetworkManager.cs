using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetworkManager : MonoSingletonDontDestroyOnLoad<NetworkManager>
{
    [SerializeField] private string serverHost;
    [SerializeField] private int port;
    private UdpClient udpClient;
    private Thread receiveThread;
    private bool isRunning = true;
    private Queue<string> messageQueue = new();

    public override void Init()
    {
        udpClient = new UdpClient();
        udpClient.Connect(serverHost, port);
        receiveThread = new Thread(ClientListenServer)
                        {
                            IsBackground = true
                        };
        receiveThread.Start();
    }

    private void Update()
    {
        ProcessMessages();
    }

    private void ClientListenServer()
    {
        while (isRunning)
        {
            var remoteEndPoint = new IPEndPoint(IPAddress.Any, 8080);
            var receiveBytes = udpClient.Receive(ref remoteEndPoint);
            var receivedData = Encoding.ASCII.GetString(receiveBytes);
            lock (messageQueue)
            {
                messageQueue.Enqueue(receivedData);
            }
        }
    }

    public void ProcessMessages()
    {
        lock (messageQueue)
        {
            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                Receive(message);
            }
        }
    }

    public void Request(string action, string data)
    {
        var message = $"{AccountManager.instance.CreateToken(action)};{data}";
        try
        {
            var sendBytes = Encoding.ASCII.GetBytes(message);
            udpClient.Send(sendBytes, sendBytes.Length);
            Debug.Log("Sent message: " + message);
        }
        catch (Exception e)
        {
            Debug.Log("Error sending escape request: " + e.Message);
        }
    }

    private static void Receive(string receivedData)
    {
        Debug.Log("Received data: " + receivedData);
        var data = receivedData.ToObject<DataAccessObject>();
        Debug.Log("Received data token: " + data.GetToken());
        ObserverManager.instance.SendMessageNotification(
            data.GetToken(),
            receivedData
        );
    }

    public void OnApplicationQuit()
    {
        isRunning = false;
        receiveThread?.Abort();
        udpClient?.Close();
    }
}