//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using ProtoBuf;
using System;


//[Serializable, ProtoContract(Name = @"SCHeartBeat")]
public class SCHeartBeat : SCPacketBase
{

    //[ProtoMember(1)]
    //public int Code;

    public SCHeartBeat()
    {
    }

    public override int Id
    {
        get
        {
            return PacketID.SCHeartBeat;
        }
    }
}
