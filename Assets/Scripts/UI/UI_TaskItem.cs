using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TaskItem : MonoBehaviour
{
    [Header("UI组件")]
    private Text UI_need;
    private Text UI_reward;
    private Button UI_submit;
    [Header("参数")]
    //任务描述
    public string taskDescription;
    //作物名称
    private string need;
    //需求量
    private int needCount;
    //奖励类型
    private string rewardType;
    //奖励
    private string reward;
    public void Init(string taskDescription,string need,int needCount,string rewardType,string reward)
    {
        UI_need = transform.Find("需求").GetComponent<Text>();   
        UI_reward = transform.Find("奖励").GetComponent<Text>();
        UI_submit = transform.Find("提交").GetComponent<Button>();

        this.taskDescription = taskDescription;
        this.need = need;
        this.needCount = needCount;
        this.rewardType = rewardType;
        this.reward = reward;

        //修改奖励
        if (rewardType == "Gold")
        {
            UI_reward.text = $"{reward}金币";
        }
        else if (rewardType == "Building")
        {
            UI_reward.text = $"{reward}";
        }
        UpdateCount();
    }
    //检查仓库内当前作物的数量
    public void UpdateCount()
    {
        //如果没有仓库
        int num;
        if (UI_InventoryPanel.Instance == null)
        {
            num = 0;
        } else
        {
            num = UI_InventoryPanel.Instance.GetCount(need); 
        }
        //修改数量
        UI_need.text = $"{need}"+num.ToString()+"/"+needCount.ToString();
        //检查是否满足需求
        if (num>=needCount)
        {
            UI_submit.interactable = true;
        }
        else
        {
            UI_submit.interactable = false;
        }
    }

    //显示任务描述
    public void ShowDetail()
    {
        Debug.Log(1);
        UI_TaskPanel.Instance.ShowDescription(taskDescription);
    }
    //提交任务
    public void SubmitTask()
    {
        //修改仓库容量
        UI_InventoryPanel.Instance.AddCrop(need,-needCount);
        //获取奖励
        //奖励有两种：金币，建筑
        if (rewardType == "Gold")
        {
            GetGold();
        } else if(rewardType == "Building")
        {
            GetBuilding();
        }
        Destroy(this.gameObject);
    }
    //获取奖励
    public void GetGold()
    {
        //奖励的金币加上售卖作物的价格
        Player_C.Instance.Gold += (Int32.Parse(reward) + (10 * needCount));
    }
    public void GetBuilding()
    {
        //
    }
}
