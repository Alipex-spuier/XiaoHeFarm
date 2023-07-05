using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_ShopPanelBase<T> : UI_ListPanelBase<T> where T:UI_ShopPanelBase<T>
{
    // 商店的全部配置
    [SerializeField] ShopConf shopConf;

    protected override void OnStart()
    {
        // 创建全部选项，并且初始化数值
        for (int i = 0; i < shopConf.ShopConfItems.Length; i++)
        {
            UI_ShopItem item = GameObject.Instantiate(prefab_Item, parent_Item).GetComponent<UI_ShopItem>();
            item.Init(shopConf.ShopConfItems[i], CloseButtonClick);
            //UI_DayPanel.Instance.BuildBuilding(confItem.Name);
        }
    }


}
