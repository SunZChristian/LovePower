using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public enum EVideoOperation
    { 
        Play = 1,
        Pause,
        ChangeProgress
    }

    [Serializable, ProtoContract(Name = @"CSVideoOperation")]
    public class CSVideoOperation : CSPacketBase
    {
        [ProtoMember(1)]
        public int OperationCode;

        [ProtoMember(2)]
        public int VideoProgressValue;

        public override int Id => 1004;

        public override void Clear()
        {
            
        }
    }
}