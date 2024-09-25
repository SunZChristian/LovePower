using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    [FriendOfAttribute(typeof(Account))]
    public class C2R_LoginAccountHandler:MessageSessionHandler<C2R_LoginAccount,R2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2R_LoginAccount request, R2C_LoginAccount response)
        {
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (session.GetComponent<SessionLockingComponent>()!=null)
            {
                response.Error = ErrorCore.ERR_RequestRepeatedly;
                session.Disconnect().Coroutine();//等待一秒后断开，立即断开的话，客户端收不到错误返回
                return;
            }

            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCore.ERR_LoginInfoIsNull;
                session.Disconnect().Coroutine();//等待一秒后断开，立即断开的话，客户端收不到错误返回
                return;
            }
            
            //TODO 账号密码合法性校验，这里就不做了

            var coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await coroutineLockComponent.Wait(CoroutineLockType.LoginAccount,request.Account.GetLongHashCode()))
                {
                    //数据库操作，账号是否存在，不存在则创建账号
                    DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());

                    List<Account> accountInfoList = await dbComponent.Query<Account>(d => d.AccountName.Equals(request.Account));
                    Account account = null;
                    
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);
                        
                        if (!account.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCore.ERR_LoginPasswordError;
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.Account.Trim();
                        account.Password    = request.Password;
                        account.CreateTime  = TimeInfo.Instance.ServerNow();
                        account.AccountType = (int)AccountType.General;
                        await dbComponent.Save<Account>(account);
                    }
                    
                    //Realm与LoginCenter之间通信
                    R2L_LoginAccountRequest r2LLoginAccountRequest = R2L_LoginAccountRequest.Create();
                    r2LLoginAccountRequest.Account = request.Account;

                    StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.LoginCenterConfig;
                    var loginAccountResponse = await session.Fiber().Root.GetComponent<MessageSender>().Call(loginCenterConfig.ActorId, r2LLoginAccountRequest) as
                            L2R_LoginAccountResponse;

                    if (loginAccountResponse.Error != ErrorCore.ERR_SUCCESS)
                    {
                        response.Error = loginAccountResponse.Error;
                        session?.Disconnect().Coroutine();
                        session?.Dispose();
                        return;
                    }
                    
                    
                }
            }
        }
    }
}