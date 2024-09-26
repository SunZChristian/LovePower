namespace ET.Client
{
    [MessageHandler(SceneType.NetClient)]
    public class Main2NetClient_LoginGameHandler:MessageHandler<Scene,Main2NetClient_LoginGame,NetClient2Main_LoginGame>
    {
        protected override async ETTask Run(Scene root, Main2NetClient_LoginGame request, NetClient2Main_LoginGame response)
        {
            string account = request.Account;
            
            // 创建一个gate session，并且保存到SessionComponent中
            NetComponent netComponent = root.GetComponent<NetComponent>();
            Session gateSession = await netComponent.CreateRouterSession(NetworkHelper.ToIPEndPoint(request.GateAddress), account, account);
            gateSession.AddComponent<ClientSessionErrorComponent>();
            root.GetComponent<SessionComponent>().Session = gateSession;
            
            C2G_LoginGameGate c2GLoginGameGate = C2G_LoginGameGate.Create();
            c2GLoginGameGate.Key = request.RealmKey;
            c2GLoginGameGate.Account = request.Account;
            c2GLoginGameGate.RoleId = request.RoleId;
            G2C_LoginGameGate g2CLoginGameGate = await gateSession.Call(c2GLoginGameGate) as G2C_LoginGameGate;
            if (g2CLoginGameGate.Error != ErrorCore.ERR_SUCCESS)
            {
                response.Error = g2CLoginGameGate.Error;
                Log.Error("Gate 登录失败 ："+g2CLoginGameGate.Error);
                return;
            }
            Log.Debug("Gate 登录成功");
            
            G2C_EnterGame g2CEnterGame = await gateSession.Call(C2G_EnterGame.Create()) as G2C_EnterGame;
            if (g2CEnterGame.Error != ErrorCore.ERR_SUCCESS)
            {
                response.Error = g2CEnterGame.Error;
                Log.Error("登录Map失败 ："+g2CEnterGame.Error);
                return;
            }
            Log.Debug("登录Map成功");

            response.PlayerId = g2CEnterGame.MyUnitId;
            response.Error = ErrorCore.ERR_SUCCESS;
        }
    }
}

