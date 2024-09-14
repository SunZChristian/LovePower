namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    [FriendOfAttribute(typeof(ServerInfoManagerComponent))]
    public class C2R_GetServerInfoHandler:MessageSessionHandler<C2R_GetServerInfos,R2C_GetServerInfos>
    {
        protected override async ETTask Run(Session session, C2R_GetServerInfos request, R2C_GetServerInfos response)
        {
            string Token = session.Root().GetComponent<TokenComponent>().Get(request.Account);

            if (Token == null || Token != request.Token)
            {
                response.Error = ErrorCore.ERR_TokenError;
                session?.Disconnect().Coroutine();
                return;
            }

            foreach (var serverInfoRef in session.Root().GetComponent<ServerInfoManagerComponent>().ServerInfos)
            {
                ServerInfo serverInfo = serverInfoRef;
                response.ServerInfosList.Add(serverInfo.ToMeqqqqqqqqssage());
            }

            await ETTask.CompletedTask;
        }
    }
}

