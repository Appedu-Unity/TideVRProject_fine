using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Stage2_3_VideoPlay : MonoBehaviour
{    
    public VideoPlayer videoPlayer;
    public VideoClip[] VideoClips2;
    public VideoClip[] VideoClips3;
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    public void Pause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }
    }

    public void PlayVideo(int Num)
    {
        if (Save_BasicInfo.Instance.StageNum == "2")
        {
            videoPlayer.clip = VideoClips2[Num];
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.clip = VideoClips3[Num];
            videoPlayer.Play();
        }
    }
}
