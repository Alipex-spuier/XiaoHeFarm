using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 仓库的建筑物
public class Build_Inventory : BaseBuild
{
    public override float Size => 13;
    private void OnMouseDown()
    {
        if (!isPlacing)
        {
            // 打开仓库
            UI_InventoryPanel.Instance.SetActive(true);
        }
    }

    protected override void OnPlaceOver()
    {
        base.OnPlaceOver();
        UIManager.Instance.SetHaveInventory();
    }
}
