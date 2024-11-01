using ProtoBuf;
using System;

[Serializable, ProtoContract(Name = @"CSSyncRoomStatus")]
public class CSSyncRoomStatus : CSPacketBase
{
    [ProtoMember(1)]
    public int OperationCode;

    [ProtoMember(2)]
    public int VideoProgress;

    public CSSyncRoomStatus()
    {
    }

    public override int Id
    {
        get
        {
            return PacketID.CSSyncRoomStatus;
        }
    }

}