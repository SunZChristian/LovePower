namespace ET.Client
{
    [Event(SceneType.LovePower)]
    public class LoginFinish_LLRemoveLoginUI: AEvent<Scene, LoginFinish>
    {
        protected override async ETTask Run(Scene scene, LoginFinish args)
        {
            await YIUIMgrComponent.Inst.ClosePanelAsync<LoginPanelComponent>();
        }
    }
}