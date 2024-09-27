using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LovePower)]
    public class LoginFinish_LLCreateLobbyUI: AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            await YIUIMgrComponent.Inst.Root.OpenPanelAsync<LobbyPanelComponent>();
        }
    }
}