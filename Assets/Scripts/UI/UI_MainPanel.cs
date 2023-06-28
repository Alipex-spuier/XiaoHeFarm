using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel : MonoBehaviour
{
    private Button BuildButton;
    private Text GoldText;
    void Start()
    {
        BuildButton = transform.Find("BuildButton").GetComponent<Button>();
        GoldText = transform.Find("Gold/Text").GetComponent<Text>();
        BuildButton.onClick.AddListener(BuildButtonClick);
    }

    // 建造按钮点击
    private void BuildButtonClick() 
    {
        // 打开建造面板
        UIManager.Instance.ShowBuildPanel();
    }

    // 更新金币数量文本
    public void UpdateGoldNumText(int num)
    {
        GoldText.text = num.ToString();
    }

}
