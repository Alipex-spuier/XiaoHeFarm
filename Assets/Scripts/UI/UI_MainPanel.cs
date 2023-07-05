using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPanel : MonoBehaviour
{

    private Button BuildButton;
    private Text GoldText;
    private GameObject TaskButton;
    private GameObject ChatButton;
    private GameObject Clock;
    private GameObject ExitButton;
    private GameObject AddGoldButton;
    private GameObject IllustrationButton;
    private void Awake()
    {
     
        BuildButton = transform.Find("BuildButton").GetComponent<Button>();
        GoldText = transform.Find("Gold/Text").GetComponent<Text>();
        TaskButton = transform.Find("TaskButton").gameObject;
        ChatButton = transform.Find("ChatButton").gameObject;
        Clock = transform.Find("CurrentTime").gameObject;
        ExitButton = transform.Find("ExitButton").gameObject;
        AddGoldButton = transform.Find("AddGold").gameObject;
        IllustrationButton = transform.Find("IllustrationButton").gameObject;
        BuildButton.onClick.AddListener(BuildButtonClick);
    }
    void Start()
    {

    }

    //切换至第三人称
    public void SwitchTo3()
    {
        BuildButton.gameObject.SetActive(true);
        TaskButton.SetActive(true);
        ChatButton.SetActive(true);
        ExitButton.SetActive(true);
        IllustrationButton.SetActive(true);
        AddGoldButton.SetActive(true);
    }
    //切换至第一人称
    public void SwitchTo1()
    {
        BuildButton.gameObject.SetActive(false);
        TaskButton.SetActive(false);
        ChatButton.SetActive(false);
        ExitButton .SetActive(false);
        IllustrationButton .SetActive(false);
        AddGoldButton .SetActive(false);
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

    public void PayButtonClick()
    {
        Player_C.Instance.Gold += 50;
    }
}
