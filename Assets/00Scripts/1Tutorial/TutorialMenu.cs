using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
//海浪
using StylizedWaterShader;
//

public class TutorialMenu : MonoBehaviour
{
    //調整鋸齒狀
    [Header("聲音調整")]
    public AudioSource EffectSource;
    public AudioSource BGMSource, SoundSource;
    public AudioClip ClickSound,RightSound;
    [Header("語音")]
    public AudioClip TitleSound;
    public AudioClip TitleSound2,HintWatchSound,HintButtonSound,FinSound;
    [Header("頁面")]
    public GameObject TitlePage;    
    public GameObject StartButtonPage,GamePage,EndButton,LoadingPage;
    public GameObject OceanCube;
    public Transform CubeFrontRotate,CubeRotateReset,CubeOri;
    [Header("指引頁面")]
    public GameObject HintPage;
    public Text HintText;
    [Header("控制海浪")]
    public StylizedWater WaveScript;
    public Material WaveControl;
    //控制天空
    public t_change SkyChange;
    [Header("切換位置")]
    public GameObject Cam,MenuUI;
    public Transform CamRoomPlace;
    public Transform UIRoomPlace;
    [Header("轉場")]
    public Animator ToHomePage,BlackSceneUI;
    [Header("關卡設置")]
    private int ClickNum,BtnSwitchNum;
    //切換視角
    public bool isCube =true;
    public Animator GuideAnim;
    private float Timer;
    public Button[] Btns= new Button[7];
    public bool[] isClick = new bool[7];
    public string[] Ans = new string[7];
    [Header("時間軸")]
    public Slider TimeBar;
    public GameObject WaveItem,EarthWaveItem,Moon;
    // Start is called before the first frame update
    void Start()
    {
        Save_BasicInfo.Instance.WaveTutorialRecord = "";
        //找BGM
        BGMSource = GameObject.Find("SaveInfo").GetComponent<AudioSource>();
        //設定音量
        if (Save_BasicInfo.Instance.BGMOff)
        {
            BGMSource.volume = 0;
        }
        if (Save_BasicInfo.Instance.EffectOff)
        {
            EffectSource.volume = 0;
        }
        for (int i = 0; i < Ans.Length; i++)
        {
            Ans[i] = "false";
        }
        WaveControl = WaveScript.material;
        WaveControl.SetFloat("_NormalStrength", 0.1f);
        WaveControl.SetFloat("_WaveHeight", 0.1f);
        WaveControl.SetFloat("_Wavesspeed", 0.75f);
        BlackSceneUI.Play("黑幕_淡出", 0, 0);
        Invoke("BlackDisAppear", 1f);
        //語音_開始
        Invoke("Sound_Start", 1f);
    }
    void BlackDisAppear()
    {
        BlackSceneUI.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
    }
    #region 聲音
    public void Click()
    {
        EffectSource.Stop();
        SoundSource.Stop();
        EffectSource.clip = ClickSound;
        EffectSource.Play();
    }
    #region 人聲語音
    public void Sound_Start()
    {
        SoundSource.Stop();
        SoundSource.clip = TitleSound;
        SoundSource.Play();
    }

