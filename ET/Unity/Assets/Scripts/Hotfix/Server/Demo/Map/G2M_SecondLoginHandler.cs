﻿namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class G2M_SecondLoginHandler : MessageLocationHandler<Unit, G2M_SecondLogin,M2G_SecondLogin>
    {
        protected override async ETTask Run(Unit unit, G2M_SecondLogin request, M2G_SecondLogin response)
        {

            Log.Console("作业:二次登陆逻辑，补全G2M_SecondLogin消息处理逻辑");
            await ETTask.CompletedTask;
        }
    }
}