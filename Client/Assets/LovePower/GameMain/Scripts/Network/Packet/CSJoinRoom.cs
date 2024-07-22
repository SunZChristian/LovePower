using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    [Serializable, ProtoContract(Name = @"CSJoinRoom")]
    public class CSJoinRoom : CSPacketBase
    {
        public override int Id => PacketID.CSJoinRoom;

        public override void Clear()
        {
            
        }
    }
}

