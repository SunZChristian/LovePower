using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    [Serializable, ProtoContract(Name = @"CSVideoOperation")]
    public class CSVideoOperation : CSPacketBase
    {
        [ProtoMember(1)]
        public int OperationCode;

        public override int Id => 1004;

        public override void Clear()
        {
            
        }
    }
}