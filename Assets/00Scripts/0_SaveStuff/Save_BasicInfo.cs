using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_BasicInfo : MonoBehaviour
{
    //Mode：用0、1區分，0：教學、1：測驗
    //StageNum：用數字區分，0為教學關
    public string SchoolName, ClassName, StuNum,Name, StageNum, StartTime, EndTime;
    public bool isLogin, isTestMode;
    [Header("音量調整")]
    //BGM：背景、Effect：音效、Sound：人聲
    public bool BGMOff, EffectOff, SoundOff;
    public static Save_BasicInfo Instance;
    [Header("APP順序")]
    public string APPDataNum;
    [Header("遊玩紀錄")]
    public string WaveTutorialRecord;
    [Header("測驗題目")]
    [Header("單元一")]
    public string Stage1_Skin_AllQuiz;
    public List<string> Stage1_SkinQuiz;
    public string Stage1_Shadow_AllQuiz;
    public List<string> Stage1_ShadowQuiz;
    public string Stage1_Sound_AllQuiz;
    public List<string> Stage1_SoundQuiz;
    [Header("單元二")]
    public string Stage2_Organ_AllQuiz;
    public List<string> Stage2_OrganQuiz;
    public string Stage2_Exercise_AllQuiz;
    public List<string> Stage2_ExerciseQuiz;
    [Header("單元三")]
    public string Stage3_Organ_AllQuiz;
    public List<string> Stage3_OrganQuiz;
    public string Stage3_Exercise_AllQuiz;
    public List<string> Stage3_ExerciseQuiz;
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
