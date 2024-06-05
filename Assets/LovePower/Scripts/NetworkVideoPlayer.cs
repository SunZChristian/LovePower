using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Video;
using UnityEngine.UI;

public class NetworkVideoPlayer : NetworkBehaviour
{
    public VideoPlayer videoPlayer;
    public Slider videoSlider;
    void Start()
    {
        if (!videoPlayer)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (isServer)
        {
            videoPlayer.playOnAwake = false;
        }

        videoSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Update()
    {
        if (isServer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (videoPlayer.isPlaying)
                {
                    RpcPauseVideo();
                }
                else
                {
                    RpcPlayVideo();
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                videoPlayer.time += 10;
                RpcSetVideoTime(videoPlayer.time);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                videoPlayer.time -= 10;
                RpcSetVideoTime(videoPlayer.time);
            }

            videoSlider.value = (float)(videoPlayer.time / videoPlayer.length);
        }
    }

    [ClientRpc]
    void RpcPlayVideo()
    {
        videoPlayer.Play();
    }

    [ClientRpc]
    void RpcPauseVideo()
    {
        videoPlayer.Pause();
    }

    [ClientRpc]
    void RpcSetVideoTime(double time)
    {
        videoPlayer.time = time;
    }

    void OnSliderValueChanged(float value)
    {
        if (isServer)
        {
            double time = value * videoPlayer.length;
            videoPlayer.time = time;
            RpcSetVideoTime(time);
        }
    }
}

