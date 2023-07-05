using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DayPanel : UI_ListPanelBase<UI_DayPanel>
{

    public Text weather;
       
    //AllPart下有多个Part，每个Part有一个Title和多个Content
    private GameObject allPart;
    //Part的预制体
    public GameObject Part;
    //Content的预制体
    public GameObject Content;
    //过场动画
    public GameObject Animation;

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
        Instance = this;
        allPart = transform.Find("BG/AllPart").gameObject;
        weather = transform.parent.Find("MainPanel/CurrentWeather").gameObject.GetComponent<Text>();  
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
        //如果说第一天结束了，在DayOver中day已经变为了2，那么预报天气的时候就是预报的第2+1天的天气，切换天气就是切换到第2天的天气
        //切换至第三人称
        Camer_C.Instance.SwitchTo3();
        //停止计时
        MyTimer.Instance.StopClock();
        //预报天气
        ChatScript.Instance.foreCastWeather();

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
                content.transform.Find("Item").GetComponent<Text>().text = item.Key;
                content.transform.Find("Count").GetComponent<Text>().text = item.Value.ToString();

            }
        }
        if(BuildingNum.Count > 0) {
            temp = Instantiate(Part, allPart.transform);
            temp.transform.Find("Title/Text").GetComponent<Text>().text = "新增建筑";
            foreach (var item in BuildingNum)
            {
                content = Instantiate(Content, temp.transform);
                content.transform.Find("Item").GetComponent<Text>().text = item.Key;
                content.transform.Find("Count").GetComponent<Text>().text = item.Value.ToString();

            }
        }
        if (CropNum.Count > 0)
        {
            temp = Instantiate(Part, allPart.transform);
            temp.transform.Find("Title/Text").GetComponent<Text>().text = "收获作物";
            foreach (var item in CropNum)
            {
                content = Instantiate(Content, temp.transform);
                content.transform.Find("Item").GetComponent<Text>().text = item.Key;
                content.transform.Find("Count").GetComponent<Text>().text = item.Value.ToString();

            }
        }
        //如果全都为空
        if(CropBuildNum.Count == 0 && BuildingNum.Count == 0 && CropNum.Count == 0)
        {
            temp = Instantiate(Part, allPart.transform);
            temp.transform.Find("Title/Text").GetComponent<Text>().text = "今日无事发生";
        }


    }
    //进入下一天
    private void NextDay()
    {

        //显示天气
        weather.text ="当前天气："+ WeatherController.Instance.GetWeather();
        //删除AllPart的所有子物体
        for (int i=0;i<allPart.transform.childCount;i++)
        {
            Destroy(allPart.transform.GetChild(i).gameObject);
        }
        //清空字典
        CropBuildNum.Clear();
        BuildingNum.Clear();
        CropNum.Clear();
        //播放过场动画，播放完切换天气、重新计时
        Animation.SetActive(true);
        //切换天气，移动至动画加载部分
        //WeatherController.Instance.SwitchWeather();
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
        UI_illustrationPanel.Instance.AddCropTime(name);
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
        UI_illustrationPanel.Instance.AddBuildingTime(name);
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
