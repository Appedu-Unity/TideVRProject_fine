using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class TitleMenu : MonoBehaviour
{
    [Header("聲音調整")]
    public AudioSource EffectSource;
    public AudioSource BGMSource, SoundSource;
    public AudioClip ClickSound;
    [Header("語音")]
    public AudioClip TitleSound;
    //看OFF的Toggle
    public Toggle BGMOFF,EffectOFF;
    [Header("頁面")]
    public GameObject InfoPage;
    public GameObject ModePage, TutorialPage, TestPage,LoadingPage;
    public GameObject[] Tutorials = new GameObject[2];
    public GameObject[] Tests = new GameObject[2];
    [Header("基本資料(從GoogleSheet下載)")]
    public Dropdown SchoolDropDown;
    public Dropdown ClassDropdown, NameDropdown;
    public GameObject NoDataPage;
    public string[] StuDatas, SchoolNames;
    public List<string> SortOptions;
    public List<string> StuNumList;
    public string[] SchoolOptions, NameOptions, ClassOptions; 
    public bool NoClass, isSetDrop;

    // Start is called before the first frame update
    void Start()
    {
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
        #region 設定開始資料
        //有登入帳號
        if (Save_BasicInfo.Instance.isLogin)
        {
            LoadingPage.SetActive(true);
            InfoPage.SetActive(false);
            ModePage.SetActive(true);
            isSetDrop = true;
            StartCoroutine(ReadSchoolData());         
        }
        //無登入帳號
        else
        {
            LoadingPage.SetActive(true);
            isSetDrop = true;
            StartCoroutine(ReadSchoolData());
        }
        #endregion
        GameObject.Find("BlackCanvas").GetComponent<Animator>().Play("黑幕_淡出", 0, 0);
        Invoke("BlackDisAppear", 1f);
    }
    void BlackDisAppear()
    {
        GameObject.Find("BlackCanvas").gameObject.SetActive(false);
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
    public void DropClick()
    {
        if (!isSetDrop)
        {
            EffectSource.Stop();
            SoundSource.Stop();
            EffectSource.clip = ClickSound;
            EffectSource.Play();
        }
    }

    public void BackTitle_Sound()
    {
        SoundSource.Stop();
        SoundSource.clip = TitleSound;
        SoundSource.Play();
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
    }
    #endregion

    #region 切換頁面按鈕
    #region 進入選擇模式按鈕
    public void EnterBtn()
    {
        if (ClassDropdown.options[ClassDropdown.value].text == "選擇班級…" || NameDropdown.options[NameDropdown.value].text == "選擇名字…")
        {
            //資料沒有填寫完全
            NoDataPage.SetActive(true);
        }
        else
        {
            //儲存資料
            Save_BasicInfo.Instance.SchoolName = SchoolDropDown.options[SchoolDropDown.value].text;
            Save_BasicInfo.Instance.ClassName = ClassDropdown.options[ClassDropdown.value].text;
            Save_BasicInfo.Instance.Name = NameDropdown.options[NameDropdown.value].text;
            Save_BasicInfo.Instance.StuNum = StuNumList[NameDropdown.value];
            Save_BasicInfo.Instance.isLogin = true;
            LoadingPage.SetActive(true);
            //先寫入APP讀取初始基本資料
            //代表是初次遊玩
            if (Save_BasicInfo.Instance.WaveTutorialRecord != "")
            {
                string info = Save_BasicInfo.Instance.WaveTutorialRecord;
                Save_BasicInfo.Instance.WaveTutorialRecord = Save_BasicInfo.Instance.SchoolName + "@" + Save_BasicInfo.Instance.ClassName + "@" + Save_BasicInfo.Instance.StuNum + "@" + Save_BasicInfo.Instance.Name + "@0@" + Save_BasicInfo.Instance.StartTime + "@" + Save_BasicInfo.Instance.EndTime + "@" + info;
                //上傳初次遊玩的資料資料
                StartCoroutine(WriteStudentData());
            }
            else
            {
                StartCoroutine(WriteAPPData());
            }
            
        }
    }
    #endregion

    #region 進入選擇關卡按鈕
    public void SetModeBtn(int ModeNum)
    {
        ModePage.SetActive(false);
        //教學
        if (ModeNum == 0)
        {
            Tutorials[0].SetActive(true);
            Tutorials[1].SetActive(false);
            TutorialPage.SetActive(true);
        }
        //測驗
        else
        {
            Tests[0].SetActive(true);
            Tests[1].SetActive(false);
            TestPage.SetActive(true);
        }
    }
    #endregion
    #endregion

    #region 讀取資料
    public void SchoolDropClick()
    {
        StartCoroutine(ReadClassData());
        LoadingPage.SetActive(true);
        isSetDrop = true;
    }
    public void ClassDropCLick()
    {
        StartCoroutine(ReadNameData());
        LoadingPage.SetActive(true);
        isSetDrop = true;
    }
    #region 讀取學校名稱
    IEnumerator ReadSchoolData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //學校名稱的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbyYvCzfpC9rkr4jxHa_70GTOMVKNuVd2ECVnxZFGTMLYQXPeAIJMnR1Yvv1Y1dyz7k2/exec", form_Read))
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
                SchoolOptions = www.downloadHandler.text.Split(',');
                List<string> SchoolNames = new List<string>();
                for (int i = 0; i < SchoolOptions.Length; i++)
                {
                    SchoolNames.Add(SchoolOptions[i]);
                }
                SchoolDropDown.ClearOptions();
                SchoolDropDown.AddOptions(SchoolNames);
                SchoolDropDown.value = 0;
                Debug.Log("Download School!");
            }
        }
        StartCoroutine(ReadClassData());
    }
    #endregion
    #region 讀取班級名稱
    IEnumerator ReadClassData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //辨識要讀取的類型 Class為讀取班級名稱
        form_Read.AddField("Type", "Class");
        //班級跟學生名冊的試算表
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbylwdZsT9oKcOCSBKl0WaOUVqxZG3xAyUQs-2QrkBHvwdxioxbj2Zq-OUOsXKHawuypdQ/exec", form_Read))
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
                ClassOptions = www.downloadHandler.text.Split(',');
                List<string> SelectClassNames = new List<string>();
                for (int i = 0; i < ClassOptions.Length; i++)
                {
                    if (ClassOptions[i].Contains(SchoolDropDown.options[SchoolDropDown.value].text))
                    {
                        SelectClassNames.Add(ClassOptions[i].Split('_')[1]);
                    }
                }
                if (SelectClassNames.Count == 0)
                {
                    SelectClassNames.Add("選擇班級…");
                    NameDropdown.ClearOptions();
                    List<string> SelectNames = new List<string>();
                    SelectNames.Add("選擇姓名…");
                    NameDropdown.AddOptions(SelectNames);
                    NameDropdown.value = 0;
                    NoClass = true;
                }
                else
                {
                    NoClass = false;
                }    
                ClassDropdown.ClearOptions();
                ClassDropdown.AddOptions(SelectClassNames);
                ClassDropdown.value = 0;
                Debug.Log("Download Class!");
            }
        }
        if (NoClass)
        {
            LoadingPage.SetActive(false);
            NoDataPage.SetActive(true);
            isSetDrop = false;
        }
        else
        {
            StartCoroutine(ReadNameData());
        }
    }
    #endregion
    #region 讀取學生名稱
    IEnumerator ReadNameData()
    {
        WWWForm form_Read = new WWWForm();
        //辨識傳輸方式
        form_Read.AddField("method", "Read");
        //辨識要讀取的類型
        form_Read.AddField("Type", "Student");
        //讀特定工作表名稱，名稱格式：學校名稱_班級
        form_Read.AddField("ClassName", SchoolDropDown.options[SchoolDropDown.value].text + "_" + ClassDropdown.options[ClassDropdown.value].text);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbylwdZsT9oKcOCSBKl0WaOUVqxZG3xAyUQs-2QrkBHvwdxioxbj2Zq-OUOsXKHawuypdQ/exec", form_Read))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            // if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //下載的資料：首2筆資料是名稱欄，剩下欄位的偶數為姓名
                NameOptions = www.downloadHandler.text.Split(',');
                List<string> SelectNames = new List<string>();
                StuNumList.Clear();
                for (int i = 0; i < (NameOptions.Length / 2) - 1; i++)
                {
                    SelectNames.Add(NameOptions[(i + 3) + i]);
                    StuNumList.Add(NameOptions[(i + 2) + i]);
                }
                NameDropdown.ClearOptions();
                NameDropdown.AddOptions(SelectNames);
                NameDropdown.value = 0;
                isSetDrop = false;
                LoadingPage.SetActive(false);
                //語音
                if (!Save_BasicInfo.Instance.isLogin)
                {
                    SoundSource.Stop();
                    SoundSource.clip = TitleSound;
                    SoundSource.Play();
                }
                Debug.Log("Download Complete!");
            }
        }
    }
    #endregion
    #endregion

    #region 寫入資料
    #region 寫入第一次教學資料
    IEnumerator WriteStudentData()
    {
        WWWForm form = new WWWForm();
        //辨識傳輸方式
        form.AddField("method", "write");
        //輸入要寫入的工作表號碼，從0開始到6，共7個單元，測驗及教學的資料在同一個單元的工作表內
        form.AddField("Num", "0");
        //寫入資料
        form.AddField("Data", Save_BasicInfo.Instance.WaveTutorialRecord);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbwcON_NqaxuQIL0zSgex0fOr3rOiVjQFhVt1LYmxQQnU3Tm8KK3kZHESgGJ3AjuXGc_yg/exec", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Save_BasicInfo.Instance.WaveTutorialRecord = "";
                StartCoroutine(WriteAPPData());
            }
        }
    }
    #endregion
    #region 寫入APP讀取初始資料
    IEnumerator WriteAPPData()
    {
        WWWForm form = new WWWForm();
        //辨識傳輸方式
        form.AddField("method", "WriteAPPName");
        //輸入要寫入的工作表號碼，APP讀取資料為8
        form.AddField("Num", "9");
        //基本資訊
        string Info = Save_BasicInfo.Instance.SchoolName + "@" + Save_BasicInfo.Instance.ClassName + "@" + Save_BasicInfo.Instance.StuNum;
        //改成寫一欄，資訊用@隔開，分數用_隔開
        form.AddField("PensonInfo", Info);
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbwcON_NqaxuQIL0zSgex0fOr3rOiVjQFhVt1LYmxQQnU3Tm8KK3kZHESgGJ3AjuXGc_yg/exec", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Save_BasicInfo.Instance.APPDataNum = www.downloadHandler.text;
                LoadingPage.SetActive(false);
                InfoPage.SetActive(false);
                ModePage.SetActive(true);
                Debug.Log("Upload Complete!");
            }
        }
    }
    #endregion
    #endregion

    #region 切換場景
    //離開遊戲
    public void ExitBtn()
    {
        Application.Quit();
    }
    #region 進入遊戲_教學
    //從0開始輸入
    public void TutorialBtn(int StageNum)
    {
        Save_BasicInfo.Instance.StageNum = StageNum.ToString();
        if (StageNum<3)
        {
            if (StageNum == 0)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(StageNum + 1);
            }

        }
        else
        {
            
            SceneManager.LoadScene(StageNum);
        }

    }
    #endregion
    #region 進入遊戲_測驗
    //從1開始輸入
    public void TestBtn(int StageNum)
    {
        Save_BasicInfo.Instance.isTestMode = true;
        Save_BasicInfo.Instance.StageNum = StageNum.ToString();
        if (StageNum < 3)
        {
            SceneManager.LoadScene(StageNum + 1);
        }
        else
        {
            SceneManager.LoadScene(StageNum);
        }
    }
    #endregion
    #endregion
}
