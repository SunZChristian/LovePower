namespace ET.Server
{
    [MessageHandler(SceneType.Gate)]
    [FriendOfAttribute(typeof(ET.Server.Player))]
    public class L2G_DisconnectGateUnitHandler : MessageHandler<Scene, L2G_DisconnectGateUnit, G2L_DisconnectGateUnit>
    {
        protected override async ETTask Run(Scene scene, L2G_DisconnectGateUnit request, G2L_DisconnectGateUnit response)
        {
            CoroutineLockComponent coroutineLockComponent = scene.GetComponent<CoroutineLockComponent>();
            using (await coroutineLockComponent.Wait(CoroutineLockType.LoginGate, request.Account.GetLongHashCode()))
            {
                PlayerComponent playerComponent = scene.GetComponent<PlayerComponent>();
                Player player = playerComponent.GetByAccount(request.Account);

                if (player == null)
                {
                    //当前没有同账户名在线的情况，不需要执行踢人下线的逻辑
                    return;
                }

                scene.GetComponent<GateSessionKeyComponent>().Remove(request.Account.GetLongHashCode());
                
                Session gateSession = player.GetComponent<PlayerSessionComponent>()?.Session;
                if (gateSession != null && !gateSession.IsDisposed)
                {
                    A2C_Disconnect a2CDisconnect = A2C_Disconnect.Create();
                    a2CDisconnect.Error = ErrorCore.ERR_OtherAccountLogin;
                    gateSession.Send(a2CDisconnect);
                    gateSession?.Disconnect().Coroutine();
                }

                if ( player.GetComponent<PlayerSessionComponent>()?.Session != null)
                {
                    player.GetComponent<PlayerSessionComponent>().Session = null;
                }
                player.AddComponent<PlayerOfflineOutTimeComponent>();
            }
        }
    }
}