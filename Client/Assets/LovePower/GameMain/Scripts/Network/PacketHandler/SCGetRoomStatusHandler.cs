using System.Collections;
using System.Collections.Generic;
using GameFramework.Network;
using UnityEngine;

namespace LovePower
{
    public class SCGetRoomStatusHandler : PacketHandlerBase
    {
        public override int Id => PacketID.SCGetRoomStatus;

        public override void Handle(object sender, Packet packet)
        {
            SCGetRoomStatus msg = (SCGetRoomStatus)packet;
            if (msg.Code == 200)
            {
                if (msg.OperationCode == (int)EVideoOperation.Play)
                {
                    GameEntry.Video.Play();
                }
                else
                {
                    GameEntry.Video.Pause();
                }

                var value = msg.VideoProgress * 1f / 10000f;
                GameEntry.Video.SetPlayProgress(value);
            }
        }
    }
}

