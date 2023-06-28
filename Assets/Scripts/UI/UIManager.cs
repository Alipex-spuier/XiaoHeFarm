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

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        mainPanel = transform.Find("MainPanel").GetComponent<UI_MainPanel>();
        buildPanel = transform.Find("BuildPanel").GetComponent<UI_BuildPanel>();
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
}
