using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  SunZ
    /// Date    2024.8.12
    /// Desc
    /// </summary>
    [FriendOf(typeof(LobbyPanelComponent))]
    public static partial class LobbyPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LobbyPanelComponent self)
        {
            Log.Info($"大厅：初始化");
        }

        [EntitySystem]
        private static void Destroy(this LobbyPanelComponent self)
        {
            Log.Info($"大厅：删除");
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LobbyPanelComponent self)
        {
            Log.Info($"大厅：open");
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}