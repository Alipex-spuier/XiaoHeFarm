using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_ShopItem : MonoBehaviour
{
    private Text nameText;
    private Text countText;
    private Text moneyText;
    private Button buildButton;

    public UnityAction BuildButtonClickAction;

    // 当前Item 的配置
    public ShopConfItem confItem;
    // 当前建造的数量
    private int currCount;

    public int CurrCount { get => currCount; }
    public int MaxCount { get => confItem.MaxCount; }
    public int Gold { get => confItem.Gold; }
    public GameObject Prefab { get => confItem.Prefab; }
    public bool CanBuild
    {
        get
        {
            //需要校验金币
            if (Gold<=Player_C.Instance.Gold && CurrCount < MaxCount)
            {
                return true;
            }

            return false;
        }
    }

    public void Init(ShopConfItem confItem,UnityAction buildButtonClick)
    {
        nameText = transform.Find("Name").GetComponent<Text>();
        countText = transform.Find("Count").GetComponent<Text>();
        moneyText = transform.Find("Money").GetComponent<Text>();
        buildButton = transform.Find("Button").GetComponent<Button>();
        buildButton.onClick.AddListener(BuildButtonClick);

        this.confItem = confItem;
        currCount = 0;
        nameText.text = confItem.Name;
        countText.text = currCount + "/" + confItem.MaxCount.ToString();
        moneyText.text = confItem.Gold.ToString();

        


        BuildButtonClickAction = buildButtonClick;
    }



    // 建造按钮点击
    private void BuildButtonClick()
    {
        // 不能建造
        if (!CanBuild )
        {
            return;
        }

        // 发起建造
        Player_C.Instance.Build(this);
        // 关闭面板
        BuildButtonClickAction();
    }

    // 更新建造数据
    public void UpdateBuidlData()
    {
        currCount += 1;
        countText.text = currCount + "/" + confItem.MaxCount.ToString();
        if(confItem.Name=="商店"||confItem.Name=="仓库")
        UI_DayPanel.Instance.BuildBuilding(confItem.Name);
    }
}
