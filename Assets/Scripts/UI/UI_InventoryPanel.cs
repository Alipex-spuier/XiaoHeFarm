using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UI_InventoryPanel : UI_ListPanelBase<UI_InventoryPanel>
{
    public Player_C player;

    private Dictionary<string, int> cropDic = new Dictionary<string, int>();

    private List<UI_InventoryItem> itemList = new List<UI_InventoryItem>();

    protected override void OnStart()
    {
        // 显示仓库中的所有物品列表以及对应的数量
        UpdateItems();
    }

    private void UpdateItems()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i].gameObject);
        }
        itemList.Clear();
        foreach (var item in cropDic)
        {
            UI_InventoryItem inventoryItem = Instantiate(prefab_Item, parent_Item).GetComponent<UI_InventoryItem>();
            inventoryItem.Init(item.Key, item.Value);
            itemList.Add(inventoryItem);
        }
    }

    public void AddCrop(string cropName,int count)
    {
        if (cropDic.ContainsKey(cropName))
        {
            cropDic[cropName] += count;
        }
        else
        {
            cropDic.Add(cropName, count);
        }
        UpdateItems();
        //需要通知任务面板刷新
        UI_TaskPanel.Instance.UpdateTaskItem();
    }

    public void SellCrop(string cropName,int count)
    {
        if (cropDic.ContainsKey(cropName) && cropDic[cropName]  >= count)
        {
            //GameObject crop = GameObject.Find("cropName");
            if (cropName == "向日葵")
            {
                Player_C.Instance.Gold += count * Crop_Sunflower.Price;
            }
            else if (cropName == "苹果")
            {
                Player_C.Instance.Gold += count * Crop_Apple.Price;
            }
        }
        else if (cropDic[cropName] < count)
        {
            UIManager.Instance.ShowTips("存量不足！");
        }
    }

    public void SellButtonClick()
    {
        foreach (var item in cropDic)
        {
            SellCrop(item.Key, item.Value);
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i].gameObject);
        }
        cropDic.Clear();
        itemList.Clear();
    }

    //返回数量
    public int GetCount(string cropName)
    {
        try
        {
            return cropDic[cropName];
        }
        catch
        {
            return 0;
        }
    }

}
