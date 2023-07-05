using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IllItem : MonoBehaviour
{
    //描述面板
    private Text description;
    //相关信息
    private illustrationConfItem myConf;


    public void Init(illustrationConfItem conf)
    {
        gameObject.name = conf.Name;
        myConf = conf;
        transform.Find("名字").GetComponent<Text>().text = conf.Name;
        transform.Find("图片").GetComponent<Image>().sprite= conf.sprite;
        description = transform.parent.parent.parent.Find("IllustratedDescription/Text").GetComponent<Text>();
    }
    public void ShowDes()
    {
        //判断myConf的类型
        if (myConf.GetType() == typeof(CropConfItem))
        {
            var temp = (CropConfItem)myConf;
            description.text = "";
            description.text += $"建造价格:{temp.BuildGold}"+"\n";
            description.text += $"售卖价格:{temp.SellGold}" + "\n";
            description.text += $"种植次数:{temp.BuildTimes}" + "\n";
            description.text += temp.description;
        } else if (myConf.GetType() == typeof(BuildConfItem))
        {
            var temp = (BuildConfItem)myConf;
            description.text = "";
            description.text += $"建造价格:{temp.BuildGold}" + "\n";
            description.text += $"建造次数:{temp.BuildTimes}" + "\n";
            description.text += temp.description;
        } else if(myConf.GetType() == typeof(WeatherConfItem))
        {
            var temp = (WeatherConfItem)myConf;
            description.text = "";
            description.text += temp.description;
        }
        
    }

    public void UpdateCropPrice(int price) {
        var temp = (CropConfItem)myConf;
        temp.BuildGold = price;
    }
    public void AddBuildingTimes()
    {
        var temp = (BuildConfItem)myConf;
        temp.BuildTimes++;
    }
    public void AddCropTimes()
    {
        var temp = (CropConfItem)myConf;
        temp.BuildTimes++;
    }
}
