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
                Log.Error($"请求服务器列表失败！, {r2CGetServerInfos.Error}");
                return;
            }

            ServerInfoProto serverInfoProto = r2CGetServerInfos.ServerInfosList[0];
            //获取区服角色列表
            C2R_GetRoles c2RGetRoles = C2R_GetRoles.Create();
            c2RGetRoles.Token = Token;
            c2RGetRoles.Account = account;
            c2RGetRoles.ServerId = serverInfoProto.Id;
            R2C_GetRoles r2CGetRoles = await clientSenderComponent.Call(c2RGetRoles) as R2C_GetRoles;
            if (r2CGetRoles.Error != ErrorCore.ERR_SUCCESS)
            {
                Log.Error($"请求区服角色列表失败！, {r2CGetRoles.Error}");
                return;
            }

            RoleInfoProto roleInfoProto = default;
            if (r2CGetRoles.RoleInfo.Count <= 0)
            {
                //没有角色信息，先创建角色
                C2R_CreateRole c2RCreateRole = C2R_CreateRole.Create();
                c2RCreateRole.Token = Token;
                c2RCreateRole.Account = account;
                c2RCreateRole.ServerId = serverInfoProto.Id;
                c2RCreateRole.Name = account;
                
                R2C_CreateRole r2CCreateRole = await clientSenderComponent.Call(c2RCreateRole) as R2C_CreateRole;
                if (r2CCreateRole.Error != ErrorCore.ERR_SUCCESS)
                {
                    Log.Error($"创建角色失败！, {r2CCreateRole.Error}");
                    return;
                }

                roleInfoProto = r2CCreateRole.RoleInfo;
            }
            else
            {
                //存在角色信息，取第一个
                roleInfoProto = r2CGetRoles.RoleInfo[0];
            }
            
            //请求获取RealmKey
            C2R_GetRealmKey c2RGetRealmKey = C2R_GetRealmKey.Create();
            c2RGetRealmKey.Token = Token;
            c2RGetRealmKey.Account = account;
            c2RGetRealmKey.ServerId = serverInfoProto.Id;
            R2C_GetRealmKey r2CGetRealmKey = await clientSenderComponent.Call(c2RGetRealmKey) as R2C_GetRealmKey;

            if (r2CGetRealmKey.Error != ErrorCore.ERR_SUCCESS)
            {
                Log.Error("获取RealmKey失败！");
                return;
            }
            
            //请求游戏角色进度map地图
            NetClient2Main_LoginGame netClient2MainLoginGame = await clientSenderComponent.LoginGameAsync(account,r2CGetRealmKey.Key,roleInfoProto.Id,r2CGetRealmKey.Address) as NetClient2Main_LoginGame;
            if (netClient2MainLoginGame.Error != ErrorCore.ERR_SUCCESS)
            {
                Log.Error($"进入游戏失败：{netClient2MainLoginGame.Error}");
                return;
            }
            
            Log.Debug("进入游戏成功！！！");

            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
} 