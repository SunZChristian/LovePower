namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    [FriendOf(typeof(RoleInfo))]
    public class C2R_CreateRoleHandler:MessageSessionHandler<C2R_CreateRole,R2C_CreateRole>
    {
        protected override async ETTask Run(Session session, C2R_CreateRole request, R2C_CreateRole response)
        {
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCore. ERR_RequestRepeatedly
                session?.Disconnect().Coroutine();
                return;
            }

            string Token = session.Root().GetComponent<TokenComponent>().Get(request.Account);
            if (Token == null || Token != request.Token)
            {
                response.Error = ErrorCore.ERR_TokenError;
                session?.Disconnect().Coroutine();
                return;
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                response.Error = ErrorCore.ERR_RoleNameIsNull;
                return;
            }

            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            
        }
    }
}

