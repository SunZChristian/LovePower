using ProtoBuf;
using System;

namespace LovePower
{
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

        public override void Clear()
        {
        }
    }
}
