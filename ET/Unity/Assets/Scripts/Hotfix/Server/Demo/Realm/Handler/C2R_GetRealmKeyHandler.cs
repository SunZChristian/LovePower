namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    public class C2R_GetRealmKeyHandler:MessageSessionHandler<C2R_GetRealmKey,R2C_GetRealmKey>
    {
        protected override async ETTask Run(Session session, C2R_GetRealmKey request, R2C_GetRealmKey response)
        {
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCore.ERR_RequestRepeatedly;
                session.Disconnect().Coroutine();
                return;
            }

            string token = session.Root().GetComponent<TokenComponent>().Get(request.Account);
            if (token == null || token != request.Token)
            {
                response.Error = ErrorCore.ERR_TokenError;
                session.Disconnect().Coroutine();
                return;
            }

            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await coroutineLockComponent.Wait(CoroutineLockType.LoginAccount,request.Account.GetLongHashCode()))
                {
                    //随机分配一个Gate
                    StartSceneConfig startSceneConfig = RealmGateAddressHelper.GetGate(request.ServerId, request.Account);
                    Log.Debug("gate address: "+startSceneConfig);
                    
                     //向Gate请求一个Key,客户端可以拿着这Key链接Gate
                     R2G_GetLoginKey r2GGetLoginKey = R2G_GetLoginKey.Create();
                     r2GGetLoginKey.Account = request.Account;
                     G2R_GetLoginKey g2RGetLoginKey = await session.Fiber().Root.GetComponent<MessageSender>().Call(startSceneConfig.ActorId,r2GGetLoginKey) as G2R_GetLoginKey;

                     response.Address = startSceneConfig.InnerIPPort.ToString();
                     response.Key = g2RGetLoginKey.Key;
                     response.GateId = g2RGetLoginKey.GateId;
                     
                     session.Disconnect().Coroutine();
                }
            }
        }
    }
}

