using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LovePower)]
    public class LoginFinish_LLCreateLobbyUI: AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            Log.Info("收到消息！！！");
        }
    }
}

