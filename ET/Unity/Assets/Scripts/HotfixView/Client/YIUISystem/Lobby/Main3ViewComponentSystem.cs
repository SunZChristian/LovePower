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
    [FriendOf(typeof(Main3ViewComponent))]
    public static partial class Main3ViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this Main3ViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this Main3ViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this Main3ViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        
        private static void OnEventJoinRoomAction(this Main3ViewComponent self)
        {
            
        }
        #endregion YIUIEvent结束
    }
}