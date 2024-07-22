using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace LovePower
{
    /// <summary>
    /// 视频播放面板
    /// </summary>
    public class VideoHallPanel : PanelBase
    {
        public RawImage m_rawimg_video;
        public SunZSlider m_slider_videoProgress;
        public Button m_btn_play;
        public Button m_btn_pause;
        public Button m_btn_selectfile;
        public TextMeshProUGUI m_txt_time;

        public bool isPlayingWhenPrePointDown;

        private void Awake()
        {
            m_rawimg_video.texture = GameEntry.Video.VideoRenderTexture;

            m_slider_videoProgress.onValueChanged.AddListener(OnSliderVideoProgress);
            m_slider_videoProgress.OnPointDownEvent.AddListener(OnSliderPointDown);
            m_slider_videoProgress.OnPointUpEvent.AddListener(OnSliderPointUp);
            m_btn_play.onClick.AddListener(OnBtnPlay);
            m_btn_pause.onClick.AddListener(OnBtnPause);
            m_btn_selectfile.onClick.AddListener(OnBtnSelectFile);


            var canInteractable = PlayerRuntimeData.IsOwner;
            m_slider_videoProgress.interactable = canInteractable;
            m_btn_play.interactable = canInteractable;
            m_btn_pause.interactable = canInteractable;            
        }

        private void Start()
        {
            SetPlayState(GameEntry.Video.IsPlaying);

            GameEntry.Event.Subscribe(VideoPlayStateEventArgs.EventId, EventVideoPlayStateChaneg);
            GameEntry.Video.OnPrepareCompleted.AddListener(OnVideoPrepareCompleted);

            if (!PlayerRuntimeData.IsOwner)
            {
                GameEntry.TcpClient.GetRoomStatus();
            }
        }

        private void OnDestroy()
        {
            GameEntry.Event.Unsubscribe(VideoPlayStateEventArgs.EventId, EventVideoPlayStateChaneg);
            GameEntry.Video.OnPrepareCompleted.RemoveListener(OnVideoPrepareCompleted);
        }

        private void Update()
        {
            //if (GameEntry.Video.IsPlaying)
            //{
            SetVideoSliderValueWithoutNotify((float)GameEntry.Video.CurentTime, (float)GameEntry.Video.TotalTime);
            //}
        }

        private void OnSliderVideoProgress(float value)
        {
            GameEntry.Video.SetPlayProgress(value);
        }

        private void OnSliderPointDown()
        {
            isPlayingWhenPrePointDown = GameEntry.Video.IsPlaying;

            GameEntry.Video.Pause();
        }

        private void OnSliderPointUp()
        {
            if (isPlayingWhenPrePointDown)
                GameEntry.Video.Play();

            GameEntry.TcpClient.ChangeVideoProgress(m_slider_videoProgress.value);
        }

        private void OnBtnPlay()
        {
            GameEntry.Video.Play();
            GameEntry.TcpClient.PlayVideo();
        }

        private void OnBtnPause()
        {
            GameEntry.Video.Pause();
            GameEntry.TcpClient.PauseVideo();
        }

        private void OnBtnSelectFile()
        {
            Log.Info("选择文件");

            //if (Application.platform == RuntimePlatform.WindowsEditor
            //    || Application.platform == RuntimePlatform.WindowsPlayer)
            //{
            //    Log.Info("当前是PC");
            //    string path = OpenDialogHelper.SelectFile("视频文件(*.mp4*.mov*.mpg*.mpeg*.avi*.asf)\0*.mp4;*.mov*.mpg*.mpeg*.avi*.asf", null, (url) =>
            //    {
            //        Log.Info("path:" + url);

            //        //切换视频
            //        GameEntry.Video.ChangeVideoUrl(url);
            //    });
            //}
            //else if (Application.platform == RuntimePlatform.Android)
            //{
            Log.Info("当前是安卓");
            string[] fileTypes = new string[] { "video/*" };
            var permission = NativeFilePicker.PickFile(OnPickFileHandler, fileTypes);//视频（mp4、mov、wav、avi）
            Debug.Log("文件访问权限: " + permission);
            //}
        }

        public void SetPlayState(bool isPlay)
        {
            m_btn_play.gameObject.SetActive(!isPlay);
            m_btn_pause.gameObject.SetActive(isPlay);
        }

        public void SetVideoSliderValueWithoutNotify(float curTime, float totalTime)
        {
            m_slider_videoProgress.SetValueWithoutNotify(curTime / totalTime);
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

        public void EventVideoPlayStateChaneg(object sender, GameEventArgs e)
        {
            VideoPlayStateEventArgs args = (VideoPlayStateEventArgs)e;
            if (args != null)
            {
                SetPlayState(args.IsPlaying);
            }
        }

        public void OnVideoPrepareCompleted()
        {
            //重置进度条Handle的位置
            m_slider_videoProgress.handleRect.anchoredPosition = Vector2.zero;
        }

        private void OnPickFileHandler(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Log.Info("取消了");
            }
            else
            {
                Log.Info("path:" + path);

                //切换视频
                GameEntry.Video.ChangeVideoUrl(path);
            }
            
        }
    }
}