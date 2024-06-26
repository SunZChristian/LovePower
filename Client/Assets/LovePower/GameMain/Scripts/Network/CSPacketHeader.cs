﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
using ProtoBuf;

namespace LovePower
{
    [ProtoContract]
    public sealed class CSPacketHeader : PacketHeaderBase
    {
        [ProtoMember(1)]
        public override int Id { get; set; }

        [ProtoMember(2)]
        public override int PacketLength { get; set; }

        public override PacketType PacketType
        {
            get
            {
                return PacketType.ClientToServer;
            }
        }
    }
}
