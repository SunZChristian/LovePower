using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    [Serializable, ProtoContract(Name = @"SCJoinRoom")]
    public class SCJoinRoom : SCPacketBase
    {
        [ProtoMember(1)]
        public int Code;

        [ProtoMember(2)]
        public string Message;

        public override int Id => PacketID.SCJoinRoom;

        public override void Clear()
        {
            
        }
    }
}

