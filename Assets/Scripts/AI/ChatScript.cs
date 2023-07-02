using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatScript : MonoBehaviour
{
    //聊天UI层
    [SerializeField]private GameObject m_ChatPanel;
    //输入的信息
    [SerializeField]private InputField m_InputWord;
    //返回的信息
    [SerializeField]private Text m_TextBack;
    //gpt-3.5-turbo
    [SerializeField] public GptTurboScript m_GptTurboScript;
    //promot_Useful
    [SerializeField] private string m_lan = "使用中文回答";
/*    public WeatherController weather_C;
    private string weather;
    private bool isBk;
    private int previousDay = -1;*/



    //AI回复的信息
    private void CallBack(string _callback){
/*        if (isBk)
        {
            _callback = _callback.Trim();
            m_TextBack.text = "";
            //开始逐个显示返回的文本
            m_WriteState = true;
            StartCoroutine(SetTextPerWord(_callback));
            weather = _callback;
        }
        else
        {*/
            _callback = _callback.Trim();
            m_TextBack.text = "";
            //开始逐个显示返回的文本
            m_WriteState = true;
            StartCoroutine(SetTextPerWord(_callback));
       /* }*/
    }

    //发送信息
    public void SendData()
    {
        if (m_InputWord.text.Equals(""))
            return;

        string _msg = m_GptTurboScript.Prompt + m_lan + " " + m_InputWord.text;
        StartCoroutine(m_GptTurboScript.GetPostData(_msg,CallBack));

        m_InputWord.text = "";
        m_TextBack.text = "...";


    }

    //发送信息
    public void SendData(string _postData)
    {
        if (_postData.Equals(""))
            return;

        string _msg = m_GptTurboScript.Prompt + m_lan + " " + _postData;
        StartCoroutine(m_GptTurboScript.GetPostData(_msg, CallBack));

        m_TextBack.text = "...";


    }
    #region 文字逐个显示
    //逐字显示的时间间隔
    [SerializeField]private float m_WordWaitTime=0.2f;
    //是否显示完成
    [SerializeField]private bool m_WriteState=false;
    private IEnumerator SetTextPerWord(string _msg){
        int currentPos=0;
        while(m_WriteState){
            yield return new WaitForSeconds(m_WordWaitTime);
            currentPos++;
            //更新显示的内容
            m_TextBack.text=_msg.Substring(0,currentPos);

            m_WriteState=currentPos<_msg.Length;

        }
    }

    #endregion

/*    private void foreCastWeather()
    {
        //todo: 刚开始获取3个，然后每天获取一个
        string _msgBk = m_lan + "给我一个天气名称，范围在小雨，普通，暴雨，沙尘暴，雪，高温,并且如果你给我的天气在这个范围，就输出明天天气的情况，进行天气预报";
        isBk = true;
        StartCoroutine(m_GptTurboScript.GetPostData(_msgBk, CallBack));
        isBk = false;
        Debug.Log(weather);
        weather_C.AddWeatherForecast(MyTimer.Instance.day + 1, weather);
    }

    private void Update()
    {
        if (MyTimer.Instance.day != previousDay)
        {
                foreCastWeather();
            previousDay = MyTimer.Instance.day;
        }
    }*/

}
