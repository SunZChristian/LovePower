using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using System.IO;

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

        private float timer = 0;

        private void Update()
        {
            if (GameEntry.TcpClient.IsNetworkConnected && PlayerRuntimeData.IsOwner)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 1;
                    var operationCode = IsPlaying ? EVideoOperation.Play : EVideoOperation.Pause;
                    var progress = (int)(CurentTime / TotalTime * 10000);
                    GameEntry.TcpClient.SyncRoomStatus((int)operationCode, progress);
                }
            }
        }

        #region ���з���
        public void Play()
        {
            if (m_videoPlayer == null)
                return;
            if (string.IsNullOrEmpty(m_videoPlayer.url))
                return;
            if (m_videoPlayer.isPlaying)
                return;
            if (!IsVideoFile(m_videoPlayer.url))
            {
                GameEntry.UI.ShowAlert("���ò�Ʋ�����Ƶ�ļ�Ŷ~");
                return;
            }

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

        /// <summary>
        /// ���ò��Ž���
        /// </summary>
        /// <param name="percentage">0-1��ֵ</param>
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


        #region ˽�з���
        /// <summary>
        /// ��Ƶ׼�����ʱ��ִ��
        /// </summary>
        /// <param name="source"></param>
        private void PrepareCompleted(VideoPlayer source)
        {
            Log.Info("��Ƶ׼����ɣ�" + source.url);
            OnPrepareCompleted?.Invoke();
        }


        private bool IsVideoFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            string extension = Path.GetExtension(filePath).ToLower();
            string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv", ".webm" };

            foreach (string videoExtension in videoExtensions)
            {
                if (extension == videoExtension)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}