using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 植物状态
public enum CropState
{
    // 空的,需要种子
    Empty,
    // 生长中
    Grow,
    // 成熟，需要采摘
    Ripe
}

public abstract class CropBase : BaseBuild
{
    [SerializeField] private string cropName;
    [SerializeField]
    private CropState cropState;
    public GameObject imgHand;
    public GameObject imgSeed;
    public GameObject imgWater;
    public string weather;
    private float waterState;
    public static int[] exp = { 0, 0 };
    private int actualCropNum;
    private float growingTime;//植物状态发生变化的时间
    // 等级
    [SerializeField]
    private int lv = -1;
    // 不同等级的预制体
    [SerializeField]
    private GameObject[] lvPrefabs;
    // 模型
    private GameObject model;
    public int Lv
    {
        get => lv;
        set
        {
            // 传进来的值和现在的不一样
            if (lv != value)
            {
                lv = value;
                // 如果成熟
                if (lv == lvPrefabs.Length - 1)
                {
                    CropState = CropState.Ripe;
                }
                // 修改我的模型
                //  TODO:待用对象池替代 模型实例化以及销毁        
                if (model != null) Destroy(model);
                model = GameObject.Instantiate(lvPrefabs[Lv], transform); 
                if (Lv == 1)
                {
                    if (WeatherController.Instance.GetWeather() != "小雨" && WeatherController.Instance.GetWeather() != "暴雨")
                    { 
                        imgWater.SetActive(true);
                    }//若天气不为雨天，则显示缺水，并且要求进行浇水操作
                    else {
                        Water();
                        imgWater.SetActive(false);
                        DoGrow();//若天气为雨，则生长至第二阶段的作物自动进行浇水，并显示已经浇过水，作物自动生长
                    }
                }
            }
        }
    }
    public CropState CropState
    {
        get => cropState;
        set
        {
            cropState = value;
            switch (cropState)
            {
                case CropState.Empty:
                    Lv = 0;
                    imgHand.SetActive(false);
                    imgSeed.SetActive(true);
                    imgWater.SetActive(false);
                    break;
                case CropState.Grow:
                    imgHand.SetActive(false);
                    imgSeed.SetActive(false);
                    imgWater.SetActive(false);
                    InitGrow();
                    break;
                case CropState.Ripe:
                    imgHand.SetActive(true);
                    imgSeed.SetActive(false);
                    imgWater.SetActive(false);
                    break;
            }
        }
    }

    private void Start()
    {
        Lv = 0;
        imgHand.SetActive(false);
        imgSeed.SetActive(false);
        imgWater.SetActive(false);
        weather = WeatherController.Instance.GetWeather();
    }
    protected override void OnPlaceOver()
    {
        // 默认有种子
        CropState = CropState.Grow;
        UI_DayPanel.Instance.BuildCrop(cropName);
        
    }
    private void InitGrow()
    {
        DoGrow();
    }
    private void DoGrow()
    {
        //MyTimer.Instance.ScheduleAction(1f, LoseWater);
        MyTimer.Instance.ScheduleAction(5f, UpGrade);
    }
    // 升级
    private void UpGrade()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        // 播放建造动画，结束事件为PlaceOver
        transform.DOScale(1, 0.8f).onComplete = OnUpGradeOver;
    }
    // 在升级结束时候，更换模型
    private void OnUpGradeOver()
    {
        Lv++;
    }

    private void OnMouseDown()
    {
        if (isPlacing) return;

        switch (CropState)
        {
            case CropState.Empty:
                if (Player_C.Instance.Gold> confItem.Gold / 2)
                {
                    Player_C.Instance.Gold -= confItem.Gold / 2;
                    // 播种
                    CropState = CropState.Grow;
                }
                else
                {
                    UIManager.Instance.ShowTips("金币不足！");
                }
                
                break;
            case CropState.Grow:
                if (Lv == 1 && waterState == 0)//生长进入第二阶段且未浇过水
                {
                    Water();//点击作物进行浇水
                    imgWater.SetActive(false);//缺水图标消失
                    transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    transform.DOScale(1, 0.8f);//播放交互成功的动画
                    DoGrow();//作物继续生长
                    return;
                }
                else if(Lv == 1 && waterState == 1)
                {
                    transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    transform.DOScale(1, 0.8f);//播放交互成功的动画
                    return;
                }
                break;
            case CropState.Ripe:
                if (UIManager.Instance.HaveInventory)
                {
                    // 进仓库
                    
                    if (cropName == "苹果")
                    {
                        switch (WeatherController.Instance.GetWeather())
                        {
                            case "普通":
                                actualCropNum = Crop_Apple.Production;
                                break;
                            case "雪":
                            case "小雨":
                                actualCropNum = Crop_Apple.Production * 11 / 10;
                                break;
                            case "暴雨":
                            case "高温":
                            case "沙尘暴":
                                actualCropNum = Crop_Apple.Production * 4 / 5;
                                break;
                        }
                        if (exp[0] < 5)
                        {
                            exp[0]++;
                        }
                    }
                    else if(cropName == "向日葵")
                    {
                        switch (WeatherController.Instance.GetWeather())
                        {
                            case "普通":
                                actualCropNum = Crop_Sunflower.Production;
                                break;
                            case "小雨":
                                actualCropNum = Crop_Sunflower.Production * 3 / 5;
                                break;
                            case "暴雨":
                                actualCropNum = Crop_Sunflower.Production * 1 / 5;
                                break;
                            case "高温":
                            case "沙尘暴":
                            case "雪":
                                actualCropNum = Crop_Sunflower.Production * 8 / 5;
                                break;
                        }
                        if (exp[1] < 10)
                        {
                            exp[1]++;
                        }
                    }
                    else if (cropName == "小麦")
                    {
                        switch (WeatherController.Instance.GetWeather())
                        {
                            case "普通":
                                actualCropNum = Crop_Wheat.Production;
                                break;
                            case "雪":
                            case "小雨":
                            case "高温":
                            case "沙尘暴":
                                actualCropNum = Crop_Wheat.Production * 6 / 5;
                                break;
                            case "暴雨":
                                actualCropNum = Crop_Wheat.Production * 1 / 5;
                                break;
                        }
                        if (exp[1] < 10)
                        {
                            exp[1]++;
                        }
                    }
                    UI_DayPanel.Instance.GetCrop(cropName, actualCropNum);
                    UI_InventoryPanel.Instance.AddCrop(cropName, actualCropNum);
                    CropState = CropState.Empty;
                    waterState = 0;
                }
                else
                {
                    // 提醒
                    UIManager.Instance.ShowTips("您需要一个仓库后才能收割作物！");
                }

                break;
        }
    }

    public static int GetExp(int i)
    {
        return exp[i];
    }
    private void StopGrowing()
    {

    }//缺水时停止生长
    private void Water()
    {
        waterState = 1;
    }//浇水
    /*private void LoseWater()
    {
        if(waterState >= 30)
        {
            waterState -= 30;
        }
        else
        {
            StopGrowing();
            waterState -= 30;
        }
    }*/

}
