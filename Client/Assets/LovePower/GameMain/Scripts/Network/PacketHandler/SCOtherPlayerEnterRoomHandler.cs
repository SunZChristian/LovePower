using GameFramework.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class SCOtherPlayerEnterRoomHandler : PacketHandlerBase
    {
        public override int Id => 1003;

        public override void Handle(object sender, Packet packet)
        {
            GameEntry.UI.ShowAlert("欢迎宝宝进入房间~");
        }
    }
}

