using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Video;
using UnityEngine.UI;

namespace LovePower
{
    public class VideoSyncManager : NetworkBehaviour
    {
        public VideoPlayer videoPlayer;

        [SyncVar(hook = nameof(OnPlayStateChanged))]
        private bool isPlaying;

        [SyncVar(hook = nameof(OnTimeChanged))]
        private double videoTime;

        public VideoHallPanel m_panel;


        private void Start()
        {
            // 监听视频播放事件以更新UI
            //videoPlayer.prepareCompleted += OnVideoReady;
            //videoPlayer.frameReady += OnFrameReady;

            videoPlayer.loopPointReached += (source) => { Debug.Log("播放完成"); CmdPauseVideo(); };
        }

        private void Update()
        {
            if (isServer)
            {
                if (isPlaying)
                {
                    videoTime = videoPlayer.time;
                }
            }

            if (!isServer)
            {
                if (isPlaying != videoPlayer.isPlaying)
                {
                    if (isPlaying)
                    {
                        videoPlayer.Play();
                    }
                    else
                    {
                        videoPlayer.Pause();
                    }
                }

                if (Mathf.Abs((float)(videoPlayer.time - videoTime)) > 0.1f)
                {
                    videoPlayer.time = videoTime;
                }
            }

            var value = (float)(videoPlayer.time / videoPlayer.clip.length);
            m_panel.SetVideoSliderValue(value, (float)videoPlayer.time, (float)videoPlayer.clip.length);
        }

        private void OnVideoReady(VideoPlayer source)
        {
            // 视频准备完成后可以开始播放  
            //videoPlayer.Play();
        }

        private void OnFrameReady(VideoPlayer source, long frameIdx)
        {
            // 更新当前时间文本  
            //currentTimeText.text = TimeUtility.FormatTime(source.time);

            // 更新Slider的Value以匹配视频的当前帧  
            //videoSlider.value = (int)(source.frame * videoSlider.maxValue / videoPlayer.frameCount);

            //m_panel.SetVideoSliderValue()
        }

        //[Command]
        public void CmdPlayVideo()
        {
            if (!isServer)
                return;

            isPlaying = true;
            m_panel.SetPlayState(true);
        }

        //[Command]
        public void CmdPauseVideo()
        {
            if (!isServer)
                return;
            isPlaying = false;
            m_panel.SetPlayState(false);
        }

        //[Command]
        public void CmdSetTime(double time)
        {
            videoTime = time;
            videoPlayer.time = time;
        }

        private void OnPlayStateChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.Pause();
            }
        }

        private void OnTimeChanged(double oldValue, double newValue)
        {
            if(!isServer)
                videoPlayer.time = newValue;
        }
    }
}

