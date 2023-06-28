using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_TaskPanel : UI_ListPanelBase<UI_TaskPanel>
{
    public GameObject mainTask;
    //所有任务
    private List<UI_TaskItem> itemList = new List<UI_TaskItem>();
    //任务描述
    public Text taskDescription;
    private void Awake()
    {
        Instance = this;
        mainTask = transform.Find("MainTask").gameObject;
        taskDescription = transform.Find("TaskDescription/Text").GetComponent<Text>();
        taskDescription.text = "";
    }
    //打开任务面板
    public void TaskButtonClick()
    {
        // 打开任务面板
        UIManager.Instance.ShowTaskPanel();
    }
    //测试用
    public void CreateTask()
    {
        CreateTask("aksjdfjaskfdjjaskjdfkjsadlfjalskdf", "向日葵", 15, "Gold", "25");
    }
    //创建任务
    public void CreateTask(string taskDescription, string need, int needCount, string rewardType, string reward)
    {
        //传入任务描述，需求，需求量，奖励类型(Building/Gold)，奖励内容(建筑名/金币数量）
        UI_TaskItem taskItem = Instantiate(prefab_Item, parent_Item).GetComponent<UI_TaskItem>();
        taskItem.Init(taskDescription, need, needCount, rewardType, reward);
        itemList.Add(taskItem);
    }
    //显示任务描述
    public void ShowDescription(string detail)
    {
        taskDescription.text = detail;
    }
    //刷新任务
    public void UpdateTaskItem()
    {
        foreach(var x in  itemList)
        {
            x.UpdateCount();
        }
    }
}
