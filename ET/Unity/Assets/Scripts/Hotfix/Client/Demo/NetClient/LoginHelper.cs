namespace ET.Client
{
    public static class LoginHelper
    {
        public static async ETTask Login(Scene root, string account, string password)
        {
            root.RemoveComponent<ClientSenderComponent>();
            
            ClientSenderComponent clientSenderComponent = root.AddComponent<ClientSenderComponent>();
            
            var response = await clientSenderComponent.LoginAsync(account, password);

            if (response.Error != ErrorCore.ERR_SUCCESS)
            {
                Log.Error($"登录出错啦，{response.Error}");
                return;
            }
            
            Log.Info("登录成功~");

            string Token = response.Token;
            //root.GetComponent<PlayerComponent>().MyId = playerId;
            
            //获取服务器列表
            C2R_GetServerInfos c2rGetServerInfos = C2R_GetServerInfos.Create();
            c2rGetServerInfos.Token = Token;
            c2rGetServerInfos.Account = account;
            R2C_GetServerInfos r2CGetServerInfos = await clientSenderComponent.Call(c2rGetServerInfos) as R2C_GetServerInfos;
            if (r2CGetServerInfos.Error != ErrorCore.ERR_SUCCESS)
            {
                Log.Error($"请求服务器列表失败！, {response.Error}");
                return;
            }

            ServerInfoProto serverInfoProto = r2CGetServerInfos.ServerInfosList[0];
            //获取区服角色列表
            
            
            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
} 