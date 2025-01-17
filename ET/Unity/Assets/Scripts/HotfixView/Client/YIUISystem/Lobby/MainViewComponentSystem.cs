﻿using System;
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
    [FriendOf(typeof(MainViewComponent))]
    public static partial class MainViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this MainViewComponent self)
        {
            Log.Info($"mainview 初始化");
            
            Log.Info($"当前显隐状态："+self.UIBase.ActiveSelf);
        }
        
        [EntitySystem]
        private static void Destroy(this MainViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this MainViewComponent self)
        {
            //self.UIBase.SetActive(true);
           
            await ETTask.CompletedTask;
            return true;
        }

        
        #region YIUIEvent开始
        
        private static void OnEventJoinRoomAction(this MainViewComponent self)
        {
            Log.Info($"加入房间");
            
            //进入房间
            
        }
        
        private static void OnEventCreateRoomAction(this MainViewComponent self)
        {
            Log.Info($"创建房间");
            
            //创建房间
        }
        #endregion YIUIEvent结束
    }
}