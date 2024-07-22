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
                //进入房间成功
                GameEntry.UI.ShowAlert("右脚迈进爱的小窝");
                GameEntry.Event.FireNow(JoinRoomSuccessArgs.EventId, JoinRoomSuccessArgs.Create());
            }
            else if (packetImpl.Code == 1001)
            {
                //房间还未创建
                GameEntry.UI.ShowAlert("哐哐哐~ 屋里怎么没人呢");
            }
            else if (packetImpl.Code == 1002)
            {
                //房间人数已满
                GameEntry.UI.ShowAlert("是哪个男模在屋里！");
            }
        }
    }
}

