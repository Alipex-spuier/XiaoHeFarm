using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherBase : MonoBehaviour
{
    //持有一个天空
    public Material skybox;
    //持有粒子系统
    public ParticleSystem weather;
    //持有光照
    public Light light;
    private void Awake()
    {
        try
        {
            weather = GetComponent<ParticleSystem>();
            light = GetComponentInChildren<Light>();
        }
        //普通天气没有粒子系统，手动拖
        catch
        {

        }
        
    }
    //停止当前天气
    public void Stop()
    {
        if (weather != null)
        {
            weather.Stop();
        }
        light.gameObject.SetActive(false);
    }
    //启动当前天气
    public void Begin()
    {
        //物体设置为active
        gameObject.SetActive(true);
        //更换天空盒
        RenderSettings.skybox = skybox;
        //播放粒子
        if( weather != null )
        {
            weather.Play();
        }
        //光照
        light.gameObject.SetActive(true);
    }
}
