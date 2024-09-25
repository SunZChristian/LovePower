namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    [FriendOf(typeof(RoleInfo))]
    public class C2R_DeleteRoleHandler : MessageSessionHandler<C2R_DeleteRole,R2C_DeleteRole>
    {
        protected override async ETTask Run(Session session, C2R_DeleteRole request, R2C_DeleteRole response)
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
            
            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await coroutineLockComponent.Wait(CoroutineLockType.CreateRole,request.Account.GetLongHashCode()))
                {
                    DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());
                    var roleInfos = await dbComponent.Query<RoleInfo>(d => d.Id == request.RoleInfoId && d.ServerId == request.ServerId);
                    if (roleInfos == null || roleInfos.Count < 0)
                    {
                        response.Error = ErrorCore.ERR_RoleNotExit;
                        return;
                    }

                    var roleInfo = roleInfos[0];
                    session.AddChild(roleInfo);

                    roleInfo.State = (int)RoleInfoState.Freeze;
                    await dbComponent.Save<RoleInfo>(roleInfo);
                    response.DeleteRoleInfoId = roleInfo.Id;
                    roleInfo?.Dispose();
                }
            
            }
        }
    }
}


