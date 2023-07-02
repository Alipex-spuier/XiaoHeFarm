using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeatherController : MonoBehaviour
{

    //对应天气的天空盒
    public Material[] skyboxes;
    //当前天空盒索引
    private int currentSkyboxIndex = 0; 
    //对应天气的粒子系统
    private Dictionary<string, WeatherBase> allWeather = new Dictionary<string, WeatherBase>();
    private Dictionary<int, string> randomWeather = new Dictionary<int, string>();
    private string currentWeather = "普通" ;
    //天气预报
    private Dictionary<int, string> weatherForecast = new Dictionary<int, string>();
    [SerializeField]
    public string CurrentWeather { get => currentWeather; 
        set 
        {

            //切换到特殊天气
            WeatherBase temp;
            if (allWeather.TryGetValue(value, out temp))
            {
                allWeather[currentWeather].Stop();
                currentWeather = value;
                temp.gameObject.SetActive(true);
                temp.Begin();
            }
        } 
    }
    public static WeatherController Instance { get; private set; }
    private void Awake()
    {   
        Instance = this;
        //取一级子物体，active为false的获取不到
        for(int i=0;i<transform.childCount;i++)
        {
            string name = transform.GetChild(i).gameObject.name;
            allWeather.Add(name,transform.GetChild(i).GetComponent<WeatherBase>());
            randomWeather.Add(i, name);
        }
        
        //Active设置为false
        foreach(var x in allWeather)
        {
            x.Value.gameObject.SetActive(false);
        }
        SetWeather("普通");
    }


    private void SetWeather(string name)
    {
        CurrentWeather = name;
    }
    public string GetWeather()
    {
        return CurrentWeather;
    }
    private void SetRandomWeather()
    {
        System.Random random = new System.Random();
        //取[0,4]的随机数
        int index = random.Next(0, 5);
        CurrentWeather = randomWeather[index];
    }
    //天气切换
    public void SwitchWeather()
    {
        //前几天都是好天气
        if (MyTimer.Instance.day == 0)
        {
            CurrentWeather = "普通";
        }
        else
        {
                //如果今天的天气被预报过
                if (weatherForecast.ContainsKey(MyTimer.Instance.day))
                {
                    SetWeather(weatherForecast[MyTimer.Instance.day]);
                }
                //否则随机一个天气
                else
                {
                    SetRandomWeather();
                }
        }
    }

    //天气预报
    public void AddWeatherForecast(int day, string weather)
    {
        weatherForecast.Add(day, weather);
    }
}
