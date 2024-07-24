using ProtoBuf;
using System;

[Serializable, ProtoContract(Name = @"SCCreateRoom")]
public class SCCreateRoom : SCPacketBase
{
    //[ProtoMember(1)]
    //public int Code;

    //[ProtoMember(2)]
    //public string Message;

    public SCCreateRoom()
    {
    }

    public override int Id
    {
        get
        {
            return PacketID.SCCreateRoom;
        }
    }

}