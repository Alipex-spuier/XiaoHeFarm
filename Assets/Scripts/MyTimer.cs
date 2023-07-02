using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyTimer : MonoBehaviour
{
    private List<float> waitTime = new List<float>();
    private List<Action> actions = new List<Action>();
    //private Dictionary<float, Action> scheduledActions = new Dictionary<float, Action>();


    public static MyTimer Instance;

    public delegate void Tick();
    public event Tick tick;

    public Text currentTime;

    public float oneDay;
    public float time;
    public int day;


    private void Awake()
    {
        Instance = this;
        currentTime = GetComponent<Text>();
        day = 1;
        time = 0;
        BeginClock();
    }
    private void FixedUpdate()
    {
        //面板打开时暂停时间
        if (tick != null&& Player_C.Instance.currPanel == null)
        {
            tick();
        }
    }
    public float GetCurrentTime()
    {
        return time;
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
        currentTime.text = time.ToString();
        if(time>=oneDay)
        {
            day++;
            time = 0;
            //显示结算面板，进行结算
            UIManager.Instance.ShowDayPanel();
        }
        // 检查是否有已经到达指定时间的函数需要执行
        List<float> temp = new List<float>();
        int k = waitTime.Count;
        for(int i=0;i<k;i++)
        {
            if (time >= waitTime[i])
            {
                actions[i].Invoke(); // 执行函数
                waitTime.RemoveAt(i);
                actions.RemoveAt(i);
                k = waitTime.Count;
            }
        }

    }
    public void ScheduleAction(float delay, Action action)
    {
        float scheduledTime = GetCurrentTime() + delay;
        waitTime.Add(scheduledTime);
        actions.Add(action);
        //scheduledActions.Add(scheduledTime, action);
    }

}
