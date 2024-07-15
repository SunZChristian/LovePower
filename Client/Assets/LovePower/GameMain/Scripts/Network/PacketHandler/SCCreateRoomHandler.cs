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
                return 1001;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            GameEntry.Event.FireNow(UILoadingStateEventArgs.EventId, UILoadingStateEventArgs.Create(false));

            SCCreateRoom packetImpl = (SCCreateRoom)packet;
            //Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());
            if (packetImpl.Code == 200)
            {
                //创建成功
                GameEntry.UI.ShowAlert("创建房间成功~");
            }
            else
            {
                //已经有人创建房间啦
                GameEntry.UI.ShowAlert("你的宝儿已经把房间建好啦~");
            }
        }
    }
}