using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Stage2_3Menu : MonoBehaviour
{
    
    [Header("聲音調整")]
    public AudioSource EffectSource;
    public AudioSource BGMSource, SoundSource;
    public AudioClip ClickSound, RightSound, WrongSound;
    [Header("語音")]
    private bool isPlay;
    //教學
    public AudioClip Tu_TitleSound;
    public AudioClip Tu_DescriptionSound,Tu_Fin1Sound, Tu_Fin2Sound;
    //測驗
    public AudioClip Test_TitleSound, Test_FinSound;
    public AudioClip[] Test_HintSound = new AudioClip[2];
    [Header("基本設定")]
    //看OFF的Toggle
    public Toggle BGMOFF, EffectOFF;
    public GameObject LoadingPage;
    public string WriteInRecord;
    public GameObject SettingPage;
    public Transform SettingOriPlace, Setting_TuChangePlace;
    //該關限定
    public Text Tu_TitleText,Test_TitleText,Test_ToTuBtnText;
    [Header("基本按鈕")]
    public GameObject Change_HomeButton;
    public GameObject BasicBtns_Ori,BasicBtns_Change;
    [Header("紀錄")]
    public string[] TutorialRecords_2 = new string[5];
    public string[] TutorialRecords_3 = new string[8];
    public string[] TestRecords = new string[5];
    [Header("教學")]
    public GameObject TutorialModePage;
    [Header("頁面")]
    public GameObject Tu_TiltePage;
    public GameObject Tu_GamPage,Tu_EndConPage;
    public Transform EndPageOriPlace,EndPageChangePlace;
    [Header("設定影片")]
    public bool isPlayVideo;
    public GameObject VideoObject;
    public GameObject ReplayButton;
    public Stage2_3_VideoPlay Videoplay;
    [Header("關卡設定")]
    public int AnimalNum;
    public GameObject Tu_BackBtns;
    public Button Tu_WatchButton, Tu_ExerciseButton;
    public Transform Tu_TagPlace,Tu_TagAnimalPlace;
    public GameObject[] Tu_TagPrefabs_2 = new GameObject[5];
    public GameObject[] Tu_TagPrefabs_3 = new GameObject[8];
    public GameObject[] Tu_TagAnimPrefabs_2 = new GameObject[5];
    public GameObject[] Tu_TagAnimPrefabs_3 = new GameObject[8];
    [Header("動物生成")]
    public Transform BirdCreatePlace;
    public Transform CrabCreatePlace, ShellCreatePlace;
    public Transform[] BirdCreateSpots, CrabCreateSpots, ShellCreateSpots;
    public GameObject[] CreateBirdPrefabs = new GameObject[5];
    public GameObject[] CreateCrabPrefabs = new GameObject[5];
    public GameObject[] CreateShellPrefabs = new GameObject[3];
    public List<Stage2_3Click> BirdClicks, SeaAnimalClicks;
    [Header("測驗")]
    public bool TestFin;
    public GameObject TestModePage;
    public GameObject TestTitlePage, Test_OrganPage, Test_ExercisePage, TestResultPage, Test_EndConPage, Test_EndFinPage;  
    public int TestQuizNum;
    //0：器官、1：運動
    public int TestType;
    public Stage2_3Test TestMenu;
    public string[] StuData;
    [Header("測驗種類")]
    public int TestScore;
    public string Test_ScoreResult;
    public string[] Test_CountScore;
    [Header("計時")]
    public bool isTesting;
    public float Timer;
    public string[] TestTimer = new string[5];
    [Header("計算分數")]
    public Transform Test_QuizTextPlace;
    public Transform Test_ScoreTextPlace;
    public GameObject Test_ScoreItemPrefab, Test_QuizTextPrefab;
    public List<string> TestData;
    public List<string> Test_SortData;
    //單元二鳥類 StageNum：2、單元三蟹類、跟螺貝類 StageNum：3
    // Start is called before the first frame update
    void Start()
    {
        #region 基本設定
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
        #endregion
        WriteInRecord = "";
        //測驗
        if (Save_BasicInfo.Instance.isTestMode)
        {
            #region 設定題目
            if (Save_BasicInfo.Instance.StageNum == "2")
            {
                Test_TitleText.text = "單元二 一般動物的構造與運動方式";
                //器官配對
                if (Save_BasicInfo.Instance.Stage2_OrganQuiz.Count < 1)
                {
                    Save_BasicInfo.Instance.Stage2_OrganQuiz.Clear();
                    string[] Quizs = Save_BasicInfo.Instance.Stage2_Organ_AllQuiz.Split('@');
                    for (int i = 0; i < Quizs.Length; i++)
                    {
                        Save_BasicInfo.Instance.Stage2_OrganQuiz.Add(Quizs[i]);
                    }
                }
                //運動
                if (Save_BasicInfo.Instance.Stage2_ExerciseQuiz.Count < 1)
                {
                    Save_BasicInfo.Instance.Stage2_ExerciseQuiz.Clear();
                    string[] Quizs = Save_BasicInfo.Instance.Stage2_Exercise_AllQuiz.Split('@');
                    for (int i = 0; i < Quizs.Length; i++)
                    {
                        Save_BasicInfo.Instance.Stage2_ExerciseQuiz.Add(Quizs[i]);
                    }
                }
            }
            else
            {
                Test_TitleText.text = "單元三 水中動物的構造與運動方式";
                //器官配對
                if (Save_BasicInfo.Instance.Stage3_OrganQuiz.Count < 1)
                {
                    Save_BasicInfo.Instance.Stage3_OrganQuiz.Clear();
                    string[] Quizs = Save_BasicInfo.Instance.Stage3_Organ_AllQuiz.Split('@');
                    for (int i = 0; i < Quizs.Length; i++)
                    {
                        Save_BasicInfo.Instance.Stage3_OrganQuiz.Add(Quizs[i]);
                    }
                }
                //運動
                if (Save_BasicInfo.Instance.Stage3_ExerciseQuiz.Count < 1)
                {
                    Save_BasicInfo.Instance.Stage3_ExerciseQuiz.Clear();
                    string[] Quizs = Save_BasicInfo.Instance.Stage3_Exercise_AllQuiz.Split('@');
                    for (int i = 0; i < Quizs.Length; i++)
                    {
                        Save_BasicInfo.Instance.Stage3_ExerciseQuiz.Add(Quizs[i]);
                    }
                }
            }
            #endregion
            TutorialModePage.SetActive(false);
            TestModePage.SetActive(true);
            //下載配分資料
            StartCoroutine(ReadScoreData());
        }
        //教學
        else
        {          
            if (Save_BasicInfo.Instance.StageNum == "2")
            {
                Tu_TitleText.text = "單元二 一般動物的構造與運動方式";
                BirdCreateSpots = new Transform[BirdCreatePlace.childCount];
                for (int i = 0; i < BirdCreatePlace.childCount; i++)
                {
                    BirdCreateSpots[i] = BirdCreatePlace.GetChild(i);
                }
                for (int i = 0; i < TutorialRecords_2.Length; i++)
                {
                    TutorialRecords_2[i] = "false";
                }
            }
            else
            {
                Tu_TitleText.text = "單元三 水中動物的構造與運動方式";
                CrabCreateSpots = new Transform[CrabCreatePlace.childCount];
                for (int i = 0; i < CrabCreatePlace.childCount; i++)
                {
                    CrabCreateSpots[i] = CrabCreatePlace.GetChild(i);
                }
                ShellCreateSpots = new Transform[ShellCreatePlace.childCount];
                for (int i = 0; i < ShellCreatePlace.childCount; i++)
                {
                    ShellCreateSpots[i] = ShellCreatePlace.GetChild(i);
                }
                for (int i = 0; i < TutorialRecords_3.Length; i++)
                {
                    TutorialRecords_3[i] = "false";
                }
            }
            TutorialModePage.SetActive(true);
            TestModePage.SetActive(false);
        }
        GameObject.Find("BlackCanvas").GetComponent<Animator>().Play("黑幕_淡出", 0, 0);
        Invoke("BlackDisAppear", 1f);
    }

    void BlackDisAppear()
    {
        GameObject.Find("BlackCanvas").gameObject.SetActive(false);
        if (Save_BasicInfo.Instance.isTestMode)
        {
            //語音_測驗_開頭標題
            SoundSource.Stop();
            SoundSource.clip = Test_TitleSound;
            SoundSource.Play();
        }
        else
        {
            //語音_開頭標題
            SoundSource.Stop();
            SoundSource.clip = Tu_TitleSound;
            SoundSource.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        //測驗計時
        if (isTesting)
        {
            Timer += Time.deltaTime;
        }
        //確認運動影片狀態
        if (!Save_BasicInfo.Instance.isTestMode)
        {
            if (isPlayVideo)
            {
                if (Videoplay.videoPlayer.isPlaying)
                {
                    ReplayButton.SetActive(false);
                }
                else
                {
                    ReplayButton.SetActive(true);
                }
            }
        }
    }

    #region 聲音
    public void Click()
    {
        EffectSource.Stop();
        SoundSource.Stop();
        EffectSource.clip = ClickSound;
        EffectSource.Play();
    }
    public void TestSound()
    {
        EffectSource.Stop();
        SoundSource.Stop();
        EffectSource.clip = RightSound;
        EffectSource.Play();
    }
    #endregion

    #region 設定
    public void OpenSettingPage()
    {
        //設定音量
        if (Save_BasicInfo.Instance.BGMOff)
        {
            BGMOFF.isOn = true;
        }
        if (Save_BasicInfo.Instance.EffectOff)
        {
            EffectOFF.isOn = true;
        }
        if (Save_BasicInfo.Instance.isTestMode)
        {

        }
        else
        {
            if (isPlay)
            {
                SettingPage.transform.position = Setting_TuChangePlace.position;
            }
            else
            {
                SettingPage.transform.position = SettingOriPlace.position;
            }
        }
        SettingPage.SetActive(true);
    }
    public void SettingSound()
    {
        if (BGMOFF.isOn)
        {
            Save_BasicInfo.Instance.BGMOff = true;
            BGMSource.volume = 0;
        }
        else
        {
            Save_BasicInfo.Instance.BGMOff = false;
            BGMSource.volume = 1;
        }
        if (EffectOFF.isOn)
        {
            Save_BasicInfo.Instance.EffectOff = true;
            EffectSource.volume = 0;
        }
        else
        {
            Save_BasicInfo.Instance.EffectOff = false;
            EffectSource.volume = 1;
        }
        SettingPage.SetActive(false);
    }
    #endregion

    public void HomeBtn()
    {
        SoundSource.Stop();
        if (Save_BasicInfo.Instance.isTestMode)
        {
            if (isTesting)
            {
                if (TestType == 0)
                {
                    Test_EndConPage.transform.position = EndPageChangePlace.position;
                }
                else
                {
                    Test_EndConPage.transform.position = EndPageOriPlace.position;
                }
                Test_EndConPage.SetActive(true);
            }
            else
            {
                if (TestFin)
                {
                    Test_EndFinPage.SetActive(true);
                }
                else
                {
                    Test_EndConPage.transform.position = EndPageOriPlace.position;
                    Test_EndConPage.SetActive(true);
                }

            }
        }
        else
        {
            if (isPlay)
            {
                Tu_EndConPage.transform.position = EndPageChangePlace.position;
            }
            else
            {
                Tu_EndConPage.transform.position = EndPageOriPlace.position;
            }
            Tu_EndConPage.SetActive(true);
        }
    }

    #region 教學
    #region 介面按鈕功能
    public void Tu_TitlePageBtn()
    {
        SoundSource.Stop();
        Save_BasicInfo.Instance.StartTime = DateTime.Now.ToString();
        Tu_SetAnimalPlace();
        BasicBtns_Ori.SetActive(false);
        BasicBtns_Change.SetActive(true);
    }

    public void Tu_BackMenuConBtn()
    {
        if (Tu_TagAnimalPlace.childCount != 0)
        {
            for (int i = 0; i < Tu_TagAnimalPlace.childCount; i++)
            {
                Destroy(Tu_TagAnimalPlace.GetChild(i).gameObject);
            }
        }
        LoadingPage.SetActive(true);
        //紀錄資料
        Save_BasicInfo.Instance.EndTime = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
        string Ans = "";
        if (Save_BasicInfo.Instance.StageNum == "2")
        {
            //整理寫入資訊
            for (int i = 0; i < TutorialRecords_2.Length - 1; i++)
            {
                Ans += TutorialRecords_2[i] + "@";
            }
            Ans += TutorialRecords_2[4];
        }
        else
        {
            //整理寫入資訊
            for (int i = 0; i < TutorialRecords_3.Length - 1; i++)
            {
                Ans += TutorialRecords_3[i] + "@";
            }
            Ans += TutorialRecords_3[7];
        }
        WriteInRecord = Save_BasicInfo.Instance.SchoolName + "@" + Save_BasicInfo.Instance.ClassName + "@" + Save_BasicInfo.Instance.StuNum + "@" + Save_BasicInfo.Instance.Name + "@0@" + Save_BasicInfo.Instance.StartTime + "@" + Save_BasicInfo.Instance.EndTime + "@" + Ans;
        StartCoroutine(WriteStudentData());
    }
    #endregion

    #region 生成動物
    public void Tu_SetAnimalPlace()
    {
        if (Save_BasicInfo.Instance.StageNum == "2")
        {
            //移除
            List<Transform> BirdSpotList= new List<Transform>();
            for (int i = 0; i < BirdCreateSpots.Length; i++)
            {
                BirdSpotList.Add(BirdCreateSpots[i]);
                if (BirdCreateSpots[i].childCount != 0)
                {
                    for (int k = 0; k < BirdCreateSpots[i].childCount; k++)
                    {
                        Destroy(BirdCreateSpots[i].GetChild(k).gameObject);
                    }
                }
            }
            //新增：先確認每隻都有出現
            for (int i = 0; i < CreateBirdPrefabs.Length; i++)
            {
                int Num = UnityEngine.Random.Range(0, BirdSpotList.Count);
                Instantiate(CreateBirdPrefabs[i], BirdSpotList[Num].position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0f, 360f),0)), BirdSpotList[Num]);
                BirdSpotList.Remove(BirdSpotList[Num]);
            }
            //新增：隨機的動物
            for (int i = 0; i < BirdSpotList.Count; i++)
            {
                int Num = UnityEngine.Random.Range(0, CreateBirdPrefabs.Length);
                Instantiate(CreateBirdPrefabs[Num], BirdSpotList[i].position, BirdSpotList[i].rotation, BirdSpotList[i]);
            }

        }
        else
        {
            //移除
            List<Transform> CrabSpotList = new List<Transform>();
            List<Transform> ShellSpotList = new List<Transform>();
            for (int i = 0; i < CrabCreateSpots.Length; i++)
            {
                CrabSpotList.Add(CrabCreateSpots[i]);
                if (CrabCreateSpots[i].childCount != 0)
                {
                    for (int k = 0; k < CrabCreateSpots[i].childCount; k++)
                    {
                        Destroy(CrabCreateSpots[i].GetChild(k).gameObject);
                    }
                }
            }
            for (int i = 0; i < ShellCreateSpots.Length; i++)
            {
                ShellSpotList.Add(ShellCreateSpots[i]);
                if (ShellCreateSpots[i].childCount != 0)
                {
                    for (int k = 0; k < ShellCreateSpots[i].childCount; k++)
                    {
                        Destroy(ShellCreateSpots[i].GetChild(k).gameObject);
                    }
                }
            }
            //新增：先確認每隻都有出現
            for (int i = 0; i < CreateCrabPrefabs.Length; i++)
            {
                int Num = UnityEngine.Random.Range(0, CrabSpotList.Count);
                Instantiate(CreateCrabPrefabs[i], CrabSpotList[Num].position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0)), CrabSpotList[Num]);
                CrabSpotList.Remove(CrabSpotList[Num]);
            }
            for (int i = 0; i < CreateShellPrefabs.Length; i++)
            {
                int Num = UnityEngine.Random.Range(0, ShellSpotList.Count);
                Instantiate(CreateShellPrefabs[i], ShellSpotList[Num].position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0f, 360f), 0)), ShellSpotList[Num]);
                ShellSpotList.Remove(ShellSpotList[Num]);
            }
            //新增：隨機的動物
            for (int i = 0; i < CrabSpotList.Count; i++)
            {
                int Num = UnityEngine.Random.Range(0, CreateCrabPrefabs.Length);
                Instantiate(CreateCrabPrefabs[Num], CrabSpotList[i].position, CrabSpotList[i].rotation, CrabSpotList[i]);
            }
            for (int i = 0; i < ShellSpotList.Count; i++)
            {
                int Num = UnityEngine.Random.Range(0, CreateShellPrefabs.Length);
                Instantiate(CreateShellPrefabs[Num], ShellSpotList[i].position, ShellSpotList[i].rotation, ShellSpotList[i]);
            }
        }
        //頁面
        Change_HomeButton.SetActive(true);
        Tu_TiltePage.SetActive(false);
        Tu_GamPage.SetActive(false);
    }
    #endregion

    #region 選擇動物
    //點擊動物
    public void Tu_SelectAnimal()
    {
        Click();
        if (Save_BasicInfo.Instance.StageNum == "2")
        {
            BirdClicks.Clear();
            for (int i = 0; i < BirdCreateSpots.Length; i++)
            {
                BirdClicks.Add(BirdCreateSpots[i].GetChild(0).GetComponent<Stage2_3Click>());
            }
            for (int i = 0; i < BirdClicks.Count; i++)
            {
                if (BirdClicks[i].isClick)
                {
                    AnimalNum = BirdClicks[i].Num;
                    Tu_SetGamePage();
                }
            }
        }
        else 
        {
            SeaAnimalClicks.Clear();
            for (int i = 0; i < CrabCreateSpots.Length; i++)
            {
                SeaAnimalClicks.Add(CrabCreateSpots[i].GetChild(0).GetComponent<Stage2_3Click>());
            }
            for (int i = 0; i < ShellCreateSpots.Length; i++)
            {
                SeaAnimalClicks.Add(ShellCreateSpots[i].GetChild(0).GetComponent<Stage2_3Click>());
            }
            for (int i = 0; i < SeaAnimalClicks.Count; i++)
            {
                if (SeaAnimalClicks[i].isClick)
                {
                    AnimalNum = SeaAnimalClicks[i].Num;
                    Tu_SetGamePage();
                }
            }
        }
    }
    //處理觀察頁面
    public void Tu_SetGamePage()
    {
        //生成作答
        if (Save_BasicInfo.Instance.StageNum == "2")
        {
            if (Tu_TagPlace.childCount != 0)
            {
                for (int i = 0; i < Tu_TagPlace.childCount; i++)
                {
                    Destroy(Tu_TagPlace.GetChild(i).gameObject);
                }
            }
            if (Tu_TagAnimalPlace.childCount != 0)
            {
                for (int i = 0; i < Tu_TagAnimalPlace.childCount; i++)
                {
                    Destroy(Tu_TagAnimalPlace.GetChild(i).gameObject);
                }
            }
            Instantiate(Tu_TagPrefabs_2[AnimalNum], Tu_TagPlace.position, Tu_TagPlace.rotation, Tu_TagPlace);
            Instantiate(Tu_TagAnimPrefabs_2[AnimalNum], Tu_TagAnimalPlace.position, Tu_TagAnimalPlace.rotation, Tu_TagAnimalPlace);
            //鎖定其他場上動物
            for (int i = 0; i < BirdClicks.Count; i++)
            {
                BirdClicks[i].GetComponent<Collider>().enabled = false;
            }
        }
        else
        {
            if (Tu_TagPlace.childCount != 0)
            {
                for (int i = 0; i < Tu_TagPlace.childCount; i++)
                {
                    Destroy(Tu_TagPlace.GetChild(i).gameObject);
                }
            }
            if (Tu_TagAnimalPlace.childCount != 0)
            {
                for (int i = 0; i < Tu_TagAnimalPlace.childCount; i++)
                {
                    Destroy(Tu_TagAnimalPlace.GetChild(i).gameObject);
                }
            }
            Instantiate(Tu_TagPrefabs_3[AnimalNum], Tu_TagPlace.position, Tu_TagPlace.rotation, Tu_TagPlace);
            Instantiate(Tu_TagAnimPrefabs_3[AnimalNum], Tu_TagAnimalPlace.position, Tu_TagAnimalPlace.rotation, Tu_TagAnimalPlace);
            //鎖定其他場上動物
            for (int i = 0; i < SeaAnimalClicks.Count; i++)
            {
                SeaAnimalClicks[i].GetComponent<Collider>().enabled = false;
            }
        }
        Tu_WatchButton.interactable = true;
        Tu_ExerciseButton.gameObject.SetActive(false);
        Tu_BackBtns.SetActive(false);
        Change_HomeButton.SetActive(false);
        VideoObject.SetActive(false);
        Tu_GamPage.SetActive(true);
        //語音_遊戲說明
        SoundSource.Stop();
        SoundSource.clip = Tu_DescriptionSound;
        SoundSource.Play();
        isPlay = true;
    }
    #endregion

    #region 遊戲功能

    public void Tu_OtherAnimalBtn()
    {
        isPlay = false;
        isPlayVideo = false;
        Tu_SetAnimalPlace();
    }

    #region 確認觀察的答案
    public void Tu_WatchBtn()
    {
        bool isWrong = false;
        for (int i = 0; i < Tu_TagPlace.GetChild(0).GetChild(1).childCount; i++)
        {
            string AnsSpot = Tu_TagPlace.GetChild(0).GetChild(1).GetChild(i).gameObject.name;
            if (Tu_TagPlace.GetChild(0).GetChild(1).GetChild(i).childCount > 0)
            {
                if (!Tu_TagPlace.GetChild(0).GetChild(1).GetChild(i).GetChild(0).gameObject.name.Contains(AnsSpot))
                {
                    isWrong = true;
                    break;
                }
            }
            else
            {
                isWrong = true;
                break;
            }
        }
        if (isWrong)
        {
            SoundSource.Stop();
            EffectSource.clip = WrongSound;
            EffectSource.Play();
        }
        else
        {
            if (Tu_TagPlace.childCount != 0)
            {
                for (int i = 0; i < Tu_TagPlace.childCount; i++)
                {
                    Destroy(Tu_TagPlace.GetChild(i).gameObject);
                }
            }
            Tu_BackBtns.SetActive(true);
            Tu_ExerciseButton.gameObject.SetActive(true);
            Tu_WatchButton.interactable = false;
            EffectSource.clip = RightSound;
            EffectSource.Play();
            //語音_恭喜觀察過
            SoundSource.Stop();
            SoundSource.clip = Tu_Fin1Sound;
            SoundSource.Play();
        }   
    }
    #endregion

    public void Tu_ExerciseBtn()
    {
        if (Tu_TagPlace.childCount != 0)
        {
            for (int i = 0; i < Tu_TagPlace.childCount; i++)
            {
                Destroy(Tu_TagPlace.GetChild(i).gameObject);
            }
        }
        //播放運動動畫
        ReplayButton.SetActive(false);
        VideoObject.SetActive(true);
        Videoplay.PlayVideo(AnimalNum);
        isPlayVideo = true;
        Invoke("Tu_ExerciseFin", 10f);
    }
    public void RePlayExerciseBtn()
    {
        Videoplay.PlayVideo(AnimalNum);
        ReplayButton.SetActive(false);
    }
    
    public void Tu_ExerciseFin()
    {
        ReplayButton.SetActive(true);
        if (Save_BasicInfo.Instance.StageNum == "2")
        {
            TutorialRecords_2[AnimalNum] = "true";
        }
        else
        {
            TutorialRecords_3[AnimalNum] = "true";
        }
        EffectSource.clip = RightSound;
        EffectSource.Play();
        //語音_恭喜收集完畢
        SoundSource.Stop();
        SoundSource.clip = Tu_Fin2Sound;
        SoundSource.Play();
    }

    #endregion
    #endregion

    #region 測驗
    #region 介面按鈕功能
    public void Test_ReStartBtn()
    {
        SceneManager.LoadScene(3);
    }
    public void Test_ToTutorialBtn()
    {
        Save_BasicInfo.Instance.isTestMode = false;
        SceneManager.LoadScene(3);
    }
    public void Test_BackMenuConBtn()
    {
        SceneManager.LoadScene(1);
    }
    #endregion

    public void Test_TitleBtn()
    {
        Save_BasicInfo.Instance.StartTime = DateTime.Now.ToString();
        TestMenu.ResetStage_Organ();
        Test_OrganPage.SetActive(true);
        //語音_測驗_器官
        SoundSource.Stop();
        SoundSource.clip = Test_HintSound[0];
        SoundSource.Play();
        //第一種題型
        Test_OrganPage.SetActive(true);
        TestTitlePage.SetActive(false);
        BasicBtns_Ori.SetActive(false);
        BasicBtns_Change.SetActive(true);
        isTesting = true;
    }

    public void CheckTestFin()
    {
        //重新計時
        Timer = 0;

        //結束
        if (TestQuizNum == 5)
        {
            TestFin = true;

            LoadingPage.SetActive(true);
            Test_OrganPage.SetActive(false);
            #region 設定儲存的資料
            Save_BasicInfo.Instance.EndTime = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
            string Ans = "";
            #region 整理寫入資訊
            int AllTime = 0;
            for (int i = 0; i < TestTimer.Length; i++)
            {
                int Time = int.Parse(TestTimer[i]);
                AllTime += Time;
            }
            //處理分數
            int SplitStage = Test_CountScore.Length / 6;
            int[] UnitScore = new int[SplitStage];
            for (int i = 0; i < SplitStage; i++)
            {
                if (Save_BasicInfo.Instance.StageNum == "2")
                {
                    UnitScore[i] = int.Parse(Test_CountScore[i+5]);
                }
                else
                {
                    UnitScore[i] = int.Parse(Test_CountScore[i+10]);
                }
            }
            int MinusScore = 0;
            for (int i = 0; i < TestRecords.Length; i++)
            {
                string[] AnsString = TestRecords[i].Split('_');
                if (AnsString[1] != "正確")
                {
                    MinusScore += UnitScore[i];
                }
            }
            TestScore = 100 - MinusScore;
            if (TestScore < 60)
            {
                Test_ScoreResult = "N0_" + TestScore.ToString();
            }
            else
            {
                Test_ScoreResult = "Pass_" + TestScore.ToString();
            }
            #endregion
            //串資訊
            Ans += AllTime.ToString() + "@" + Test_ScoreResult + "@" + TestRecords.Length.ToString() + "@";
            for (int i = 0; i < TestTimer.Length; i++)
            {
                Ans += TestTimer[i] + "@";
            }
            for (int i = 0; i < TestRecords.Length - 1; i++)
            {
                Ans += TestRecords[i] + "@";
            }
            Ans += TestRecords[TestRecords.Length - 1];
            #endregion
            WriteInRecord = Save_BasicInfo.Instance.SchoolName + "@" + Save_BasicInfo.Instance.ClassName + "@" + Save_BasicInfo.Instance.StuNum + "@" + Save_BasicInfo.Instance.Name + "@1@" + Save_BasicInfo.Instance.StartTime + "@" + Save_BasicInfo.Instance.EndTime + "@" + Ans;
            //寫入資料
            StartCoroutine(WriteStudentData());
        }
        else
        {
            Test_OrganPage.SetActive(false);
            Test_ExercisePage.SetActive(false);
            //固定第二種題型
            if (TestQuizNum == 2)
            {
                TestMenu.ResetStage_Exercise();
                Test_ExercisePage.SetActive(true);
                BasicBtns_Ori.SetActive(true);
                BasicBtns_Change.SetActive(false);
                //語音_測驗_運動
                SoundSource.Stop();
                SoundSource.clip = Test_HintSound[1];
                SoundSource.Play();
            }
            //隨機
            else
            {
                TestType = UnityEngine.Random.Range(0, 2);
                if (TestType == 0)
                {
                    TestMenu.ResetStage_Organ();
                    BasicBtns_Ori.SetActive(false);
                    BasicBtns_Change.SetActive(true);
                    Test_OrganPage.SetActive(true);
                    //語音_測驗_器官
                    SoundSource.Stop();
                    SoundSource.clip = Test_HintSound[0];
                    SoundSource.Play();
                }
                else
                {
                    TestMenu.ResetStage_Exercise();
                    BasicBtns_Ori.SetActive(true);
                    BasicBtns_Change.SetActive(false);
                    Test_ExercisePage.SetActive(true);
                    //語音_測驗_運動
                    SoundSource.Stop();
                    SoundSource.clip = Test_HintSound[1];
                    SoundSource.Play();
                }
            }
            isTesting = true;
        }

    }

    #endregion

    #region 寫入資料
    #region 寫入教學資料
    IEnumerator WriteStudentData()
    {
        WWWForm form = new WWWForm();
        //辨識傳輸方式
        form.AddField("method", "write");
        //輸入要寫入的工作表號碼，從0開始到6，共7個單元，測驗及教學的資料在同一個單元的工作表內
        form.AddField("Num", Save_BasicInfo.Instance.StageNum);
        //改成寫一欄，資訊用@隔開，分數用_隔開
        form.AddField("Data", WriteInRecord);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbwcON_NqaxuQIL0zSgex0fOr3rOiVjQFhVt1LYmxQQnU3Tm8KK3kZHESgGJ3AjuXGc_yg/exec", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (Save_BasicInfo.Instance.isTestMode)
                {
                    //讀取所有人資料
                    StartCoroutine(ReadStudentData());
                }
                else
                {
                    //上傳APP資料
                    StartCoroutine(WriteAPPData());
                }
                Debug.Log("Upload Complete!");
            }
        }
    }
    #endregion
    #region 寫入APP資料
    IEnumerator WriteAPPData()
    {
        WWWForm form = new WWWForm();
        //辨識傳輸方式
        form.AddField("method", "WriteData");
        //輸入要寫入的工作表號碼，APP讀取資料為8
        form.AddField("Num", "9");
        //要寫入的行數
        form.AddField("SelectRow", Save_BasicInfo.Instance.APPDataNum);
        #region 計算有收集的動物
        string data = "";
        //單元答案的欄數：從2開始，3、5、7、9、11、13
        if (Save_BasicInfo.Instance.StageNum == "2")
        {
            form.AddField("ColumnNum", "5");
            for (int i = 0; i < TutorialRecords_2.Length - 1; i++)
            {
                if (TutorialRecords_2[i] == "true")
                {
                    data += "1@";
                }
                else
                {
                    data += "0@";
                }
            }
            if (TutorialRecords_2[TutorialRecords_2.Length - 1] == "true")
            {
                data += "1";
            }
            else
            {
                data += "0";
            }
        }
        else
        {
            form.AddField("ColumnNum", "7");
            for (int i = 0; i < TutorialRecords_3.Length - 1; i++)
            {
                if (TutorialRecords_3[i] == "true")
                {
                    data += "1@";
                }
                else
                {
                    data += "0@";
                }
            }
            if (TutorialRecords_3[TutorialRecords_3.Length - 1] == "true")
            {
                data += "1";
            }
            else
            {
                data += "0";
            }
        }
        #endregion
        form.AddField("Data", data);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbwcON_NqaxuQIL0zSgex0fOr3rOiVjQFhVt1LYmxQQnU3Tm8KK3kZHESgGJ3AjuXGc_yg/exec", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                SceneManager.LoadScene(1);
                Debug.Log("Upload Complete!");
            }
        }
    }
    #endregion
    #endregion

    #region 讀取資料
    #region 學生紀錄
    IEnumerator ReadStudentData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //輸入要讀取的工作表號碼，從0開始，測驗及教學的資料在同一個單元的工作表內
        form_Read.AddField("Num", Save_BasicInfo.Instance.StageNum);
        //學校名稱的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbwcON_NqaxuQIL0zSgex0fOr3rOiVjQFhVt1LYmxQQnU3Tm8KK3kZHESgGJ3AjuXGc_yg/exec", form_Read))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            // if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //下載的資料
                StuData = www.downloadHandler.text.Split(',');
                TestData.Clear();
                for (int i = 0; i < StuData.Length; i++)
                {
                    string[] Splilt = StuData[i].Split('@');
                    if (Splilt[4] == "1")
                    {
                        TestData.Add(StuData[i]);
                    }
                }
                Test_SetResultPage();
                Debug.Log("Download School!");
            }
        }
    }
    public void Test_SetResultPage()
    {
        #region 排列成績
        List<int> Score = new List<int>();
        for (int i = 0; i < TestData.Count; i++)
        {
            string[] Split = TestData[i].Split('@');
            string[] SpiltScore = Split[8].Split('_');
            Score.Add(int.Parse(SpiltScore[1]));
        }
        //排列分數(由高到低)
        Score = Score.OrderByDescending(number => number).ToList();
        string[] OrderedScore = new string[0];
        if (Score.Count > 10)
        {
            OrderedScore = new string[10];
        }
        else
        {
            OrderedScore = new string[Score.Count];
        }
        for (int i = 0; i < OrderedScore.Length; i++)
        {
            OrderedScore[i] = Score[i].ToString();
        }
        //排列資訊
        Test_SortData.Clear();
        for (int i = 0; i < OrderedScore.Length; i++)
        {
            for (int j = 0; j < TestData.Count; j++)
            {
                string[] Split = TestData[j].Split('@');
                string[] SplitScore = Split[8].Split('_');
                if (SplitScore[1] == OrderedScore[i])
                {
                    Test_SortData.Add(TestData[j]);
                    TestData.Remove(TestData[j]);
                }
            }
        }
        #endregion
        #region 設定結束頁面
        //排行榜
        if (Test_ScoreTextPlace.childCount != 0)
        {
            for (int i = 0; i < Test_ScoreTextPlace.childCount; i++)
            {
                Destroy(Test_ScoreTextPlace.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < Test_SortData.Count; i++)
        {
            Instantiate(Test_ScoreItemPrefab, Test_ScoreTextPlace.position, Test_ScoreTextPlace.rotation, Test_ScoreTextPlace);
        }
        for (int i = 0; i < Test_SortData.Count; i++)
        {
            string[] Spilt = Test_SortData[i].Split('@');
            string[] SpiltScore = Spilt[8].Split('_');
            Test_ScoreTextPlace.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString() + ".";
            Test_ScoreTextPlace.GetChild(i).GetChild(1).GetComponent<Text>().text = Spilt[3];
            Test_ScoreTextPlace.GetChild(i).GetChild(2).GetComponent<Text>().text = SpiltScore[1];
            if (int.Parse(SpiltScore[1]) < 60)
            {
                Test_ScoreTextPlace.GetChild(i).GetChild(2).GetComponent<Text>().color = Color.red;
            }
        }
        //錯誤題目
        if (Test_QuizTextPlace.childCount != 0)
        {
            for (int i = 0; i < Test_QuizTextPlace.childCount; i++)
            {
                Destroy(Test_QuizTextPlace.GetChild(i).gameObject);
            }
        }
        List<string> WrongText = new List<string>();
        for (int i = 0; i < TestRecords.Length; i++)
        {
            string[] Split = TestRecords[i].Split('_');
            if (Split[1] != "正確")
            {
                Instantiate(Test_QuizTextPrefab, Test_QuizTextPlace.position, Test_QuizTextPlace.rotation, Test_QuizTextPlace);
                WrongText.Add(TestRecords[i]);
            }
        }
        for (int i = 0; i < Test_QuizTextPlace.childCount; i++)
        {
            string[] Split = WrongText[i].Split('_');
            Test_QuizTextPlace.GetChild(i).GetComponent<Text>().text = Split[0];
        }
        if (Save_BasicInfo.Instance.StageNum == "2")
        {
            Test_ToTuBtnText.text = "進行單元二教學";
        }
        else
        {
            Test_ToTuBtnText.text = "進行單元三教學";
        }
        Test_ExercisePage.SetActive(false);
        BasicBtns_Ori.SetActive(true);
        BasicBtns_Change.SetActive(false);
        TestResultPage.SetActive(true);
        LoadingPage.SetActive(false);
        //語音_結束測驗
        SoundSource.Stop();
        EffectSource.clip = Test_FinSound;
        EffectSource.Play();
        #endregion
    }
    #endregion

    #region 配分資料
    //讀取配分
    IEnumerator ReadScoreData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //測驗配分的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbzF7d7qybA3mTw2PkZiAQpBb9EqSaQua88RoRGXmutddpw1PC7LfbMvBV0QIjUtCYh8/exec", form_Read))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            // if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //下載的資料
                Test_CountScore = www.downloadHandler.text.Split(',');
                Debug.Log("Download Score!");
            }
        }
    }
    #endregion
    #endregion
}
