using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable, ProtoContract(Name = @"CSGetRoomStatus")]
public class CSGetRoomStatus : CSPacketBase
{
    public override int Id => 1005;

}