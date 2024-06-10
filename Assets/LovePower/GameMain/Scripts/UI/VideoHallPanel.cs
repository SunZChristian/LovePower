using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

namespace LovePower
{
    public class VideoHallPanel : MonoBehaviour
    {
        public VideoSyncManager videoSyncManager;
        public Slider m_slider_videoProgress;
        public Button m_btn_play;
        public Button m_btn_pause;
        public TextMeshProUGUI m_txt_time;

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


            }
        }

        private void OnSliderVideoProgress(float value)
        {
            //if (videoSyncManager.authority)
            //{

            //}
            double newTime = value * videoSyncManager.videoPlayer.clip.length;
            videoSyncManager.CmdSetTime(newTime);
        }

        private void OnBtnPlay()
        {
            //if (videoSyncManager.authority)
            videoSyncManager.CmdPlayVideo();
        }

        private void OnBtnPause()
        {
            //if (videoSyncManager.authority)
            videoSyncManager.CmdPauseVideo();
        }

        public void SetPlayState(bool isPlay)
        {
            m_btn_play.gameObject.SetActive(!isPlay);
            m_btn_pause.gameObject.SetActive(isPlay);
        }

        public void SetVideoSliderValue(float value, float curTime, float totalTime)
        {
            m_slider_videoProgress.SetValueWithoutNotify(value);
            RefreshTime(curTime, totalTime);
        }

        private void RefreshTime(float seconds, float totalSeconds)
        {
            m_txt_time.text = FormatTime(seconds) + "/" + FormatTime(totalSeconds);
        }

        public static string FormatTime(float seconds)
        {
            int hours = (int)Mathf.Floor(seconds / 60 / 60);
            int remainingSeconds = (int)Mathf.Round(seconds % (60 * 60));
            int minutes = (int)Mathf.Floor(remainingSeconds / 60);
            remainingSeconds = (int)Mathf.Round(remainingSeconds % 60);

            string formatted = string.Empty;
            if (hours > 0)
            {
                formatted = hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + remainingSeconds.ToString("D2");
            }
            else
            {
                formatted = minutes.ToString("D2") + ":" + remainingSeconds.ToString("D2");
            }

            return formatted;
        }
    }
}