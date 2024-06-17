using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace LovePower
{
    public class SCCreateRoomHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return 2;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            SCCreateRoom packetImpl = (SCCreateRoom)packet;
            //Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());
            if (packetImpl.IsCreateSuccess)
            {
                //创建成功
            }
            else
            {
                //已经有人创建房间啦
            }
        }
    }
}