    #endregion
    #endregion
    #region 切換場景
    public void NextLevel()
    {
        string ClickAns = "" ;
        if (Save_BasicInfo.Instance.isLogin)
        {
            LoadingPage.SetActive(true);
            //已經登入過會直接紀錄資料          
            for (int i = 0; i < Ans.Length-1; i++)
            {
                ClickAns+=Ans[i]+"@";
            }
            ClickAns += Ans[6];
            Save_BasicInfo.Instance.EndTime = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
            Save_BasicInfo.Instance.WaveTutorialRecord = Save_BasicInfo.Instance.SchoolName + "@" + Save_BasicInfo.Instance.ClassName + "@" + Save_BasicInfo.Instance.StuNum +"@"+ Save_BasicInfo.Instance.Name + "@0@" + Save_BasicInfo.Instance.StartTime + "@" + Save_BasicInfo.Instance.EndTime + "@" + ClickAns;
            //上傳資料
            StartCoroutine(WriteStudentData());
        }
        else
        {
            LoadingPage.SetActive(true);           
            for (int i = 0; i < Ans.Length - 1; i++)
            {
                Save_BasicInfo.Instance.WaveTutorialRecord += Ans[i] + "@";
            }
            Save_BasicInfo.Instance.WaveTutorialRecord += Ans[6];
            //一開始進來的時候
            Save_BasicInfo.Instance.EndTime = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
            //直接跳到首頁
            SceneManager.LoadScene(1);
        }
    }
    public void ToMenu()
    {
        SceneManager.LoadScene(1);
    }
    #region 寫入資料
    IEnumerator WriteStudentData()
    {
        WWWForm form = new WWWForm();
        //辨識傳輸方式
        form.AddField("method", "write");
        //輸入要寫入的工作表號碼，從0開始到6，共7個單元，測驗及教學的資料在同一個單元的工作表內
        form.AddField("Num", "0");
        //改成寫一欄，資訊用@隔開
        form.AddField("Data", Save_BasicInfo.Instance.WaveTutorialRecord);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbz-hBL6hbzkQJd9dC6GMcWaEDUrjTYMiAyEwcjMO89nXHTTJl1NyeM4aCp7lFEeKvE_rg/exec", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Save_BasicInfo.Instance.WaveTutorialRecord = "";
                SceneManager.LoadScene(1);
                Debug.Log("Upload Complete!");
            }
        }
    }
    #endregion
    #endregion
    #region 開頭海浪動畫
    //開始遊戲_先撥動畫?
    public void StartBtn()
    {
        TitlePage.SetActive(false);
        SkyChange.isChange = true;
        //紀錄時間
        Save_BasicInfo.Instance.StartTime = DateTime.Now.ToString();
        //提示
        HintText.text = "現在可以觀察潮汐與天空的變化喔！";
        HintText.alignment = TextAnchor.MiddleCenter;
        HintPage.SetActive(true);
        //語音_提示1
        SoundSource.Stop();
        SoundSource.clip = HintWatchSound;
        SoundSource.Play();
        HintPage.GetComponent<Animator>().Play("提示_淡出", 0, 0);
        Invoke("SetWave", 12f);
    }
    //evening
    public void SetWave()
    {
        WaveControl.SetFloat("_WaveHeight", 0.3f);
        WaveControl.SetFloat("_NormalStrength", 0.15f);
        WaveControl.SetFloat("_Wavesspeed", 1.2f);
        Invoke("SetWave2", 13f);
    }
    //night
    public void SetWave2()
    {
        WaveControl.SetFloat("_WaveHeight", 0.35f);
        WaveControl.SetFloat("_NormalStrength", 0.16f);
        WaveControl.SetFloat("_Wavesspeed", 1.3f);
        Invoke("SetWave_End", 15f);
    }
    //fin
    public void SetWave_End()
    {
        //提示
        HintPage.SetActive(false);
        ToHomePage.gameObject.SetActive(true);
        ToHomePage.Play("轉場_淡入", 0, 0);
        BlackSceneUI.gameObject.SetActive(true);
        BlackSceneUI.Play("黑幕_淡入", 0, 0);
        Invoke("CloseBlackScene", 2f);
    }
    #region 轉場
    public void CloseBlackScene()
    {
        Cam.transform.position = CamRoomPlace.position;
        Cam.transform.rotation = CamRoomPlace.rotation;
        MenuUI.transform.position = UIRoomPlace.position;
        MenuUI.transform.rotation = UIRoomPlace.rotation;
        MenuUI.transform.localScale = UIRoomPlace.localScale;
        SkyChange.isChange = false;
        WaveControl.SetFloat("_NormalStrength", 0.1f);
        WaveControl.SetFloat("_WaveHeight", 0.1f);
        WaveControl.SetFloat("_Wavesspeed", 0.75f);
        Invoke("FadeOut", 2f);
    }
    public void FadeOut()
    {
        ToHomePage.Play("轉場_淡出", 0, 0);
        BlackSceneUI.Play("黑幕_淡出", 0, 0);
        Invoke("BlackDisAppear", 1f);
        Invoke("NextAppear", 1.5f);
    }
    public void NextAppear()
    {
        StartButtonPage.SetActive(true);
        //語音_標題2
        SoundSource.Stop();
        SoundSource.clip = TitleSound2;
        SoundSource.Play();
    }
    #endregion
    #endregion
    public void HintAppear()
    {
        HintText.text = "在畫面上有沙灘的模型及操作按鈕，同學可以使用VR手把點擊按鈕，看看潮汐有什麼變化喔！";

        HintText.fontSize = 63;
        HintText.alignment = TextAnchor.MiddleLeft;
        HintPage.GetComponent<Animator>().Play("提示", 0, 0);
        //語音_提示2
        SoundSource.Stop();
        SoundSource.clip = HintButtonSound;
        SoundSource.Play();
        // HintPage.GetComponent<Animator>().enabled = false;
        HintPage.SetActive(true);
    }
    public void ClickStateBtn(int Num)
    {
        OceanCube.GetComponent<Animator>().enabled = true;
        HintPage.SetActive(false);
        isClick[Num] = true;
        Ans[Num] = "true";
        BtnSwitchNum = Num;
        TimeBar.gameObject.SetActive(false);
        for (int i = 0; i < Btns.Length; i++)
        {
            Btns[i].interactable = false;
        }
        switch (BtnSwitchNum)
        {
            //視角：海邊
            case 0:
                OceanCube.transform.position = CubeFrontRotate.position;
                OceanCube.transform.rotation = CubeFrontRotate.rotation;
                isCube = true;
                OceanCube.GetComponent<Animator>().Play("Normal_方塊", 0, 0);
                GuideAnim.gameObject.SetActive(false);
                Invoke("ClickEvent", 0.5f);
                break;
            //視角：剖視
            case 1:
                OceanCube.transform.position = CubeRotateReset.position;
                OceanCube.transform.rotation = CubeRotateReset.rotation;
                isCube = true;
                OceanCube.GetComponent<Animator>().Play("Normal_方塊", 0, 0);
                GuideAnim.gameObject.SetActive(false);
                Invoke("ClickEvent", 0.5f);
                break;
            //視角：太空
            case 2:
                OceanCube.transform.position = CubeRotateReset.position;
                OceanCube.transform.rotation = CubeOri.rotation;
                GuideAnim.gameObject.SetActive(false);
                if (isCube)
                {
                    isCube = false;
                    OceanCube.GetComponent<Animator>().Play("ToSpace", 0, 0);
                    Invoke("ClickEvent", 6f);
                }
                else
                {
                    OceanCube.GetComponent<Animator>().Play("Normal_太空", 0, 0);
                    Invoke("ClickEvent", 0.5f);
                }
                break;
            //潮汐控制因素：乾潮
            case 3:
                GuideAnim.gameObject.SetActive(true);
                if (isCube)
                {
                    OceanCube.GetComponent<Animator>().Play("乾潮_方塊", 0, 0);
                    GuideAnim.Play("乾潮_方塊_說明", 0, 0);
                    Invoke("ClickEvent", 6.5f);
                }
                if (!isCube)
                {
                    OceanCube.GetComponent<Animator>().Play("乾潮_太空", 0, 0);
                    GuideAnim.Play("乾潮_太空_說明", 0, 0);
                    Invoke("ClickEvent", 8f);
                }
                break;
            //潮汐控制因素：滿潮
            case 4:
                GuideAnim.gameObject.SetActive(true);
                if (isCube)
                {
                    OceanCube.GetComponent<Animator>().Play("滿潮_方塊", 0, 0);
                    GuideAnim.Play("滿潮_方塊_說明", 0, 0);
                    Invoke("ClickEvent", 6.5f);
                }
                if (!isCube)
                {
                    OceanCube.GetComponent<Animator>().Play("滿潮_太空", 0, 0);
                    GuideAnim.Play("滿潮_太空_說明", 0, 0);
                    Invoke("ClickEvent", 8f);
                }
                break;
            //潮汐控制因素：時間
            case 5:
                GuideAnim.gameObject.SetActive(false);
                TimeBar.gameObject.SetActive(true);
                TimeBar.value = 0;
                if (isCube)
                {
                    OceanCube.GetComponent<Animator>().Play("Normal_方塊", 0, 0);
                }
                if (!isCube)
                {
                    OceanCube.GetComponent<Animator>().Play("Normal_太空", 0, 0);
                }
                Invoke("Btn5", 0.5f);
                break;
            //潮汐控制因素：引力
            case 6:
                GuideAnim.gameObject.SetActive(true);
                if (isCube)
                {
                    OceanCube.GetComponent<Animator>().Play("引力_方塊", 0, 0);
                    GuideAnim.Play("引力_方塊_說明", 0, 0);
                    Invoke("ClickEvent", 14f);
                }
                if (!isCube)
                {
                    OceanCube.GetComponent<Animator>().Play("引力_太空", 0, 0);
                    GuideAnim.Play("引力_太空_說明", 0, 0);
                    Invoke("ClickEvent", 22f);
                }
                break;
        }
    }
    #region 結束點擊
    public void ClickEvent()
    {
        EffectSource.clip = RightSound;
        EffectSource.Play();
        GuideAnim.gameObject.SetActive(false);
        for (int i = 0; i < Btns.Length; i++)
        {
            Btns[i].interactable = true;
        }
        ClickNum = 0;
        for (int i = 0; i < isClick.Length; i++)
        {
            if (isClick[i])
            {
                ClickNum++;
            }
        }
        if (ClickNum == 7)
        {
            
            EndButton.SetActive(true);
            HintText.text = "恭喜同學完成所有操作！同學是不是更了解潮汐了呢？接下來我們看看生活在潮汐間有甚麼動物吧！";
            //語音_結束
            SoundSource.Stop();
            SoundSource.clip = FinSound;
            SoundSource.Play();
            HintText.fontSize = 65;
            HintText.alignment = TextAnchor.MiddleLeft;
        }
        HintPage.SetActive(true);
    }
    #endregion
    #region 時間按鈕
    public void TimeBarEvent()
    {
        if (isCube)
        {
            WaveItem.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, TimeBar.value * 100);
        }
        else
        {
            EarthWaveItem.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, TimeBar.value * 100);
            Moon.transform.eulerAngles = new Vector3(0, 90 , (TimeBar.value * 180));
        }
    }
    public void Btn5()
    {
        OceanCube.GetComponent<Animator>().enabled = false;
        WaveItem.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
        EarthWaveItem.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0f);
        ClickEvent();
    }
    #endregion
}
