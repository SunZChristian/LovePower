using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class SCOtherPlayerEnterRoom : SCPacketBase
    {
        public override int Id => PacketID.SCOtherPlayerEnterRoom;

        public override void Clear()
        {
            
        }
    }
}

