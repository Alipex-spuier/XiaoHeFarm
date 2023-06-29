using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeatherController : MonoBehaviour
{
    private Dictionary<string, ParticleSystem> allWeather = new Dictionary<string, ParticleSystem>();
    private Dictionary<int, string> randomWeather = new Dictionary<int, string>();
    private Queue<string> weatherForecast = new Queue<string>();
    private string currentWeather = "normal" ;
    [SerializeField]
    public string CurrentWeather { get => currentWeather; 
        set 
        {
            //切换回普通天气
            if (value == "normal")
            {
                try
                {
                    //如果当前不是普通天气
                    allWeather[currentWeather].Stop(true);
                    currentWeather = "normal";
                }
                catch
                {
                    //如果当前是普通天气
                }
                return;
            }
            //切换到特殊天气
            ParticleSystem temp;
            if (allWeather.TryGetValue(value, out temp))
            {
                //如果此时已有天气，取消当前天气
                if (currentWeather != null&&currentWeather!="normal")
                {
                    allWeather[currentWeather].Stop(true);
                }
                currentWeather = value;
                temp.gameObject.SetActive(true);
                temp.Play();
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
            allWeather.Add(name,transform.GetChild(i).GetComponent<ParticleSystem>());
            randomWeather.Add(i, name);
        }
        
        //Active设置为false
        foreach(var x in allWeather)
        {
            x.Value.gameObject.SetActive(false);
        }
    }


    public void SetWeather(string name)
    {
        CurrentWeather = name;
    }
    public string GetWeather()
    {
        return CurrentWeather;
    }
    public void SetRandomWeather()
    {
        System.Random random = new System.Random();
        //取[0,4]的随机数
        int index = random.Next(0, 5);
        CurrentWeather = randomWeather[index];
    }
}
