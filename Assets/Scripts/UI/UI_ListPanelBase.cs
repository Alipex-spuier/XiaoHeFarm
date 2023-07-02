using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IPanel
{ }

// UI列表面板型基类
public class UI_ListPanelBase<T> : Singleton<T>, IPanel where T: UI_ListPanelBase<T>
{
    [SerializeField] protected Button CloseButton;
    // UI元素的父物体
    [SerializeField] protected Transform parent_Item;

    // UI预制体
    [SerializeField] protected GameObject prefab_Item;

    private void Start()
    {
        CloseButton.onClick.AddListener(CloseButtonClick);
        CloseButtonClick();
        OnStart();
    }

    protected virtual void OnStart() { }


    // 关闭按钮点击
    protected virtual void CloseButtonClick()
    {
        Player_C.Instance.currPanel = null;
        // 关闭自身
        SetActive(false);
    }

    // 修改显示
    public void SetActive(bool isShow)
    {

        if (isShow)
        {
            //如果当前没有面板被打开，就打开面板
            if (Player_C.Instance.currPanel == null)
            {
                Player_C.Instance.currPanel = this;
                gameObject.SetActive(isShow);
            }
            //否则什么也不做
        }
        else
        {
            gameObject.SetActive(isShow);
        }

    }
}
