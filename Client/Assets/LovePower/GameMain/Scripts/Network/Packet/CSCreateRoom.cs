//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using ProtoBuf;
using System;

namespace LovePower
{
    [Serializable, ProtoContract(Name = @"CSCreateRoom")]
    public class CSCreateRoom : CSPacketBase
    {
        public CSCreateRoom()
        {
        }

        public override int Id
        {
            get
            {
                return PacketID.CSCreateRoom;
            }
        }

        public override void Clear()
        {

        }
    }
}
