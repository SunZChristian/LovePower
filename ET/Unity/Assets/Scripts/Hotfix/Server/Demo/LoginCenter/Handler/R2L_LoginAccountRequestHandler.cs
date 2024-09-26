namespace ET.Server
{
    [MessageHandler(SceneType.LoginCenter)]
    public class R2L_LoginAccountRequestHandler:MessageHandler<Scene,R2L_LoginAccountRequest,L2R_LoginAccountResponse>
    {
        protected override async ETTask Run(Scene scene, R2L_LoginAccountRequest request, L2R_LoginAccountResponse response)
        {
            long accountId = request.Account.GetLongHashCode();

            CoroutineLockComponent coroutineLockComponent = scene.Root().GetComponent<CoroutineLockComponent>();
            using (await coroutineLockComponent.Wait(CoroutineLockType.LoginCenterLock,accountId))
            {
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExist(accountId))
                {
                    response.Error = ErrorCore.ERR_SUCCESS;
                    return;
                }

                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(accountId);
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone, request.Account);
                L2G_DisconnectGateUnit l2GDisconnectGateUnit = L2G_DisconnectGateUnit.Create();
                l2GDisconnectGateUnit.Account = request.Account;
                G2L_DisconnectGateUnit g2LDisconnectGateUnit =await scene.GetComponent<MessageSender>().Call(gateConfig.ActorId, l2GDisconnectGateUnit) as G2L_DisconnectGateUnit;

               
            }
        }
    }
}