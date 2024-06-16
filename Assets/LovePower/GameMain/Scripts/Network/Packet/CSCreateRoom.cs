using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                return 1001;
            }
        }

        public override void Clear()
        {
        }
    }
}
