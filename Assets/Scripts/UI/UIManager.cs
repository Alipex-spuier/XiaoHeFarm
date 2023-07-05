using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]private Text UITipsText;
    [SerializeField] private Animator UITipsAnimator;
    private bool haveInventory = false;
    public bool HaveInventory { get => haveInventory; }

    private UI_MainPanel mainPanel;
    private UI_BuildPanel buildPanel;
    private UI_TaskPanel taskPanel;
    private UI_DayPanel dayPanel;
    private UI_illustrationPanel illustrationPanel;

    private void Awake()
    {
        Instance = this;
        mainPanel = transform.Find("MainPanel").GetComponent<UI_MainPanel>();
        buildPanel = transform.Find("BuildPanel").GetComponent<UI_BuildPanel>();
        taskPanel = transform.Find("TaskPanel").GetComponent<UI_TaskPanel>();
        dayPanel = transform.Find("DayPanel").GetComponent<UI_DayPanel>();
        illustrationPanel = transform.Find("illustratedPanel").GetComponent<UI_illustrationPanel>();
    }
    void Start()
    {

    }


    // 打开建造面板
    public void ShowBuildPanel() 
    {
        buildPanel.SetActive(true);
    }

    // 更新金币UI
    public void UpdateGoldUI(int num)
    {
        mainPanel.UpdateGoldNumText(num);
    }

    public void SetHaveInventory()
    {
        haveInventory = true;
    }

    public void ShowTips(string tips)
    {
        UITipsText.text = tips;
        UITipsAnimator.SetTrigger("ShowUITips");
    }

    public void ShowTaskPanel()
    {
        taskPanel.SetActive(true);
    }
    public void ShowDayPanel()
    {
        dayPanel.SetActive(true);
    }
    public void ShowIllPanel()
    {
        illustrationPanel.SetActive(true);
    }

}
