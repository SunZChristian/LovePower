using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  SunZ
    /// Date    2024.9.26
    /// Desc
    /// </summary>
    [FriendOf(typeof(LoginMainViewComponent))]
    public static partial class LoginMainViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LoginMainViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this LoginMainViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LoginMainViewComponent self)
        {

            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        
        private static void OnEventLittleCuteAction(this LoginMainViewComponent self)
        {
            Log.Info("我是小可爱");
            LoginHelper.Login(self.Root(), "SunZ", "19941129").Coroutine();
        }
        
        private static void OnEventSoCuteAction(this LoginMainViewComponent self)
        {
            Log.Info("我是大可爱");
            LoginHelper.Login(self.Root(), "GuSY", "19960115").Coroutine();
            
        }
        #endregion YIUIEvent结束
    }
}