using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DayPanel : UI_ListPanelBase<UI_DayPanel>
{
       
    //AllPart下有多个Part，每个Part有一个Title和多个Content
    private GameObject allPart;
    //Part的预制体
    public GameObject Part;
    //Content的预制体
    public GameObject Content;

    //场景初始加载时：先调用OnEnable，再调用Close
    private bool finishInit = false;
    //数据
    //当天新增的田地的数量
    private Dictionary<string,int> CropBuildNum = new Dictionary<string,int>();
    //当天新增的建筑的数量
    private Dictionary<string,int> BuildingNum = new Dictionary<string,int>();
    //当天新增的作物的数量（进入仓库的）
    private Dictionary<string,int> CropNum = new Dictionary<string, int>();

    private void Awake()
    {
        allPart = transform.Find("BG/AllPart").gameObject;
        CropBuildNum.Add("向日葵", 5);
        BuildingNum.Add("商店", 1);
        CropNum.Add("向日葵", 25);
    }

    private void OnEnable()
    {
        //如果是初始化时
        if(!finishInit) {
            return;
        }
        //否则表明当天结束
        DayOver();
    }
    protected override void CloseButtonClick()
    {   //如果是初始化时
        if (!finishInit) {
            finishInit = true;
        }
        //否则表明要进入下一天
        else
        {
            NextDay();
        }
        base.CloseButtonClick();
    }

    //当天结束
    private void DayOver()
    {
        Debug.Log("DayOver");
        //停止计时
        MyTimer.Instance.StopClock();
        //切换天气
        WeatherController.Instance.SwitchWeather();
        //显示结算界面
        //首先检测不为空的字典
        GameObject temp;
        GameObject content;
        if (CropBuildNum.Count > 0)
        {
            temp = Instantiate(Part, allPart.transform);
            temp.transform.Find("Title/Text").GetComponent<Text>().text = "新增田地";
            //遍历字典
            foreach(var item in CropBuildNum)
            {
                content = Instantiate(Content, temp.transform);
                Content.transform.Find("Item").GetComponent<Text>().text = item.Key;
                Content.transform.Find("Count").GetComponent<Text>().text = item.Value.ToString();

            }
        }
        if(BuildingNum.Count > 0) {
            temp = Instantiate(Part, allPart.transform);
            temp.transform.Find("Title/Text").GetComponent<Text>().text = "新增建筑";
            foreach (var item in BuildingNum)
            {
                content = Instantiate(Content, temp.transform);
                Content.transform.Find("Item").GetComponent<Text>().text = item.Key;
                Content.transform.Find("Count").GetComponent<Text>().text = item.Value.ToString();

            }
        }
        if (CropNum.Count > 0)
        {
            temp = Instantiate(Part, allPart.transform);
            temp.transform.Find("Title/Text").GetComponent<Text>().text = "收获作物";
            foreach (var item in CropNum)
            {
                content = Instantiate(Content, temp.transform);
                Content.transform.Find("Item").GetComponent<Text>().text = item.Key;
                Content.transform.Find("Count").GetComponent<Text>().text = item.Value.ToString();

            }
        }



    }
    //进入下一天
    private void NextDay()
    {
        Debug.Log("NextDay");
        //开始计时
        MyTimer.Instance.BeginClock();
        //删除AllPart的所有子物体
        for(int i=0;i<allPart.transform.childCount;i++)
        {
            Destroy(allPart.transform.GetChild(i).gameObject);
        }
        //清空字典
        CropBuildNum.Clear();
        BuildingNum.Clear();
        CropNum.Clear();
    }
    //新增田地
    public void BuildCrop(string name)
    {
        if (CropBuildNum.ContainsKey(name))
        {
            CropBuildNum[name]++;
        } else
        {
            CropBuildNum.Add(name, 1);
        }
    }
    //新增建筑
    public void BuildBuilding(string name)
    {
        if (BuildingNum.ContainsKey(name))
        {
            BuildingNum[name]++;
        }
        else
        {
            BuildingNum.Add(name, 1);
        }
    }
    //收获作物
    public void GetCrop(string name,int num)
    {
        if (CropNum.ContainsKey(name))
        {
            CropNum[name]+=num;
        }
        else
        {
            CropNum.Add(name, num);
        }
    }
}
