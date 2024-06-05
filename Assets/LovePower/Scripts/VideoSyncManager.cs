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
        }

        [Command]
        public void CmdPlayVideo()
        {
            isPlaying = true;
        }

        [Command]
        public void CmdPauseVideo()
        {
            isPlaying = false;
        }

        [Command]
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
            videoPlayer.time = newValue;
        }
    }
}

