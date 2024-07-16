using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class CreateRoomSuccessArgs : GameEventArgs
    {
        /// <summary>
        /// 创建房间成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(CreateRoomSuccessArgs).GetHashCode();

        public override int Id => EventId;

        public static CreateRoomSuccessArgs Create()
        {
            var args = ReferencePool.Acquire<CreateRoomSuccessArgs>();
            return args;
        }

        public override void Clear()
        {
            
        }
    }
}

