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
        /// ��������ɹ��¼���š�
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

