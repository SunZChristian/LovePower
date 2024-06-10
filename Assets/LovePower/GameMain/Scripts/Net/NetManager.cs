using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace LovePower
{
    public class NetManager : MonoBehaviour
    {
        private TcpClient client;
        private NetworkStream stream;
        private const string userName = "¹ËÐ¡¶÷";
        private const string loverName = "ËïÐ¡ÕÜ";
        private const string roomName = "";

        public static NetManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void ConnectToServer()
        {
            if (client == null)
            {
                client = new TcpClient("127.0.0.1", 5000);
                stream = client.GetStream();
                Debug.Log("Connected to server.");
            }
        }

        public void CreateRoom()
        {
            ConnectToServer();
            SendMessageToServer($"create|{roomName}|{userName}");
            StartReceivingMessages();
        }

        public void JoinRoom()
        {
            ConnectToServer();
            SendMessageToServer($"join|{roomName}|{loverName}");
            StartReceivingMessages();
        }

        //void SendChatMessage()
        //{
        //    string message = messageInput.text;
        //    SendMessageToServer($"message|{message}");
        //}

        void DisbandRoom()
        {
            SendMessageToServer("disband");
        }

        void SendMessageToServer(string message)
        {
            if (stream != null)
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Debug.Log("Sent: " + message);
            }
        }

        void StartReceivingMessages()
        {
            byte[] buffer = new byte[1024];
            stream.BeginRead(buffer, 0, buffer.Length, (IAsyncResult ar) =>
            {
                int bytesRead = stream.EndRead(ar);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                //chatOutput.text += response + "\n";
                StartReceivingMessages();
            }, null);
        }

        void OnApplicationQuit()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
