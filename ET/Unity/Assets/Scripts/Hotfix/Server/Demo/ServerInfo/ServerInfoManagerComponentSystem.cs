namespace ET.Server
{
    [EntitySystemOf(typeof(ServerInfoManagerComponent))]
    [FriendOf(typeof(ServerInfoManagerComponent))]
    [FriendOf(typeof(ServerInfo))]
    public static partial class ServerInfoManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ServerInfoManagerComponent self)
        {
            self.Load();
        }

        [EntitySystem]
        private static void Destroy(this ServerInfoManagerComponent self)
        {
            foreach (var serverInfoRef in self.ServerInfos)
            {
                ServerInfo serverInfo = serverInfoRef;
                serverInfo.Dispose();
            }
            
            self.ServerInfos.Clear();
        }

        public static void Load(this ServerInfoManagerComponent self)
        {
            foreach (var serverInfoRef in self.ServerInfos)
            {
                ServerInfo serverInfo = serverInfoRef;
                serverInfo?.Dispose();
            }
            
            self.ServerInfos.Clear();

            var serverInfoConfig = StartZoneConfigCategory.Instance.GetAll();
            foreach (var info in serverInfoConfig.Values)
            {
                if (info.ZoneType != 1)
                { 
                    continue;
                }

                ServerInfo newServerInfo = self.AddChildWithId<ServerInfo>(info.Id);
                newServerInfo.ServerName = info.DBName;
                newServerInfo.Status = (int)ServerStatus.Normal;
                self.ServerInfos.Add((newServerInfo));
            }
        }
    }
}

