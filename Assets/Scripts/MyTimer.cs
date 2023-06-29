using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTimer : MonoBehaviour
{
    public static MyTimer Instance;

    public delegate void Tick();
    public event Tick tick;

    public Text currentTime;

    public float oneDay;
    public float time;
    public int day;

    private Dictionary<int,string> weatherForecast = new Dictionary<int,string>();
    private void Awake()
    {
        Instance = this;
        currentTime = transform.Find("Canvas/CurrentTime").GetComponent<Text>();
        day = 1;
        time = 0;
        BeginClock();
    }
    private void FixedUpdate()
    {
        if (tick != null)
        {
            tick();
        }
    }
    public float GetCurrentTime()
    {
        return time;
    } 
    //开始计时
    private void BeginClock()
    {
        tick += RunTheClock;
        SwitchWeather();
    }
    //停止计时
    private void StopClock()
    {
        tick -= RunTheClock;
    }
    //计时
    private void RunTheClock()
    {
        time += Time.deltaTime;
        currentTime.text = time.ToString();
        if(time>=oneDay)
        {
            StopClock();
            day++;
            time = 0;

        }
    }
    //天气切换
    private void SwitchWeather()
    {
        //前几天都是好天气
        if (day == 0)
        {
            WeatherController.Instance.CurrentWeather = "normal";
        }
        else
        {
            try
            {
                //如果今天的天气被预报过
                if (weatherForecast.ContainsKey(day))
                {
                    WeatherController.Instance.SetWeather(weatherForecast[day]);
                }
            }
            //否则随机一个天气
            catch
            {
                WeatherController.Instance.SetRandomWeather();
            }
        }
    }
    
    //天气预报
    private void AddWeatherForecast(int day,string weather)
    {
        weatherForecast.Add(day, weather);
    }

    
}
