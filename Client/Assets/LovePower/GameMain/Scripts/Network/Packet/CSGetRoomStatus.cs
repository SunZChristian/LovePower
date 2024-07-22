using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    [Serializable, ProtoContract(Name = @"CSGetRoomStatus")]
    public class CSGetRoomStatus : CSPacketBase
    {
        public override int Id => PacketID.CSGetRoomStatus;

        public override void Clear()
        {
            
        }
    }

}
