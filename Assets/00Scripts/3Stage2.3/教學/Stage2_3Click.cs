using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_3Click : MonoBehaviour
{
    [Header("顯示題目")]
    public bool isClick;
    [Header("抓取題目數字")]
    public int Num;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 開啟題目
    /// </summary>
    public void Click()
    {
        isClick = true;
        GameObject.FindObjectOfType<Stage2_3Menu>().Tu_SelectAnimal();
    }
}
