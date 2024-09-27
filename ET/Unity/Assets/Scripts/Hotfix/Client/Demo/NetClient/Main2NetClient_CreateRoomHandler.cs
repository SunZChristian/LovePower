namespace ET.Client
{
    [MessageHandler(SceneType.NetClient)]
    public class Main2NetClient_CreateRoomHandler:MessageHandler<Scene,Main2NetClient_CreateRoom,NetClient2Main_CreateRoom>
    {
        protected override async ETTask Run(Scene scene, Main2NetClient_CreateRoom request, NetClient2Main_CreateRoom response)
        {
                        
            C2G_CreateRoom c2GCreateRoom = C2G_CreateRoom.Create();
            G2C_CreateRoom g2CCreateRoom = await scene.GetComponent<ClientSenderComponent>().Call(c2GCreateRoom) as G2C_CreateRoom;
            if (g2CCreateRoom.Error != ErrorCore.ERR_SUCCESS)
            {
                Log.Error("创建房间失败 ："+g2CCreateRoom.Error);
                //response.Error = ErrorCore
                return;
            }

            response.Error = ErrorCore.ERR_SUCCESS;
            response.RoomId = g2CCreateRoom.RoomId;
        }
    }
}

