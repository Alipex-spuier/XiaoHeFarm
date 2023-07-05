using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class illustrationConfItem
{
    // √˚◊÷
    public string Name;
    //Õº∆¨
    public Sprite sprite;
    //√Ë ˆ
    public string description;
}
[Serializable]
public class CropConfItem : illustrationConfItem
{
    public int BuildGold;
    public int SellGold;
    public int BuildTimes;
}
[Serializable]
public class BuildConfItem: illustrationConfItem
{
    public int BuildGold;
    public int BuildTimes;
}
[Serializable]
public class WeatherConfItem : illustrationConfItem
{
    public string Weather;
}

[CreateAssetMenu(fileName = "≈‰÷√Œƒº˛", menuName = "≈‰÷√/Õºº¯≈‰÷√")]
public class IllustrationConf : ScriptableObject
{
    public WeatherConfItem[] weatherConfItems;
    public CropConfItem[] cropConfItems;
    public BuildConfItem[] buildConfItems;
}
