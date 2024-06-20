using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

namespace LovePower
{
    /// <summary>
    ///  ”∆µ≤•∑≈√Ê∞Â
    /// </summary>
    public class VideoHallPanel : PanelBase
    {
        //public VideoSyncManager videoSyncManager;
        public SunZSlider m_slider_videoProgress;
        public Button m_btn_play;
        public Button m_btn_pause;
        public TextMeshProUGUI m_txt_time;

        public bool isPauseWhenPrePointDown;

        private void Awake()
        {
            m_slider_videoProgress.onValueChanged.AddListener(OnSliderVideoProgress);
            m_slider_videoProgress.OnPointDownEvent.AddListener(OnSliderPointDown);
            m_slider_videoProgress.OnPointUpEvent.AddListener(OnSliderPointUp);
            m_btn_play.onClick.AddListener(OnBtnPlay);
            m_btn_pause.onClick.AddListener(OnBtnPause);
        }

        private void Start()
        {
            SetPlayState(false);
        }

        private void Update()
        {
            if (GameEntry.Video.IsPlaying)
            {
                SetVideoSliderValueWithoutNotify((float)GameEntry.Video.CurentTime, (float)GameEntry.Video.TotalTime);
            }
        }

        private void OnSliderVideoProgress(float value)
        {
            GameEntry.Video.SetPlayProgress(value);
        }

        private void OnSliderPointDown()
        {
            GameEntry.Video.Pause();
        }

        private void OnSliderPointUp()
        {
            
        }

        private void OnBtnPlay()
        {
            GameEntry.Video.Play();
            SetPlayState(true);
        }

        private void OnBtnPause()
        {
            GameEntry.Video.Pause();
            SetPlayState(false);
        }

        public void SetPlayState(bool isPlay)
        {
            m_btn_play.gameObject.SetActive(!isPlay);
            m_btn_pause.gameObject.SetActive(isPlay);
        }

        public void SetVideoSliderValueWithoutNotify(float curTime, float totalTime)
        {
            m_slider_videoProgress.SetValueWithoutNotify(curTime/totalTime);
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