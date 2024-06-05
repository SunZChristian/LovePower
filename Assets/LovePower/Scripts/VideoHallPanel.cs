using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LovePower
{
    public class VideoHallPanel : MonoBehaviour
    {
        public VideoSyncManager videoSyncManager;
        public Slider m_slider_videoProgress;
        public Button m_btn_play;
        public Button m_btn_pause;

        private void Awake()
        {
            m_slider_videoProgress.onValueChanged.AddListener(OnSliderVideoProgress);
            m_btn_play.onClick.AddListener(OnBtnPlay);
            m_btn_pause.onClick.AddListener(OnBtnPause);
        }

        private void Start()
        {
            SetPlayState(false);
        }

        private void Update()
        {
            if (videoSyncManager != null)
            {
                m_slider_videoProgress.SetValueWithoutNotify((float)(videoSyncManager.videoPlayer.time / videoSyncManager.videoPlayer.clip.length));
            }
        }

        private void OnSliderVideoProgress(float value)
        {
            if (videoSyncManager.authority)
            {
                double newTime = value * videoSyncManager.videoPlayer.clip.length;
                videoSyncManager.CmdSetTime(newTime);
            }

        }

        private void OnBtnPlay()
        {
            if (videoSyncManager.authority)
                videoSyncManager.CmdPlayVideo();
        }

        private void OnBtnPause()
        {
            if (videoSyncManager.authority)
                videoSyncManager.CmdPauseVideo();
        }

        public void SetPlayState(bool isPlay)
        {
            m_btn_play.gameObject.SetActive(!isPlay);
            m_btn_pause.gameObject.SetActive(isPlay);
        }

        public void SetVideoSliderValue(float value)
        {
            m_slider_videoProgress.SetValueWithoutNotify(value);
        }
    }
}