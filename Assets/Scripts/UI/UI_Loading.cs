using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Loading : MonoBehaviour
{
    public void SwitchWeather()
    {
        WeatherController.Instance.SwitchWeather();
        UI_DayPanel.Instance.weather.text = "当前天气：" + WeatherController.Instance.GetWeather();
    }
}
