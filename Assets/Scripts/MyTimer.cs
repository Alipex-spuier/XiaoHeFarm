using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTimer : MonoBehaviour
{
    int x = 0;
    private List<float> waitTime = new List<float>();
    private List<Action> actions = new List<Action>();
    private Dictionary<float, Action> scheduledActions = new Dictionary<float, Action>();
    private Dictionary<int, KeyValuePair<float, Action>> temp = new Dictionary<int, KeyValuePair<float, Action>>();

    public static MyTimer Instance;

    public delegate void Tick();
    public event Tick tick;

    public Image currentTime;


    public float oneDay;
    public float time;
    public float absoluteTime;
    public int day;


    private void Awake()
    {
        Instance = this;
        currentTime = transform.Find("ClockPoint").GetComponent<Image>();
        day = 1;
        time = 0;
        //教程结束才开始
    
    }

    [Obsolete]
    private void FixedUpdate()
    {
        //面板打开时、加载过场动画时暂停时间
        if (tick != null && Player_C.Instance.currPanel == null&&!loadingtext.Instance.isLoading&&ChatScript.Instance.m_ChatPanel.active==false)
        {
            tick();
        }
    }
    public float GetCurrentTime()
    {
        return time;
    }
    public float GetAbsoluteTime()
    {
        return absoluteTime;
    }
    //开始计时
    public void BeginClock()
    {
        tick += RunTheClock;

    }
    //停止计时
    public void StopClock()
    {
        tick -= RunTheClock;
    }
    //计时
    private void RunTheClock()
    {
        time += Time.deltaTime;
        absoluteTime += Time.deltaTime;
        currentTime.fillAmount = time / oneDay;
        if (time >= oneDay)
        {
            day++;
            time = 0;
            //显示结算面板，进行结算
            UIManager.Instance.ShowDayPanel();
        }
        // 检查是否有已经到达指定时间的函数需要执行
        List<int> keysToRemove = new List<int>();
        foreach (var pair in temp)
        {
            if (absoluteTime >= pair.Value.Key)
            {
                pair.Value.Value.Invoke(); // 执行函数
                keysToRemove.Add(pair.Key); // 添加到待移除列表
            }
        }

        // 移除已经执行的函数
        foreach (int key in keysToRemove)
        {
            temp.Remove(key);
        }
    }
    public void ScheduleAction(float delay, Action action)
    {
        float scheduledTime = GetAbsoluteTime() + delay;
        //scheduledActions.Add(scheduledTime, action);
        temp.Add(x++, new KeyValuePair<float, Action>(scheduledTime, action));
    }

}




