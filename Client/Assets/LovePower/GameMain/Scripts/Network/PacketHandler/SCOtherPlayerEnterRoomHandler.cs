using GameFramework.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class SCOtherPlayerEnterRoomHandler : PacketHandlerBase
    {
        public override int Id => PacketID.SCOtherPlayerEnterRoom;

        public override void Handle(object sender, Packet packet)
        {
            GameEntry.UI.ShowAlert("��ӭ�������뷿��~");
        }
    }
}
