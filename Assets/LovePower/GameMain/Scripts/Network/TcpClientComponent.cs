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
            // ����Ƶ��
            m_NetworkChannelHelper = new NetworkChannelHelper();
            m_Channel = GameEntry.Network.CreateNetworkChannel("LovePower", GameFramework.Network.ServiceType.Tcp, m_NetworkChannelHelper);

            // ���ӷ�����
            m_Channel.Connect(IPAddress.Parse("127.0.0.1"), 1234);

            Log.Info("��ʼ���ӷ�����");
        }
    }

}

