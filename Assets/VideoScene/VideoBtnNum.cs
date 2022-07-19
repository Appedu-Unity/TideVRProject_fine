using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoBtnNum : MonoBehaviour
{
    public int VideoNum,SetNumber;
    //public bool isSelect,NotSelect,Select;
    //public Text Ans;
    public VideoSystem Videos;
    //public RecordNumbers Record;
    // Start is called before the first frame update
    void Start()
    {
        //Ans.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayRightVideo()
    {
        Videos.GetComponent<VideoSystem>();
        Videos.videoPlayer.clip = Videos.VideoClips[VideoNum];
        Videos.PlayPause();

    }
    //public void Selected()
    //{
    //    if (Select == true)
    //    {
    //        Select = false;
    //    }
    //    else
    //    {
    //        Select = true;
    //    }
    //    Bool();
    //    Record.GetComponent<RecordNumbers>().SetNum();
    //    SetNumber = int.Parse(Ans.text);
    //}
    //public void Bool()
    //{
    //    if (Select == true)
    //    {
    //        isSelect = true;
    //        NotSelect = false;
    //    }
    //    else
    //    {
    //        isSelect = false;
    //        NotSelect = true;
    //    }
    //}
}
