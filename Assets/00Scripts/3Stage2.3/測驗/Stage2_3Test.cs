using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2_3Test : MonoBehaviour
{
    public Stage2_3Menu Menu;
    [Header("基本")]
    public int AnsNum;
    public Text Exercise_TilteText;
    [Header("器官配對")]
    public Transform OrganTagPlace;
    public GameObject[] OrganTagPrefab2 = new GameObject[5];
    public GameObject[] OrganTagPrefab3 = new GameObject[8];
    [Header("運動")]
    public int AnimalType;
    public string[] AnimalNames2 =new string[5];
    public string[] AnimalNames3 = new string[8];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region 設定題目
    public void ResetStage_Organ()
    {
        #region 單元2
        if (Save_BasicInfo.Instance.StageNum == "2")
        {
            //補充題目
            if (Save_BasicInfo.Instance.Stage2_OrganQuiz.Count < 1)
            {
                Save_BasicInfo.Instance.Stage2_OrganQuiz.Clear();
                string[] Quizs = Save_BasicInfo.Instance.Stage2_Organ_AllQuiz.Split('@');
                for (int i = 0; i < Quizs.Length; i++)
                {
                    Save_BasicInfo.Instance.Stage2_OrganQuiz.Add(Quizs[i]);
                }
            }
            //設定題目
            AnsNum = Random.Range(0, Save_BasicInfo.Instance.Stage2_OrganQuiz.Count);
            string[] QuizInfo = Save_BasicInfo.Instance.Stage2_OrganQuiz[AnsNum].Split('_');
            Menu.TestRecords[Menu.TestQuizNum] = QuizInfo[1];
            Save_BasicInfo.Instance.Stage2_OrganQuiz.Remove(Save_BasicInfo.Instance.Stage2_OrganQuiz[AnsNum]);
            AnsNum = int.Parse(QuizInfo[0]);
            //生成Tag物件
            if (OrganTagPlace.childCount != 0)
            {
                for (int i = 0; i < OrganTagPlace.childCount; i++)
                {
                    Destroy(OrganTagPlace.GetChild(i).gameObject);
                }
            }
            Instantiate(OrganTagPrefab2[AnsNum], OrganTagPlace.position, OrganTagPlace.rotation, OrganTagPlace);
        }
        #endregion
        #region 單元3
        else
        {
            //補充題目
            if (Save_BasicInfo.Instance.Stage3_OrganQuiz.Count < 1)
            {
                Save_BasicInfo.Instance.Stage3_OrganQuiz.Clear();
                string[] Quizs = Save_BasicInfo.Instance.Stage3_Organ_AllQuiz.Split('@');
                for (int i = 0; i < Quizs.Length; i++)
                {
                    Save_BasicInfo.Instance.Stage3_OrganQuiz.Add(Quizs[i]);
                }
            }
            //設定題目
            AnsNum = Random.Range(0, Save_BasicInfo.Instance.Stage3_OrganQuiz.Count);
            string[] QuizInfo = Save_BasicInfo.Instance.Stage3_OrganQuiz[AnsNum].Split('_');
            Menu.TestRecords[Menu.TestQuizNum] = QuizInfo[1];
            Save_BasicInfo.Instance.Stage3_OrganQuiz.Remove(Save_BasicInfo.Instance.Stage3_OrganQuiz[AnsNum]);
            AnsNum = int.Parse(QuizInfo[0]);
            //生成Tag物件
            if (OrganTagPlace.childCount != 0)
            {
                for (int i = 0; i < OrganTagPlace.childCount; i++)
                {
                    Destroy(OrganTagPlace.GetChild(i).gameObject);
                }
            }
            Instantiate(OrganTagPrefab3[AnsNum], OrganTagPlace.position, OrganTagPlace.rotation, OrganTagPlace);
        }
        #endregion
    }

    public void ResetStage_Exercise()
    {
        if (Save_BasicInfo.Instance.StageNum == "2")
        {
            //補充題目
            if (Save_BasicInfo.Instance.Stage2_ExerciseQuiz.Count < 1)
            {
                Save_BasicInfo.Instance.Stage2_ExerciseQuiz.Clear();
                string[] Quizs = Save_BasicInfo.Instance.Stage2_Exercise_AllQuiz.Split('@');
                for (int i = 0; i < Quizs.Length; i++)
                {
                    Save_BasicInfo.Instance.Stage2_ExerciseQuiz.Add(Quizs[i]);
                }
            }
            //設定題目
            AnsNum = Random.Range(0, Save_BasicInfo.Instance.Stage2_ExerciseQuiz.Count);
            string[] QuizInfo = Save_BasicInfo.Instance.Stage2_ExerciseQuiz[AnsNum].Split('_');
            Menu.TestRecords[Menu.TestQuizNum] = QuizInfo[1];
            Save_BasicInfo.Instance.Stage2_ExerciseQuiz.Remove(Save_BasicInfo.Instance.Stage2_ExerciseQuiz[AnsNum]);
            AnsNum = int.Parse(QuizInfo[0]);
            //生成題目
            AnimalType = 0;
            Exercise_TilteText.text = "下列何者為" + AnimalNames2[AnsNum] + "的運動方式？";
        }
        else
        {
            //補充題目
            if (Save_BasicInfo.Instance.Stage3_ExerciseQuiz.Count < 1)
            {
                Save_BasicInfo.Instance.Stage3_ExerciseQuiz.Clear();
                string[] Quizs = Save_BasicInfo.Instance.Stage3_Exercise_AllQuiz.Split('@');
                for (int i = 0; i < Quizs.Length; i++)
                {
                    Save_BasicInfo.Instance.Stage3_ExerciseQuiz.Add(Quizs[i]);
                }
            }
            //設定題目
            AnsNum = Random.Range(0, Save_BasicInfo.Instance.Stage3_ExerciseQuiz.Count);
            string[] QuizInfo = Save_BasicInfo.Instance.Stage3_ExerciseQuiz[AnsNum].Split('_');
            Menu.TestRecords[Menu.TestQuizNum] = QuizInfo[1];
            Save_BasicInfo.Instance.Stage3_ExerciseQuiz.Remove(Save_BasicInfo.Instance.Stage3_ExerciseQuiz[AnsNum]);
            AnsNum = int.Parse(QuizInfo[0]);
            //生成題目
            if (AnsNum > 4)
            {
                //螺貝類
                AnimalType = 1;
            }
            else
            {
                //蟹類
                AnimalType = 2;
            }
            Exercise_TilteText.text = "下列何者為" + AnimalNames3[AnsNum] + "的運動方式？";
        }
 
    }
    #endregion

    #region 確認作答
    public void SubmitOrganBtn()
    {
        //確認答案
        bool isWrong = false;
        for (int i = 0; i < OrganTagPlace.GetChild(0).GetChild(1).childCount; i++)
        {
            string AnsSpot = OrganTagPlace.GetChild(0).GetChild(1).GetChild(i).gameObject.name;
            if (OrganTagPlace.GetChild(0).GetChild(1).GetChild(i).childCount > 0)
            {
                if (!OrganTagPlace.GetChild(0).GetChild(1).GetChild(i).GetChild(0).gameObject.name.Contains(AnsSpot))
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
            Menu.TestRecords[Menu.TestQuizNum] += "_<color=#ff0000>擺放錯誤</color>";
        }
        //正確
        else
        {
            Menu.TestRecords[Menu.TestQuizNum] += "_正確";
        }     
        //
        Menu.TestSound();
        //換下一題並記錄時間，題目編號增加
        Menu.isTesting = false;
        int TestTime = (int)Menu.Timer;
        Menu.TestTimer[Menu.TestQuizNum] = TestTime.ToString();
        Menu.TestQuizNum++;
        //確認現在的題數
        Menu.CheckTestFin();
    }

    public void SubmitExercisBtn(int Num)
    {
        //確認答案
        if (Num == AnimalType)
        {
            Menu.TestRecords[Menu.TestQuizNum] += "_正確";
        }
        else
        {
            switch (Num)
            {
                case 0:
                    Menu.TestRecords[Menu.TestQuizNum] += "_<color=#ff0000>飛行</color>";
                    break;
                case 1:
                    Menu.TestRecords[Menu.TestQuizNum] += "_<color=#ff0000>爬行</color>";
                    break;
                case 2:
                    Menu.TestRecords[Menu.TestQuizNum] += "_<color=#ff0000>行走</color>";
                    break;
            }
        }
        //
        Menu.TestSound();
        //換下一題並記錄時間，題目編號增加
        Menu.isTesting = false;
        int TestTime = (int)Menu.Timer;
        Menu.TestTimer[Menu.TestQuizNum] = TestTime.ToString();
        Menu.TestQuizNum++;
        //確認現在的題數
        Menu.CheckTestFin();
    }
    #endregion
}
