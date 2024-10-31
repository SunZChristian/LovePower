namespace ET.Client
{
    public static class EnterRoomHelper
    {
        public static async ETTask CreateRoom(Scene root)
        {
            await EnterMapHelper.EnterMapAsync(root);
            
            Log.Info("进入地图完成");
            //进入地图完成
            
            Main2NetClient_CreateRoom main2NetClientCreateRoom = Main2NetClient_CreateRoom.Create();
            main2NetClientCreateRoom.OwnerFiberId = root.Fiber.Id;
            
            NetClient2Main_CreateRoom netClient2MainCreateRoom = await root.GetComponent<ClientSenderComponent>().Call(main2NetClientCreateRoom) as NetClient2Main_CreateRoom;
            if (netClient2MainCreateRoom.Error != ErrorCore.ERR_SUCCESS)
            {   
                //创建房间失败
                Log.Error("创建房间失败 ："+netClient2MainCreateRoom.Error);
                return;
            }
            
            await EventSystem.Instance.PublishAsync(root, new CreateRoomFinish());
        }
    }
}

