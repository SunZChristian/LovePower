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
                response.Error = ErrorCore.ERR_RequestRepeatedly;
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
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await coroutineLockComponent.Wait(CoroutineLockType.CreateRole,request.Account.GetLongHashCode()))
                {
                    DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());
                    var roleInfo = await dbComponent.Query<RoleInfo>(d => d.Name == request.Name && d.ServerId == request.ServerId);
                    if (roleInfo != null && roleInfo.Count > 0)
                    {
                        response.Error = ErrorCore.ERR_RoleNameSame;
                        return;
                    }

                    RoleInfo newRoleInfo = session.AddChild<RoleInfo>();
                    newRoleInfo.Name = request.Name;
                    newRoleInfo.State = (int)RoleInfoState.Normal;
                    newRoleInfo.Account = request.Account;
                    newRoleInfo.ServerId = request.ServerId;
                    newRoleInfo.CreateTime = TimeInfo.Instance.ServerNow();
                    newRoleInfo.LastLoginTime = 0;

                    await dbComponent.Save<RoleInfo>(newRoleInfo);

                    response.RoleInfo = newRoleInfo.ToMessage();
                    response.Error = ErrorCore.ERR_SUCCESS;
                    newRoleInfo?.Dispose();
                }
            }
            
        }
    }
}