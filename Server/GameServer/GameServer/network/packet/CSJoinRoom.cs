using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable, ProtoContract(Name = @"CSJoinRoom")]
public class CSJoinRoom : CSPacketBase
{
    public override int Id => PacketID.CSJoinRoom;

}

