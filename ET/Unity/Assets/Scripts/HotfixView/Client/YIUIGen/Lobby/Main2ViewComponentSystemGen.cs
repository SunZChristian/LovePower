﻿using System;
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
    [EntitySystemOf(typeof(Main2ViewComponent))]
    public static partial class Main2ViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this Main2ViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this Main2ViewComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this Main2ViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.VisibleTween;

            self.u_EventJoinRoom = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventJoinRoom");
            self.u_EventJoinRoomHandle = self.u_EventJoinRoom.Add(self.OnEventJoinRoomAction);

        }
    }
}