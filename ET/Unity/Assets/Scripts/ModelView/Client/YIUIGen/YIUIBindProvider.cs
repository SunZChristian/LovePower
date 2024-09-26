using YIUIFramework;

namespace YIUICodeGenerated
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// 用法: YIUIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.YIUIBindProvider.Get;
    /// </summary>
    public static class YIUIBindProvider
    {
        public static YIUIBindVo[] Get()
        {
            var list          = new YIUIBindVo[4];
            list[0] = new YIUIBindVo
            {
                PkgName       = ET.Client.MainViewComponent.PkgName,
                ResName       = ET.Client.MainViewComponent.ResName,
                CodeType      = EUICodeType.View,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.MainViewComponent),
            };
            list[1] = new YIUIBindVo
            {
                PkgName       = ET.Client.Main2ViewComponent.PkgName,
                ResName       = ET.Client.Main2ViewComponent.ResName,
                CodeType      = EUICodeType.View,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.Main2ViewComponent),
            };
            list[2] = new YIUIBindVo
            {
                PkgName       = ET.Client.Main3ViewComponent.PkgName,
                ResName       = ET.Client.Main3ViewComponent.ResName,
                CodeType      = EUICodeType.View,
                PanelLayer    = EPanelLayer.Any,
                ComponentType = typeof(ET.Client.Main3ViewComponent),
            };
            list[3] = new YIUIBindVo
            {
                PkgName       = ET.Client.LobbyPanelComponent.PkgName,
                ResName       = ET.Client.LobbyPanelComponent.ResName,
                CodeType      = EUICodeType.Panel,
                PanelLayer    = EPanelLayer.Panel,
                ComponentType = typeof(ET.Client.LobbyPanelComponent),
            };

            return list;
        }
    }
}