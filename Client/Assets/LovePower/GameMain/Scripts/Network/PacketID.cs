using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class PacketID
    {
        public const int CSHeartBeat = 1;
        public const int SCHeartBeat = 2;

        public const int CSCreateRoom = 1001;
        public const int SCCreateRoom = 1002;

        public const int CSJoinRoom = 1003;
        public const int SCJoinRoom = 1004;

        public const int CSVideoOperation = 1005;
        public const int SCVideoOperation = 1006;

        public const int SCOtherPlayerEnterRoom = 1008;

        public const int CSGetRoomStatus = 1009;
        public const int SCGetRoomStatus = 1010;
    }
}
