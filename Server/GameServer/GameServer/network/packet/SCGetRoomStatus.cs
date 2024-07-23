using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable, ProtoContract(Name = @"SCGetRoomStatus")]
public class SCGetRoomStatus : SCPacketBase
{
    [ProtoMember(1)]
    public int Code;

    [ProtoMember(2)]
    public string Message;

    [ProtoMember(3)]
    public int OperationCode;

    [ProtoMember(4)]
    public int VideoProgress;

    public override int Id => PacketID.SCGetRoomStatus;

}
