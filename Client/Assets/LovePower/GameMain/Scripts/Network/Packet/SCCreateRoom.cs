using ProtoBuf;
using System;

namespace LovePower
{
    [Serializable, ProtoContract(Name = @"SCCreateRoom")]
    public class SCCreateRoom : SCPacketBase
    {
        [ProtoMember(1)]
        public int Code;

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

        public override void Clear()
        {
        }
    }
}
