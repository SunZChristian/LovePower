using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace LovePower
{


    public class TcpClientComponent : GameFrameworkComponent
    {
        private GameFramework.Network.INetworkChannel m_Channel;
        private NetworkChannelHelper m_NetworkChannelHelper;

        public bool IsNetworkConnected = false;
        private float m_heatbeatTimer;
        private float m_heatbeatInterval = 2f;

        protected override void Awake()
        {
            base.Awake();

        }

        private void Start()
        {

            IsNetworkConnected = false;
            m_heatbeatTimer = m_heatbeatInterval;

            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
        }

        private void OnDestroy()
        {
            GameEntry.Event.Unsubscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
        }

        public void InitNetwork()
        {
            // 创建频道
            m_NetworkChannelHelper = new NetworkChannelHelper();
           
            m_Channel = GameEntry.Network.CreateNetworkChannel("LovePower", GameFramework.Network.ServiceType.Tcp, m_NetworkChannelHelper);
            //m_NetworkChannelHelper.Initialize(m_Channel);

            // 连接服务器
            m_Channel.Connect(IPAddress.Parse("127.0.0.1"), 1234);

            Log.Info("开始连接服务器");
        }

        private void Update()
        {
            if (IsNetworkConnected)
            {
                m_heatbeatTimer -= Time.deltaTime;
                if (m_heatbeatTimer <= 0)
                {
                    m_NetworkChannelHelper.SendHeartBeat();

                    m_heatbeatTimer = m_heatbeatInterval;

                    Log.Info("发送一次心跳");
                }
            }
        }

        //public void HeatBeat()
        //{
        //    m_Channel.Send<CSHeartBeat>(new CSHeartBeat());
        //}

        public void CreateRoom()
        {
            m_Channel.Send<CSCreateRoom>(ReferencePool.Acquire<CSCreateRoom>());
        }

        public void JoinRoom()
        {

        }

        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            NetworkConnectedEventArgs args = (NetworkConnectedEventArgs)e;
            if (args!= null&& args.NetworkChannel.Connected)
            {
                IsNetworkConnected = true;
                Log.Info("连接服务器成功~");
            }
        }
    }

}

