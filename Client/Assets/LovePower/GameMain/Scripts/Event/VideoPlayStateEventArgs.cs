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
        /// 视频播放状态改变事件编号。
        /// </summary>
        public static readonly int EventId = typeof(VideoPlayStateEventArgs).GetHashCode();

        public override int Id => EventId;

        public bool IsPlaying { get; private set; }

        /// <summary>
        /// 视频播放状态改变事件。
        /// </summary>
        /// <returns>创建的视频播放状态改变事件。</returns>
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

