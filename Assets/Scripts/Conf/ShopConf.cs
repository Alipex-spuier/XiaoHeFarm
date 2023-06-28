using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ShopConfItem
{
    // 名字
    public string Name;
    // 数量限制
    public int MaxCount;
    // 金币
    public int Gold;
    // 预制体
    public GameObject Prefab;
}


[CreateAssetMenu(fileName ="配置文件",menuName ="配置/商店配置")]
public class ShopConf : ScriptableObject
{
    public ShopConfItem[] ShopConfItems;
}
