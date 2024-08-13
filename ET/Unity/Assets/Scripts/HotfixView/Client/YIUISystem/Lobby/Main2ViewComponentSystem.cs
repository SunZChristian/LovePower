using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  SunZ
    /// Date    2024.8.13
    /// Desc
    /// </summary>
    [FriendOf(typeof(Main2ViewComponent))]
    public static partial class Main2ViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this Main2ViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this Main2ViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this Main2ViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        
        private static void OnEventJoinRoomAction(this Main2ViewComponent self)
        {
            
        }
        #endregion YIUIEvent结束
    }
}