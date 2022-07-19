using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage1_SkinTest : MonoBehaviour
{
    public Stage1Menu Menu;
    public RandomTagPlace TagPlace;
    public Text TestTilteText;
    public int AnsNum;
    //設定圖片
    public Image[] SkinImage = new Image[4];
    public Sprite[] SkinSprite = new Sprite[13];
    public List<Sprite> RandomSprite;
    //設定作答位置
    public ObjectRotate RotateControl;
    public Transform SpotPlace, ModelPlace;
    public GameObject[] AnsSpots = new GameObject[13];
    public GameObject[] Models = new GameObject[13];
    public Material[] AnimSkins = new Material[13];
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region 重設關卡
    public void ResetStage()
    {
        TagPlace.Reload();
        //補充題目
        if (Save_BasicInfo.Instance.Stage1_SkinQuiz.Count < 1)
        {
            Save_BasicInfo.Instance.Stage1_SkinQuiz.Clear();
            string[] Quizs = Save_BasicInfo.Instance.Stage1_Skin_AllQuiz.Split('@');
            for (int i = 0; i < Quizs.Length; i++)
            {
                Save_BasicInfo.Instance.Stage1_SkinQuiz.Add(Quizs[i]);
            }
        }
        //
        AnsNum = Random.Range(0,Save_BasicInfo.Instance.Stage1_SkinQuiz.Count);
        for (int i = 0; i < SpotPlace.childCount; i++)
        {
            Destroy(SpotPlace.GetChild(i).gameObject);
        }
        for (int i = 0; i < ModelPlace.childCount; i++)
        {
            Destroy(ModelPlace.GetChild(i).gameObject);
        }
        string[] Quiz = Save_BasicInfo.Instance.Stage1_SkinQuiz[AnsNum].Split('_');
        Save_BasicInfo.Instance.Stage1_SkinQuiz.Remove(Save_BasicInfo.Instance.Stage1_SkinQuiz[AnsNum]);
        //設定題目
        AnsNum = int.Parse(Quiz[0]);
        Menu.TestRecords[Menu.TestQuizNum] = Quiz[1];
        //設定題目文字
        string[] SpiltName = SkinSprite[AnsNum].name.Split('_');
        TestTilteText.text = "下列何者為"+ SpiltName[2]+ "的花色？";
        #region 生成題目
        Instantiate(AnsSpots[AnsNum], SpotPlace.position, SpotPlace.rotation, SpotPlace);
        RotateControl.TurnObject =  Instantiate(Models[AnsNum], ModelPlace.position, ModelPlace.rotation, ModelPlace);
        //設定花色圖片
        RandomSprite.Clear();
        for (int i = 0; i < SkinSprite.Length; i++)
        {
            if (i != AnsNum)
            {
                RandomSprite.Add(SkinSprite[i]);
            }
        }
        SkinImage[0].sprite = SkinSprite[AnsNum];
        for (int i = 1; i < SkinImage.Length; i++)
        {
            int Num = Random.Range(0, RandomSprite.Count);
            SkinImage[i].sprite = RandomSprite[Num];
            RandomSprite.Remove(RandomSprite[Num]);
        }
        #endregion
    }
    #endregion

    #region 放置標籤
    public void PutSkin()
    {
        //如果有放標籤
        if(SpotPlace.GetChild(0).transform.childCount > 0)
        {
            for (int i = 0; i < AnimSkins.Length; i++)
            {
                if (SpotPlace.GetChild(0).GetChild(0).GetComponent<Image>().sprite.name.Split('_')[2] == AnimSkins[i].name.Split('_')[3])
                {
                    ModelPlace.GetChild(0).GetComponent<SkinTest_ColorPlace>().ColoredPlace.GetComponent<Renderer>().material = AnimSkins[i];
                }
            }
            SpotPlace.GetChild(0).GetChild(0).GetComponent<TagsTrigger>().BackParentPlace();
        }
    }
    #endregion

    #region 提交答案
    public void SubmitAns()
    {
        #region 確認答案
        if (ModelPlace.GetChild(0).GetComponent<SkinTest_ColorPlace>().ColoredPlace.GetComponent<Renderer>().material.name == "White")
        {
            Menu.TestRecords[Menu.TestQuizNum] += "_<color=#ff0000>未選擇</color>";
        }
        else
        {
            //正確
            if (ModelPlace.GetChild(0).GetComponent<SkinTest_ColorPlace>().ColoredPlace.GetComponent<Renderer>().material.name.Contains(AnimSkins[AnsNum].name.Split('_')[3]))
            {
                Menu.TestRecords[Menu.TestQuizNum] += "_正確";
            }
            //錯誤
            else
            {
                string[] SpiltName = ModelPlace.GetChild(0).GetComponent<SkinTest_ColorPlace>().ColoredPlace.GetComponent<Renderer>().material.name.Split('_');
                Menu.TestRecords[Menu.TestQuizNum] += "_<color=#ff0000>" + SpiltName[3] + "</color>";
            }
        }
        #endregion
        //加音效
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
