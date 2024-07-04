using ProtoBuf;
using System;

[Serializable, ProtoContract(Name = @"SCCreateRoom")]
public class SCCreateRoom : SCPacketBase
{
    [ProtoMember(1)]
    public bool IsCreateSuccess;

    public SCCreateRoom()
    {
    }

    public override int Id
    {
        get
        {
            return 1001;
        }
    }

}