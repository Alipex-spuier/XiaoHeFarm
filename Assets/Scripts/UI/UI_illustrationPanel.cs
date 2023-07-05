using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_illustrationPanel : UI_ListPanelBase<UI_illustrationPanel>
{
    //当前显示的类型
    private int type = 0;
    //暂时存储
    [SerializeField] GameObject tempParent;
    GameObject Crops;
    GameObject Buildings;
    GameObject Weathers;
    //图鉴
    [SerializeField] IllustrationConf illConf;
    //所在父物体
    [SerializeField] Transform parent;
    //预制体
    [SerializeField] GameObject prefab;
    //所有物体
    Dictionary<string, UI_IllItem> items = new Dictionary<string, UI_IllItem>();
    //植物
    Dictionary<string, UI_IllItem> crops = new Dictionary<string, UI_IllItem>();
    //建筑
    Dictionary<string, UI_IllItem> buildings = new Dictionary<string, UI_IllItem>();
    //天气
    Dictionary<string, UI_IllItem> weathers = new Dictionary<string, UI_IllItem>();
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < illConf.cropConfItems.Length; i++)
        {
            UI_IllItem item = GameObject.Instantiate(prefab, parent).GetComponent<UI_IllItem>();
            item.Init(illConf.cropConfItems[i]);
            items.Add(illConf.cropConfItems[i].Name, item);
            crops.Add(illConf.cropConfItems[i].Name, item);
        }
        for (int i = 0; i < illConf.buildConfItems.Length; i++)
        {
            UI_IllItem item = GameObject.Instantiate(prefab, parent).GetComponent<UI_IllItem>();
            item.Init(illConf.buildConfItems[i]);
            items.Add(illConf.buildConfItems[i].Name, item);
            buildings.Add(illConf.buildConfItems[i].Name, item);
        }
        for (int i = 0; i < illConf.weatherConfItems.Length; i++)
        {
            UI_IllItem item = GameObject.Instantiate(prefab, parent).GetComponent<UI_IllItem>();
            item.Init(illConf.weatherConfItems[i]);
            items.Add(illConf.weatherConfItems[i].Name, item);
            weathers.Add(illConf.weatherConfItems[i].Name, item);
        }
        Crops = tempParent.transform.Find("Crops").gameObject;
        Buildings = tempParent.transform.Find("Buildings").gameObject;
        Weathers = tempParent.transform.Find("Weathers").gameObject;
        //首先要放在Group（初始化时有组件要获取）
        //然后要全部移出来，因为显示不下
        RemoveAll();
        ShowCrop();
    }
    private void OnEnable()
    {
        ShowCrop();
        type = 0;
    }
    public void SwitchType()
    {
        switch (type) {
            case 0:
                type = 1;
                ShowBuilding();
                break;
            case 1:
                type = 2;
                ShowWeather();
                break;
            case 2:
                type = 0;
                ShowCrop();
                break;
        
        }
    }
    //添加种植次数
    public void AddCropTime(string name)
    {
        items[name].AddCropTimes();
    }
    //添加建造次数
    public void AddBuildingTime(string name)
    {
        items[name].AddBuildingTimes();
    }
    //修改售价
    public void ChangeSellGold(string name, int price)
    {
        items[name].UpdateCropPrice(price);
    }
    //全部移出来
    public void RemoveAll()
    {
        foreach (var item in crops)
        {
            item.Value.gameObject.transform.parent = Crops.transform;
        }
        foreach (var item in buildings)
        {
            item.Value.gameObject.transform.parent = Buildings.transform;
        }
        foreach (var item in weathers)
        {
            item.Value.gameObject.transform.parent = Weathers.transform;
        }
    }
    //显示植物
    private void ShowCrop()
    {
        RemoveAll();
        //将Crop的所有子物体都移动到Group下
        for(int i = 0; i < crops.Count; i++)
        {
            Crops.transform.GetChild(0).parent = parent;
        }
    }
    //显示建筑
    private void ShowBuilding()
    {
        RemoveAll();
        //将Crop的所有子物体都移动到Group下
        for (int i = 0; i < buildings.Count; i++)
        {
            Buildings.transform.GetChild(0).parent = parent;
        }
    }
    //显示天气
    private void ShowWeather()
    {
        RemoveAll();
        //将Crop的所有子物体都移动到Group下
        for (int i = 0; i < weathers.Count; i++)
        {
            Weathers.transform.GetChild(0).parent = parent;
        }
    }
}
