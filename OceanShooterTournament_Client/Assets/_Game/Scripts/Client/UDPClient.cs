using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPClient
{
    private UdpClient udpClient;
    private Thread receiveThread;
    private bool isRunning = true;

    public Action<string> ResponseFishGame;
    private Queue<string> messageQueue = new Queue<string>();


    public UDPClient(Action<string> responseFishGame)
    {
        // Khởi tạo UDP client
        udpClient = new UdpClient();
        udpClient.Connect("192.168.0.104", 8080); // Kết nối đến server

        // Khởi chạy thread để nhận dữ liệu liên tục
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        ResponseFishGame = responseFishGame;
    }

    public void SendMessage(string mess)
    {
        try
        {
            // Tạo thông điệp cần gửi (request bắn đạn khi nhấn Escape)
            byte[] sendBytes = Encoding.ASCII.GetBytes(mess);

            // Gửi thông điệp tới server qua UDP
            udpClient.Send(sendBytes, sendBytes.Length);
            Debug.Log("Sent escape request to server");
        }
        catch (Exception e)
        {
            Debug.Log("Error sending escape request: " + e.Message);
        }
    }


    private void ReceiveData()
    {
        while (isRunning)
        {
            try
            {
                // Lắng nghe dữ liệu từ server
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 8080);
                byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
                string receivedData = Encoding.ASCII.GetString(receiveBytes);
                //Debug.Log("Received: " + receivedData);

                // Xử lý dữ liệu nhận được, ví dụ cập nhật vị trí viên đạn
                lock (messageQueue)
                {
                    messageQueue.Enqueue(receivedData);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error receiving data: " + e.Message);
            }
        }
    }

    public void ProcessMessages()
    {
        // Xử lý tất cả các tin nhắn trong hàng đợi
        lock (messageQueue)
        {
            while (messageQueue.Count > 0)
            {
                string message = messageQueue.Dequeue();
                ResponseFishGame.Invoke(message);
            }
        }
    }

    public void OnApplicationQuit()
    {
        // Dừng thread khi thoát game
        isRunning = false;
        if (receiveThread != null)
        {
            receiveThread.Abort();
        }

        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
