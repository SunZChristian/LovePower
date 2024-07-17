using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class JoinRoomSuccessArgs : GameEventArgs
    {
         /// <summary>
        /// 创建房间成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(JoinRoomSuccessArgs).GetHashCode();

        public override int Id => EventId;

        public static JoinRoomSuccessArgs Create()
        {
            var args = ReferencePool.Acquire<JoinRoomSuccessArgs>();
            return args;
        }

        public override void Clear()
        {
            
        }
    }
}

