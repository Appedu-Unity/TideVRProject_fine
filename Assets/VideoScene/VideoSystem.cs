using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSystem : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public VideoClip[] VideoClips;
    public int Order = 0;
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //撥放多個影片
    public void PlayPause()
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
    //可以撥放上一個影片及下一個影片
    public void Play_Ver2()
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
    public void PreviousVideo()
    {
        if (Order <= 0)
        {
            Order = VideoClips.Length-1;
            videoPlayer.clip = VideoClips[Order];
            videoPlayer.Play();
        }
        else
        {
            Order--;
            videoPlayer.clip = VideoClips[Order];
            videoPlayer.Play();
        }
    }
    public void NextVideo()
    {
        if (Order >= VideoClips.Length-1)
        {
            Order = 0;
            videoPlayer.clip = VideoClips[Order];
            videoPlayer.Play();
        }
        else
        {
            Order++;
            videoPlayer.clip = VideoClips[Order];
            videoPlayer.Play();
        }
    }
}
