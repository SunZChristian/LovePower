using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    [Serializable, ProtoContract(Name = @"CSSyncRoomStatus")]
    public class CSSyncRoomStatus : CSPacketBase
    {
        [ProtoMember(1)]
        public int OperationCode;

        [ProtoMember(2)]
        public int VideoProgress;

        public override int Id => PacketID.CSSyncRoomStatus;

        public override void Clear()
        {
            
        }
    }
}


