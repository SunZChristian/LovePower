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
                return PacketID.SCCreateRoom;
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
                //发消息
                GameEntry.Event.FireNow(CreateRoomSuccessArgs.EventId, CreateRoomSuccessArgs.Create());
            }
            else if (packetImpl.Code == 101)
            {
                //已经有人创建房间啦
                GameEntry.UI.ShowAlert("你的宝儿已经把房间建好啦~");
            }
            else
            {
                GameEntry.UI.ShowAlert("宝宝，你已经在房间中辣~");
            }
        }
    }
}