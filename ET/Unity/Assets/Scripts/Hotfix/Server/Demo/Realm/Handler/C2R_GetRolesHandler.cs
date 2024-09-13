namespace ET.Server
{
    []
    public class C2R_GetRolesHandler:MessageSessionHandler<C2R_GetRoles,R2C_GetRoles>
    {
        protected override async ETTask Run(Session session, C2R_GetRoles request, R2C_GetRoles response)
        {
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCore.ERR_RequestRepeatedly;
                session?.Disconnect().Coroutine();
                return;
            }

            string token = session.Root().GetComponent<TokenComponent>().Get(request, Account);

            if (token == null || token != request.Token)
            {
                response.Error = ErrorCore.ERR_TokenError;
                session?.Disconnect().Coroutine();
                return;
            }

            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await coroutineLockComponent.Wait(CoroutineLockType.CreateRole, request.Account.GetLongHashCode()))
                {
                    DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());
                    var roleInfos = await dbComponent.Query<RoleInfo>(
                            d=> d.Account == request.Account
                            && d.ServerId == request.ServerId
                            && d.State == (int)RoleInfoState.Normal
                        );

                    if (roleInfos == null || roleInfos.Count == 0)
                    {
                        return;
                    }

                    foreach (var roleInfo in roleInfos)
                    {
                        response.RoleInfo.Add(roleInfo.ToMessage());
                        roleInfo?.Dispose();
                    }
                    
                    roleInfos.Clear();
                }
            }
        }
    }
}

 