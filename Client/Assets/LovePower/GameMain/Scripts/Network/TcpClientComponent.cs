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

        public void CreateRoom()
        {
            GameEntry.Event.FireNow(UILoadingStateEventArgs.EventId, UILoadingStateEventArgs.Create(true));
            m_Channel.Send(ReferencePool.Acquire<CSCreateRoom>());
        }

        public void JoinRoom()
        {
            GameEntry.Event.FireNow(UILoadingStateEventArgs.EventId, UILoadingStateEventArgs.Create(true));
            m_Channel.Send(ReferencePool.Acquire<CSJoinRoom>());
        }

        public void PlayVideo()
        {
            var msg = ReferencePool.Acquire<CSVideoOperation>();
            msg.OperationCode = (int)EVideoOperation.Play;
            m_Channel.Send<CSVideoOperation>(msg);
        }

        public void PauseVideo()
        {
            var msg = ReferencePool.Acquire<CSVideoOperation>();
            msg.OperationCode = (int)EVideoOperation.Pause;
            m_Channel.Send<CSVideoOperation>(msg);
        }

        public void GetRoomStatus()
        {
            var msg = ReferencePool.Acquire<CSGetRoomStatus>();
            m_Channel.Send<CSGetRoomStatus>(msg);
        }

        public void SyncRoomStatus(int operationCode,int progress)
        {
            var msg = ReferencePool.Acquire<CSSyncRoomStatus>();
            msg.OperationCode = operationCode;
            msg.VideoProgress = progress;
            m_Channel.Send<CSSyncRoomStatus>(msg);
        }

        public void ChangeVideoProgress(float value)
        {
            var msg = ReferencePool.Acquire<CSVideoOperation>();
            msg.OperationCode = (int)EVideoOperation.ChangeProgress;
            msg.VideoProgressValue = (int)(value * 100);
            m_Channel.Send<CSVideoOperation>(msg);
        }

        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            NetworkConnectedEventArgs args = (NetworkConnectedEventArgs)e;
            if (args!= null&& args.NetworkChannel.Connected)
            {
                IsNetworkConnected = true;
                Log.Info("连接服务器成功~");

                //CreateRoom();
            }
        }
    }

}

