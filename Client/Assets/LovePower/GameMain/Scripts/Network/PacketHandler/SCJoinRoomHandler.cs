using GameFramework.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class SCJoinRoomHandler : PacketHandlerBase
    {
        public override int Id => PacketID.SCJoinRoom;

        public override void Handle(object sender, Packet packet)
        {
            GameEntry.Event.FireNow(UILoadingStateEventArgs.EventId, UILoadingStateEventArgs.Create(false));

            SCJoinRoom packetImpl = (SCJoinRoom)packet;
            if (packetImpl.Code == 200)
            {
                //���뷿��ɹ�
                GameEntry.UI.ShowAlert("�ҽ���������С��");
                GameEntry.Event.FireNow(JoinRoomSuccessArgs.EventId, JoinRoomSuccessArgs.Create());
            }
            else if (packetImpl.Code == 1001)
            {
                //���仹δ����
                GameEntry.UI.ShowAlert("������~ ������ôû����");
            }
            else if (packetImpl.Code == 1002)
            {
                //������������
                GameEntry.UI.ShowAlert("���ĸ���ģ�����");
            }
        }
    }
}

