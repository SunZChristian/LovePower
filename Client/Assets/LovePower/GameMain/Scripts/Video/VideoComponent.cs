using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

namespace LovePower
{
    public class VideoComponent : GameFrameworkComponent
    {
        public UnityEvent OnPrepareCompleted;

        public RenderTexture VideoRenderTexture { get; private set; }

        private VideoPlayer m_videoPlayer;

        //private MediaPlayer m_mediaPlayer;

        public double TotalTime
        {
            get
            {
                if (m_videoPlayer == null || string.IsNullOrEmpty(m_videoPlayer.url))
                    return 0;
                return m_videoPlayer.length;
            }
        }

        public double CurentTime
        {
            get
            {
                if (m_videoPlayer == null || string.IsNullOrEmpty(m_videoPlayer.url))
                    return 0;
                return m_videoPlayer.time;
            }
        }

        public bool IsPlaying
        {
            get {
                if (m_videoPlayer == null)
                    return false;
                return m_videoPlayer.isPlaying;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (m_videoPlayer == null)
                m_videoPlayer = GetComponent<VideoPlayer>();

            if (m_videoPlayer != null)
            {
                m_videoPlayer.playOnAwake = false;
                if (m_videoPlayer.targetTexture == null)
                {
                    if (VideoRenderTexture == null)
                        VideoRenderTexture = RenderTexture.GetTemporary(1920, 1080, 24);
                    VideoRenderTexture.name = "VideoTexture";
                    m_videoPlayer.targetTexture = VideoRenderTexture;
                }

                m_videoPlayer.prepareCompleted += PrepareCompleted;
            }

        }

        #region 公有方法
        public void Play()
        {
            if (m_videoPlayer == null)
                return;
            if (string.IsNullOrEmpty(m_videoPlayer.url))
                return;
            if (m_videoPlayer.isPlaying)
                return;

            m_videoPlayer.Play();
            GameEntry.Event.FireNow(this, VideoPlayStateEventArgs.Create(true));
        }

        public void Pause()
        {
            if (m_videoPlayer == null)
                return;
            if (string.IsNullOrEmpty(m_videoPlayer.url))
                return;
            if (m_videoPlayer.isPaused)
                return;

            m_videoPlayer.Pause();
            GameEntry.Event.FireNow(this, VideoPlayStateEventArgs.Create(false));
        }

        public void SetPlayProgress(float percentage)
        {
            if (m_videoPlayer == null)
                return;

            m_videoPlayer.time = m_videoPlayer.length * percentage;
        }

        public void ChangeVideoUrl(string newUrl)
        {
            if (m_videoPlayer == null)
                return;
            if (string.IsNullOrEmpty(newUrl))
                return;

            m_videoPlayer.url = newUrl;
        }

        #endregion


        #region 私有方法
        /// <summary>
        /// 视频准备完成时被执行
        /// </summary>
        /// <param name="source"></param>
        private void PrepareCompleted(VideoPlayer source)
        {
            Log.Info("视频准备完成：" + source.url);
            OnPrepareCompleted?.Invoke();
        }
        #endregion
    }
}