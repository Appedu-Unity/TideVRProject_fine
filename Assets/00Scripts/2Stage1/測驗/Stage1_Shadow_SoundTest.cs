using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage1_Shadow_SoundTest : MonoBehaviour
{
    public Stage1Menu Menu;
    public int AnsNum;
    [Header("剪影")]
    public Text ShadowTitleText;
    public GameObject[] ShadowAnsBtn;
    public List<GameObject> RandomShadowBtns;
    public Sprite[] ShadowSprites =new Sprite[13];
    public List<Sprite> RandomShadowSprites;
    [Header("叫聲")]
    public GameObject[] SoundAnsBtn;
    public List<GameObject> RandomSoundBtns;
    public Sprite[] SoundSprite = new Sprite[5];
    public List<Sprite> RandomSoundSprites;
    public AudioClip SelectSound;
    public AudioClip[] BirdSound = new AudioClip[5];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetStage()
    {
        #region 剪影
        if (Menu.TestType == 1)
        {
            //補充題目
            if (Save_BasicInfo.Instance.Stage1_ShadowQuiz.Count < 1)
            {
                Save_BasicInfo.Instance.Stage1_ShadowQuiz.Clear();
                string[] Quizs = Save_BasicInfo.Instance.Stage1_Shadow_AllQuiz.Split('@');
                for (int i = 0; i < Quizs.Length; i++)
                {
                    Save_BasicInfo.Instance.Stage1_ShadowQuiz.Add(Quizs[i]);
                }
            }
            //
            for (int i = 0; i < ShadowAnsBtn.Length; i++)
            {
                ShadowAnsBtn[i].GetComponent<Stage1_BtnClick>().isClick = false;
            }
            //設定題目
            AnsNum = Random.Range(0, Save_BasicInfo.Instance.Stage1_ShadowQuiz.Count);
            string[] QuizInfo = Save_BasicInfo.Instance.Stage1_ShadowQuiz[AnsNum].Split('_');
            Menu.TestRecords[Menu.TestQuizNum] = QuizInfo[1];
            Save_BasicInfo.Instance.Stage1_ShadowQuiz.Remove(Save_BasicInfo.Instance.Stage1_ShadowQuiz[AnsNum]);
            AnsNum = int.Parse(QuizInfo[0]);
            //設定題目文字
            string[] SpiltName = ShadowSprites[AnsNum].name.Split('_');
            ShadowTitleText.text = "下列何者剪影為" + SpiltName[2] + "？";
            #region 生成題目
            //設定圖片
            int SelectBtn = Random.Range(0, 3);
            ShadowAnsBtn[SelectBtn].transform.GetChild(0).GetComponent<Image>().sprite = ShadowSprites[AnsNum];
            RandomShadowBtns.Clear();
            RandomShadowSprites.Clear();
            for (int i = 0; i < ShadowAnsBtn.Length; i++)
            {
                if (i != SelectBtn)
                {
                    RandomShadowBtns.Add(ShadowAnsBtn[i].gameObject);
                }
            }
            for (int i = 0; i < ShadowSprites.Length; i++)
            {
                if (i != AnsNum)
                {
                    RandomShadowSprites.Add(ShadowSprites[i]);
                }
            }
            //生成其他選項
            for (int i = 0; i < RandomShadowBtns.Count; i++)
            {
                int Num = Random.Range(0, RandomSoundSprites.Count);
                RandomShadowBtns[i].transform.GetChild(0).GetComponent<Image>().sprite = RandomShadowSprites[Num];
                RandomShadowSprites.Remove(RandomShadowSprites[Num]);
            }
            #endregion
        }
        #endregion
        #region 叫聲
        else
        {
            //補充題目
            if (Save_BasicInfo.Instance.Stage1_SoundQuiz.Count < 1)
            {
                Save_BasicInfo.Instance.Stage1_SoundQuiz.Clear();
                string[] Quizs = Save_BasicInfo.Instance.Stage1_Sound_AllQuiz.Split('@');
                for (int i = 0; i < Quizs.Length; i++)
                {
                    Save_BasicInfo.Instance.Stage1_SoundQuiz.Add(Quizs[i]);
                }
            }
            AnsNum = Random.Range(0, Save_BasicInfo.Instance.Stage1_SoundQuiz.Count);
            string[] QuizInfo = Save_BasicInfo.Instance.Stage1_SoundQuiz[AnsNum].Split('_');
            Menu.TestRecords[Menu.TestQuizNum] = QuizInfo[1];
            Save_BasicInfo.Instance.Stage1_SoundQuiz.Remove(Save_BasicInfo.Instance.Stage1_SoundQuiz[AnsNum]);
            AnsNum = int.Parse(QuizInfo[0]);
            SelectSound = BirdSound[AnsNum];
            #region 生成題目
            for (int i = 0; i < SoundAnsBtn.Length; i++)
            {
                SoundAnsBtn[i].GetComponent<Stage1_BtnClick>().isClick = false;
            }
            int SelectNum = Random.Range(0, 3);
            SoundAnsBtn[SelectNum].transform.GetChild(0).GetComponent<Image>().sprite = SoundSprite[AnsNum];
            RandomSoundBtns.Clear();
            RandomSoundSprites.Clear();
            for (int i = 0; i < SoundAnsBtn.Length; i++)
            {
                if (i != SelectNum)
                {
                    RandomSoundBtns.Add(SoundAnsBtn[i]);
                }
            }
            for (int i = 0; i < SoundSprite.Length; i++)
            {
                if (i != AnsNum)
                {
                    RandomSoundSprites.Add(SoundSprite[i]);
                }
            }
            for (int i = 0; i < RandomSoundBtns.Count; i++)
            {
                int Num = Random.Range(0, RandomSoundSprites.Count);
                RandomSoundBtns[i].transform.GetChild(0).GetComponent<Image>().sprite = RandomSoundSprites[Num];
                RandomSoundSprites.Remove(RandomSoundSprites[Num]);
            }
            #endregion
        }
        #endregion
    }

    public void SubmitBtn(int BtnNum)
    {
        //剪影
        if (Menu.TestType == 1)
        {
            ShadowAnsBtn[BtnNum].GetComponent<Stage1_BtnClick>().isClick = true;
            //答對
            if (ShadowAnsBtn[BtnNum].transform.GetChild(0).GetComponent<Image>().sprite == ShadowSprites[AnsNum])
            {
                Menu.TestRecords[Menu.TestQuizNum] += "_正確";
            }
            //答錯
            else
            {
                string[] SpiltName = ShadowAnsBtn[BtnNum].transform.GetChild(0).GetComponent<Image>().sprite.name.Split('_');
                Menu.TestRecords[Menu.TestQuizNum] += "_<color=#ff0000>" + SpiltName[2] + "</color>";
            }
        }
        //叫聲
        else
        {
            SoundAnsBtn[BtnNum].GetComponent<Stage1_BtnClick>().isClick = true;
            //答對
            if (SoundAnsBtn[BtnNum].transform.GetChild(0).GetComponent<Image>().sprite == SoundSprite[AnsNum])
            {
                Menu.TestRecords[Menu.TestQuizNum] += "_正確";
            }
            //答錯
            else
            {
                string[] SpiltName = SoundAnsBtn[BtnNum].transform.GetChild(0).GetComponent<Image>().sprite.name.Split('_');
                Menu.TestRecords[Menu.TestQuizNum] += "_<color=#ff0000>" + SpiltName[1] + "</color>";
            }
        }
        //加音效
        Menu.CancelBirdSound();
        Menu.TestSound();
        //換下一題並記錄時間，題目編號增加
        Menu.isTesting = false;
        int TestTime = (int)Menu.Timer;
        Menu.TestTimer[Menu.TestQuizNum] = TestTime.ToString();
        Menu.TestQuizNum++;
        //確認現在的題數
        Menu.CheckTestFin();
    }
}
