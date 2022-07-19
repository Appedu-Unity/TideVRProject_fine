using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Stage1Menu : MonoBehaviour
{
    [Header("聲音調整")] public AudioSource EffectSource;
    public AudioSource BGMSource, SoundSource;
    [Header("狀態音效")] public AudioClip ClickSound, RightSound, WrongSound;
    [Header("語音語音")] private bool isPlayed;
    //教學
    [Header("標題說明")] public AudioClip Tu_TitleSound;
    [Header("遊戲說明")] public AudioClip Tu_GameSound;
    [Header("完成觀察")] public AudioClip Tu_FinSound;
    [Header("生物觀察標題敘述")] public AudioClip[] Tu_SubTitleSound = new AudioClip[3];
    //測驗
    [Header("測試開始標題")] public AudioClip Test_TitleSound;
    [Header("完成測驗")] public AudioClip Test_FinSound;
    [Header("VR測驗教學說明")] public AudioClip[] Test_HintSound = new AudioClip[2];
    //看OFF的Toggle
    [Header("設定音效")] public Toggle BGMOFF, EffectOFF;
    [Header("過場動畫")] public GameObject LoadingPage;
    public string WriteInRecord;
    [Header("設定UI")] public GameObject SettingPage;
    public Transform SettingOriPlace, Setting_SkinPlace;
    [Header("教學")] public GameObject TutorialModePage;
    [Header("頁面")] public GameObject Tu_TitlePage;
    public GameObject Tu_ModePage, Tu_SubTitlePage, Tu_AnimalPage, Tu_GamePage, Tu_EndConPage;
    [Header("返回首頁按鈕")] public GameObject HomeButton;
    [Header("設定按鈕")] public GameObject SettingButton;
    public GameObject[] Tu_AnimalPages;
    [Header("切換頁面")] public int ModeNum;
    public int AnimalNum;
    public Text Tu_SubTitleText;
    public string[] Tu_SubTitleStrings = new string[3];
    public Image Tu_SubTitlePic;
    public Sprite[] Tu_SubTitleSprites = new Sprite[3];
    [Header("切換物種")] public Transform AnimalPlace;
    public ObjectRotate AnimalRotateSet;
    public GameObject[] AnimalPrefabs = new GameObject[13];
    //僅鳥類
    public GameObject Tu_SoundButton, Tu_OtherAnimalButton;
    [Header("觀察功能")] public bool isGaming;
    //文字說明
    public GameObject Tu_DescriptionPage;
    public Text Tu_DescriptionText;
    public string[] Tu_DescriptionStrings = new string[13];
    public AudioClip[] Tu_DescriptionSound = new AudioClip[13];
    [Header("放大設定")] public Toggle Tu_BigButton;
    public GameObject Tu_SetScalePage;
    //聲音(鳥類)
    public AudioClip[] Tu_BirdSounds = new AudioClip[5];
    public Button Tu_PhotoButton;
    public GameObject Tu_PhotoAnimalPage;
    [Header("紀錄")] public string[] TutorialRecords = new string[13];
    public string[] TestRecords = new string[5];
    [Header("測驗")]
    public bool TestFin;
    public GameObject TestModePage;
    public GameObject TestTitlePage, Test_SkinPage, Test_ShadowPage, Test_SoundPage, TestResultPage, Test_EndConPage, Test_EndFinPage, Test_EndConPage_Skin;
    public int TestQuizNum;
    //0：花色、1：剪影、2：叫聲
    public int TestType;
    public string[] StuData;
    [Header("測驗種類")]
    public Stage1_SkinTest SkinStage;
    public Stage1_Shadow_SoundTest ShadowSoundTest;
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
        #region 切換模式
        if (Save_BasicInfo.Instance.isTestMode)
        {
            TutorialModePage.SetActive(false);
            TestModePage.SetActive(true);
            #region 設定題目
            //花色
            if (Save_BasicInfo.Instance.Stage1_SkinQuiz.Count < 1)
            {
                Save_BasicInfo.Instance.Stage1_SkinQuiz.Clear();
                string[] Quizs = Save_BasicInfo.Instance.Stage1_Skin_AllQuiz.Split('@');
                for (int i = 0; i < Quizs.Length; i++)
                {
                    Save_BasicInfo.Instance.Stage1_SkinQuiz.Add(Quizs[i]);
                }
            }
            //剪影
            if (Save_BasicInfo.Instance.Stage1_ShadowQuiz.Count < 1)
            {
                Save_BasicInfo.Instance.Stage1_ShadowQuiz.Clear();
                string[] Quizs = Save_BasicInfo.Instance.Stage1_Shadow_AllQuiz.Split('@');
                for (int i = 0; i < Quizs.Length; i++)
                {
                    Save_BasicInfo.Instance.Stage1_ShadowQuiz.Add(Quizs[i]);
                }
            }
            //叫聲
            if (Save_BasicInfo.Instance.Stage1_SoundQuiz.Count < 1)
            {
                Save_BasicInfo.Instance.Stage1_SoundQuiz.Clear();
                string[] Quizs = Save_BasicInfo.Instance.Stage1_Sound_AllQuiz.Split('@');
                for (int i = 0; i < Quizs.Length; i++)
                {
                    Save_BasicInfo.Instance.Stage1_SoundQuiz.Add(Quizs[i]);
                }
            }
            #endregion
            //下載配分資料
            StartCoroutine(ReadScoreData());
        }
        else
        {
            for (int i = 0; i < TutorialRecords.Length; i++)
            {
                TutorialRecords[i] = "false";
            }
            TutorialModePage.SetActive(true);
            TestModePage.SetActive(false);
        }
        #endregion
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

            if (TestType == 0)
            {
                if (isTesting)
                {
                    SettingPage.transform.position = Setting_SkinPlace.position;
                }
                else
                {
                    SettingPage.transform.position = SettingOriPlace.position;
                }
            }
            else
            {
                SettingPage.transform.position = SettingOriPlace.position;
            }
        }
        else
        {
            if (isGaming)
            {
                AnimalRotateSet.gameObject.SetActive(false);
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
        AnimalRotateSet.gameObject.SetActive(true);
    }
    #endregion

    public void HomeBtn()
    {
        EffectSource.Stop();
        SoundSource.Stop();
        if (Save_BasicInfo.Instance.isTestMode)
        {
            if (isTesting)
            {
                if (TestType == 0)
                {
                    Test_EndConPage_Skin.SetActive(true);
                }
                else
                {
                    Test_EndConPage.SetActive(true);
                }
            }
            else
            {
                if (TestFin)
                {
                    Test_EndFinPage.SetActive(true);
                }
                else
                {
                    Test_EndConPage.SetActive(true);
                }

            }
        }
        else
        {
            if (isGaming)
            {
                AnimalRotateSet.gameObject.SetActive(false);
            }
            Tu_EndConPage.SetActive(true);
        }
    }
    public void Tu_EndCancelBtn()
    {
        AnimalRotateSet.gameObject.SetActive(true);
    }
    #region 教學模式
    #region 介面按鈕功能
    //結束教學
    public void Tu_BackMenuConBtn()
    {
        if (AnimalPlace.childCount != 0)
        {
            for (int i = 0; i < AnimalPlace.childCount; i++)
            {
                Destroy(AnimalPlace.GetChild(i).gameObject);
            }
        }
        LoadingPage.SetActive(true);
        Save_BasicInfo.Instance.EndTime = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
        string Ans = "";
        //整理寫入資訊
        for (int i = 0; i < TutorialRecords.Length - 1; i++)
        {
            Ans += TutorialRecords[i] + "@";
        }
        Ans += TutorialRecords[12];
        WriteInRecord = Save_BasicInfo.Instance.SchoolName + "@" + Save_BasicInfo.Instance.ClassName + "@" + Save_BasicInfo.Instance.StuNum + "@" + Save_BasicInfo.Instance.Name + "@0@" + Save_BasicInfo.Instance.StartTime + "@" + Save_BasicInfo.Instance.EndTime + "@" + Ans;
        //寫入資料
        StartCoroutine(WriteStudentData());
    }

    public void Tu_TitleBtn()
    {
        //紀錄時間
        Save_BasicInfo.Instance.StartTime = DateTime.Now.ToString();
        Tu_TitlePage.SetActive(false);
        Tu_ModePage.SetActive(true);

    }
    //選擇種類
    public void Tu_ModeBtn(int Num)
    {
        ModeNum = Num;
        Tu_SubTitlePic.sprite = Tu_SubTitleSprites[Num];
        Tu_SubTitleText.text = Tu_SubTitleStrings[Num];
        Tu_SubTitleText.text.Replace("\\n", "\n");
        Tu_ModePage.SetActive(false);
        Tu_SubTitlePage.SetActive(true);
        //語音_物種標題
        SoundSource.Stop();
        SoundSource.clip = Tu_SubTitleSound[Num];
        SoundSource.Play();
    }
    #region 選擇生物
    public void Tu_SubTitleBtn()
    {
        for (int i = 0; i < Tu_AnimalPages.Length; i++)
        {
            for (int k = 0; k < Tu_AnimalPages[i].transform.childCount; k++)
            {
                if (k == 0)
                {
                    Tu_AnimalPages[i].transform.GetChild(k).gameObject.SetActive(true);
                }
                else
                {
                    Tu_AnimalPages[i].transform.GetChild(k).gameObject.SetActive(false);
                }
            }
            if (i == ModeNum)
            {
                Tu_AnimalPages[i].SetActive(true);
            }
            else
            {
                Tu_AnimalPages[i].SetActive(false);
            }
        }
        //切換按鈕
        HomeButton.SetActive(false);
        Tu_SubTitlePage.SetActive(false);
        Tu_AnimalPage.SetActive(true);
    }
    //選生物
    public void Tu_AnimalBtn(int Num)
    {
        //0-12，蟹、螺貝、鳥
        AnimalNum = Num;
        //出現動物
        if (AnimalPlace.childCount != 0)
        {
            for (int i = 0; i < AnimalPlace.childCount; i++)
            {
                Destroy(AnimalPlace.GetChild(i).gameObject);
            }
        }
        AnimalRotateSet.TurnObject = Instantiate(AnimalPrefabs[AnimalNum], AnimalPlace.position, AnimalPlace.rotation, AnimalPlace);
        //鳥類
        if (ModeNum == 2)
        {
            Tu_SoundButton.SetActive(true);
        }
        else
        {
            Tu_SoundButton.SetActive(false);
        }
        Tu_DescriptionPage.SetActive(false);
        Tu_PhotoAnimalPage.SetActive(false);
        Tu_AnimalPage.SetActive(false);
        //換同類Btn的字
        switch (ModeNum)
        {
            case 0:
                Tu_OtherAnimalButton.transform.GetChild(0).GetComponent<Text>().text = "觀察其他蟹類";
                break;
            case 1:
                Tu_OtherAnimalButton.transform.GetChild(0).GetComponent<Text>().text = "觀察其他螺貝類";
                break;
            case 2:
                Tu_OtherAnimalButton.transform.GetChild(0).GetComponent<Text>().text = "觀察其他鳥類";
                break;
        }
        //動物有沒有收集過
        if (TutorialRecords[AnimalNum] == "true")
        {
            Tu_PhotoButton.interactable = false;
        }
        else
        {
            Tu_PhotoButton.interactable = true;
        }
        //放大按鈕設定
        Tu_SetScalePage.SetActive(false);
        Tu_BigButton.isOn = false;
        AnimalPlace.localScale = Vector3.one;
        Tu_SetScalePage.transform.GetChild(0).GetComponent<Button>().interactable = true;
        Tu_SetScalePage.transform.GetChild(1).GetComponent<Button>().interactable = false;
        //
        SettingButton.SetActive(false);
        isGaming = true;
        Tu_GamePage.SetActive(true);
        //語音_說明
        if (!isPlayed)
        {
            SoundSource.Stop();
            SoundSource.clip = Tu_GameSound;
            SoundSource.Play();
            isPlayed = true;
        }
    }
    //觀察其他種類
    public void Tu_OtherTypeBtn()
    {
        isGaming = false;
        HomeButton.SetActive(true);
        Tu_ModePage.SetActive(true);
        SettingButton.SetActive(true);
        Tu_GamePage.SetActive(false);
        Tu_AnimalPage.SetActive(false);
    }
    //觀察其他同種生物
    public void Tu_OtherAnimalBtn()
    {
        isGaming = false;
        for (int i = 0; i < Tu_AnimalPages[ModeNum].transform.childCount; i++)
        {
            if (i == 0)
            {
                Tu_AnimalPages[ModeNum].transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                Tu_AnimalPages[ModeNum].transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        Tu_AnimalPage.SetActive(true);
        SettingButton.SetActive(true);
        Tu_GamePage.SetActive(false);
    }
    #endregion
    #endregion

    #region 觀察功能
    #region 放大
    public void Tu_BigBtn()
    {
        if (Tu_BigButton.isOn)
        {
            Tu_SetScalePage.SetActive(true);
        }
        else
        {
            Tu_SetScalePage.SetActive(false);
        }
    }
    public void Tu_ScaleIncreaseBtn()
    {
        AnimalPlace.localScale += Vector3.one * 0.2f;
        Tu_SetScalePage.transform.GetChild(1).GetComponent<Button>().interactable = true;
        if (AnimalPlace.localScale.x > 1.9f)
        {
            Tu_SetScalePage.transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
    }
    public void Tu_ScaleDecreaseBtn()
    {
        AnimalPlace.localScale -= Vector3.one * 0.2f;
        Tu_SetScalePage.transform.GetChild(0).GetComponent<Button>().interactable = true;
        if (AnimalPlace.localScale.x < 1.1f)
        {
            Tu_SetScalePage.transform.GetChild(1).GetComponent<Button>().interactable = false;
        }
    }
    #endregion
    //解說
    public void Tu_DescriptionBtn()
    {
        Tu_DescriptionText.text = Tu_DescriptionStrings[AnimalNum];
        Tu_DescriptionPage.SetActive(true);
        //語音_解說
    }
    //拍照
    public void Tu_PhotoBtn()
    {
        //單次遊玩一隻動物只記錄一次
        TutorialRecords[AnimalNum] = "true";
        EffectSource.Stop();
        SoundSource.Stop();
        EffectSource.clip = RightSound;
        EffectSource.Play();
        Tu_PhotoButton.interactable = false;
        //Tu_PhotoAnimalPage.SetActive(true);
        Invoke("PhotoPageDisAppear", 0.2f);
    }
    void PhotoPageDisAppear()
    {
        // Tu_PhotoAnimalPage.SetActive(false);
        //語音_觀察完畢
        SoundSource.Stop();
        SoundSource.clip = Tu_FinSound;
        SoundSource.Play();
    }
    //聲音
    public void Tu_SoundBtn()
    {
        //播聲音
    }
    #endregion
    #endregion

    #region 測驗模式
    #region 功能按鈕
    public void Test_ReStartBtn()
    {
        SceneManager.LoadScene(2);
    }
    public void Test_ToTutorialBtn()
    {
        Save_BasicInfo.Instance.isTestMode = false;
        SceneManager.LoadScene(2);
    }
    public void Test_BackMenuConBtn()
    {
        SceneManager.LoadScene(1);
    }
    #endregion
    //設定第一題
    public void Test_TitleBtn()
    {
        TestType = 0;
        HomeButton.SetActive(false);
        SkinStage.ResetStage();
        TestTitlePage.SetActive(false);
        isTesting = true;
        Test_SkinPage.SetActive(true);
        HomeButton.SetActive(true);
        //語音_測驗_花色解釋
        SoundSource.Stop();
        SoundSource.clip = Test_HintSound[0];
        SoundSource.Play();
        Save_BasicInfo.Instance.StartTime = DateTime.Now.ToString();
    }

    //確認測驗否結束
    public void CheckTestFin()
    {
        //重新計時
        Timer = 0;
        //結束
        if (TestQuizNum == 5)
        {
            TestFin = true;
            LoadingPage.SetActive(true);
            #region 設定儲存的資料
            Save_BasicInfo.Instance.EndTime = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
            string Ans = "";
            #region 整理寫入資訊
            //處理總統計資料
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
                UnitScore[i] = int.Parse(Test_CountScore[i]);
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
            WriteInRecord = Save_BasicInfo.Instance.SchoolName + "@" + Save_BasicInfo.Instance.ClassName + "@" + Save_BasicInfo.Instance.StuNum + "@" + Save_BasicInfo.Instance.Name + "@1@" + Save_BasicInfo.Instance.StartTime + "@" + Save_BasicInfo.Instance.EndTime + "@" + Ans;
            //寫入資料
            StartCoroutine(WriteStudentData());
            #endregion
        }
        //繼續測驗
        else
        {
            Test_SkinPage.SetActive(false);
            Test_ShadowPage.SetActive(false);
            Test_SoundPage.SetActive(false);
            //在1.3.5題各出現不一樣的題目類型，其他隨機
            if (TestQuizNum == 2)
            {
                TestType = 1;
                ShadowSoundTest.ResetStage();
                Test_ShadowPage.SetActive(true);
                //語音_測驗_解釋
                SoundSource.Stop();
                SoundSource.clip = Test_HintSound[1];
                SoundSource.Play();
            }
            else
            {
                if (TestQuizNum == 4)
                {
                    TestType = 2;
                    ShadowSoundTest.ResetStage();
                    Test_SoundPage.SetActive(true);
                    //語音_測驗_叫聲解釋
                    SoundSource.Stop();
                    SoundSource.clip = Test_HintSound[1];
                    SoundSource.Play();
                    Invoke("BirdSound", 5f);
                }
                else
                {
                    TestType = UnityEngine.Random.Range(0, 3);
                    switch (TestType)
                    {
                        case 0:
                            SkinStage.ResetStage();
                            Test_SkinPage.SetActive(true);
                            //語音_測驗_花色解釋
                            SoundSource.Stop();
                            SoundSource.clip = Test_HintSound[0];
                            SoundSource.Play();
                            break;
                        case 1:
                            ShadowSoundTest.ResetStage();
                            Test_ShadowPage.SetActive(true);
                            //語音_測驗_剪影解釋
                            SoundSource.Stop();
                            SoundSource.clip = Test_HintSound[1];
                            SoundSource.Play();
                            break;
                        case 2:
                            ShadowSoundTest.ResetStage();
                            Test_SoundPage.SetActive(true);
                            //語音_測驗_叫聲解釋
                            SoundSource.Stop();
                            SoundSource.clip = Test_HintSound[1];
                            SoundSource.Play();
                            Invoke("BirdSound", 5f);
                            break;
                    }
                }
            }
            isTesting = true;
        }
    }
    public void CancelBirdSound()
    {
        CancelInvoke("BirdSound");
    }
    public void BirdSound()
    {
        //SoundSource.clip = ShadowSoundTest.SelectSound;
        //SoundSource.Play();
    }
    #endregion

    #region 寫入資料
    //寫入教學資料
    IEnumerator WriteStudentData()
    {
        WWWForm form = new WWWForm();
        //辨識傳輸方式
        form.AddField("method", "write");
        //輸入要寫入的工作表號碼，從0開始到6，共7個單元，測驗及教學的資料在同一個單元的工作表內
        form.AddField("Num", "1");
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
        //單元答案的欄數：從2開始，3、5、7、9、11、13
        form.AddField("ColumnNum", "3");
        #region 計算有收集的動物
        string data = "";
        for (int i = 0; i < TutorialRecords.Length - 1; i++)
        {
            if (TutorialRecords[i] == "true")
            {
                data += "1@";
            }
            else
            {
                data += "0@";
            }
        }
        if (TutorialRecords[TutorialRecords.Length - 1] == "true")
        {
            data += "1";
        }
        else
        {
            data += "0";
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
        form_Read.AddField("Num", "1");
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
        Test_SkinPage.SetActive(false);
        Test_ShadowPage.SetActive(false);
        Test_SoundPage.SetActive(false);
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
