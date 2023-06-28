using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 商店建筑物
public class Build_Shop : BaseBuild
{
    public override float Size => 13;
    private void OnMouseDown()
    {
        if (!isPlacing)
        {
            // 打开商店
            UI_ShopPanel.Instance.SetActive(true);
        }
    }
}
