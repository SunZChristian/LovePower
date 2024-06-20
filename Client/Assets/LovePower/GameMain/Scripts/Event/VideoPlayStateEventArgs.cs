using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class VideoPlayStateEventArgs : GameEventArgs
    {
        /// <summary>
        /// ��Ƶ����״̬�ı��¼���š�
        /// </summary>
        public static readonly int EventId = typeof(VideoPlayStateEventArgs).GetHashCode();

        public override int Id => EventId;

        public bool IsPlaying { get; private set; }

        /// <summary>
        /// ��Ƶ����״̬�ı��¼���
        /// </summary>
        /// <returns>��������Ƶ����״̬�ı��¼���</returns>
        public static VideoPlayStateEventArgs Create(bool isPlaying)
        {
            VideoPlayStateEventArgs loadDataTableSuccessEventArgs = ReferencePool.Acquire<VideoPlayStateEventArgs>();
            loadDataTableSuccessEventArgs.IsPlaying = isPlaying;
            return loadDataTableSuccessEventArgs;
        }

        public override void Clear()
        {
            
        }
    }
}

