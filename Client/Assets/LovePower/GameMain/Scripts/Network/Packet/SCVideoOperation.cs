using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    [Serializable, ProtoContract(Name = @"SCVideoOperation")]
    public class SCVideoOperation : SCPacketBase
    {
        [ProtoMember(1)]
        public int Code;

        [ProtoMember(2)]
        public string Message;

        [ProtoMember(3)]
        public int OperationCode;

        [ProtoMember(4)]
        public int VideoProgressValue;

        [ProtoMember(5)]
        public bool IsForce;

        public override int Id => 1004;

        public override void Clear()
        {
            
        }
    }
}

