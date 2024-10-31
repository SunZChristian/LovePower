namespace ET.Client
{
    [Event(SceneType.LovePower)]
    public class AppStartInitFinish_LLCreateLoginUI: AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene root, AppStartInitFinish args)
        {
            await YIUIMgrComponent.Inst.Root.OpenPanelAsync<LoginPanelComponent>();
        }
    }
}

