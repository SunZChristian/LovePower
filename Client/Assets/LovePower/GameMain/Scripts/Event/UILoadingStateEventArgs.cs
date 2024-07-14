using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class UILoadingStateEventArgs : GameEventArgs
    {
        /// <summary>
        /// ��Ƶ����״̬�ı��¼���š�
        /// </summary>
        public static readonly int EventId = typeof(UILoadingStateEventArgs).GetHashCode();

        public override int Id => EventId;

        public bool Show { get; private set; }

        public static UILoadingStateEventArgs Create(bool show)
        {
            var uiLoadingStateEventArgs = ReferencePool.Acquire<UILoadingStateEventArgs>();
            uiLoadingStateEventArgs.Show = show;
            return uiLoadingStateEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}

