using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
                    break;
                case CropState.Grow:
                    imgHand.SetActive(false);
                    imgSeed.SetActive(false);
                    InitGrow();
                    break;
                case CropState.Ripe:
                    StopCoroutine("DoGrow");
                    imgHand.SetActive(true);
                    imgSeed.SetActive(false);
                    break;
            }
        }
    }
    private void Start()
    {
        Lv = 0;
        imgHand.SetActive(false);
        imgSeed.SetActive(false);
    }
    protected override void OnPlaceOver()
    {
        // 默认有种子
        CropState = CropState.Grow;
    }
    private void InitGrow()
    {
        StopCoroutine("DoGrow");
        StartCoroutine("DoGrow");
    }
    IEnumerator DoGrow()
    {
        for (int i = 0; i < lvPrefabs.Length; i++)
        {
            yield return new WaitForSeconds(10);
            UpGrade();
        }
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
                break;
            case CropState.Ripe:
                if (UIManager.Instance.HaveInventory)
                {
                    // 进仓库
                    UI_InventoryPanel.Instance.AddCrop(cropName, 1);
                    CropState = CropState.Empty;
                }
                else
                {
                    // 提醒
                    UIManager.Instance.ShowTips("您需要一个仓库后才能收割作物！");
                }

                break;
        }
    }
}
