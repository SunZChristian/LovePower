using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIComponent))]
    [FriendOf(typeof(YIUIWindowComponent))]
    [FriendOf(typeof(YIUIViewComponent))]
    [EntitySystemOf(typeof(LoginMainViewComponent))]
    public static partial class LoginMainViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LoginMainViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LoginMainViewComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this LoginMainViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.VisibleTween;

            self.u_EventSoCute = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventSoCute");
            self.u_EventSoCuteHandle = self.u_EventSoCute.Add(self.OnEventSoCuteAction);
            self.u_EventLittleCute = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventLittleCute");
            self.u_EventLittleCuteHandle = self.u_EventLittleCute.Add(self.OnEventLittleCuteAction);

        }
    }
}