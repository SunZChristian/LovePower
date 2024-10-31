using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LovePower)]
    public class LoginFinish_LLCreateLobbyUI: AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            Log.Info("打开大厅界面");
            await YIUIMgrComponent.Inst.Root.OpenPanelAsync<LobbyPanelComponent>();
        }
    }
}