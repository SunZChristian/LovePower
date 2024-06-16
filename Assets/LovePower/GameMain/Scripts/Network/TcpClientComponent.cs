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


        protected override void Awake()
        {
            base.Awake();

        }

        private void OnDestroy()
        {

        }

        public void InitNetwork()
        {
            // 创建频道
            m_NetworkChannelHelper = new NetworkChannelHelper();
            m_Channel = GameEntry.Network.CreateNetworkChannel("LovePower", GameFramework.Network.ServiceType.Tcp, m_NetworkChannelHelper);

            // 连接服务器
            m_Channel.Connect(IPAddress.Parse("127.0.0.1"), 1234);

            Log.Info("开始连接服务器");
        }
    }

}

