using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;

public class ChatScript : MonoBehaviour
{
    public GameObject quesImg;
    public static ChatScript Instance; 
    //聊天UI层
    [SerializeField]public GameObject m_ChatPanel;
    //输入的信息
    [SerializeField]private InputField m_InputWord;
    //返回的信息
    [SerializeField]private Text m_TextBack;
    //gpt-3.5-turbo
    [SerializeField] public GptTurboScript m_GptTurboScript;
    //promot_Useful
     private string m_lan = "使用中文回答，字数在30字以内,";
    public WeatherController weather_C;
    private int previousDay = -1;

    public void Awake()
    {
        Instance = this;
    }
    public class TaskData
    {
        public string taskDescription { get; set; }
        public string need { get; set; }
        public int needCount { get; set; }
        public string rewardType { get; set; }
        public string reward { get; set; }
    }
    //AI回复的信息
    private void CallBack(string _callback,string callbackType){
            _callback = _callback.Trim();
        if (callbackType == "normal"||callbackType== "weatherForcast")
        {
            m_TextBack.text = "";
            //开始逐个显示返回的文本
            m_WriteState = true;
            StartCoroutine(SetTextPerWord(_callback));
        }
        if (callbackType == "task")
        {
            TaskData taskData = JsonConvert.DeserializeObject<TaskData>(_callback);
            UI_TaskPanel.Instance.CreateTask(taskData.taskDescription, taskData.need, taskData.needCount, taskData.rewardType, taskData.reward);
            m_TextBack.text = "...";
        }
        else if (callbackType == "weather")
        {
            weather_C.AddWeatherForecast(MyTimer.Instance.day + 1, _callback);
            m_TextBack.text = "...";
            callbackType = "weatherForcast";
        }
        if (callbackType == "weatherForcast")
        {
            if (previousDay != MyTimer.Instance.day) 
            {
                string _msgWeather = m_lan + "今天的天气是" + weather_C.GetWeather() + ",明天的天气是" + _callback + ",请对天气进行天气预报。";
                StartCoroutine(m_GptTurboScript.GetPostData(_msgWeather, CallBack, "weatherForcast"));
                previousDay = MyTimer.Instance.day;
                quesImg.gameObject.SetActive(true);
            }
        }
            
    }
    //发送信息
    public void SendData()
    {
        if (m_InputWord.text.Equals(""))
            return;

        string _msg = m_GptTurboScript.Prompt + m_lan + " " + m_InputWord.text;
        StartCoroutine(m_GptTurboScript.GetPostData(_msg,CallBack,"normal"));

        m_InputWord.text = "";
        m_TextBack.text = "...";


    }

    //发送信息
    public void SendData(string _postData)
    {
        if (_postData.Equals(""))
            return;

        string _msg = m_GptTurboScript.Prompt + m_lan + " " + _postData;
        StartCoroutine(m_GptTurboScript.GetPostData(_msg, CallBack, "normal"));

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

    public void foreCastWeather()
    {
        string _msgWeather = m_lan + "给我一个天气名称，范围在小雨，普通，暴雨，沙尘暴，雪，高温。";
        StartCoroutine(m_GptTurboScript.GetPostData(_msgWeather, CallBack, "weather"));
    }
    public void createTask()
    {
        string _msgTask = m_lan + "给我1个任务面板的数据，数据为：除needCount外全为string，范围为：need=[向日葵，苹果，小麦]，needCount=(1,30),rewardType=[建筑物，Gold],reward={[建筑物：商店],[建筑物,仓库],Gold(50,300)},要求格式为：{taskDescription:'xx',need:'xx',needCount:xx,rewardType:'xx',reward:'xx'}，xx为要填入的内容，,任务描述部分除了我的模板内容，你还需要自己编一个小故事添加进去,例如为{taskDescription: '你被任命为村庄的首席园艺师，现在面临着一个紧急任务。近日，村庄的向日葵园里遭受了一次严重的蝗虫袭击，大部分向日葵都被吃掉了。为了拯救村庄的面貌，你需要紧急种植15个向日葵。只有通过你的努力，才能让村庄恢复到美丽的景象。完成任务后，你将获得200个金币作为奖励，以表彰你的努力和对村庄的贡献。', need: '向日葵', needCount: 15, rewardType: 'Gold', reward: '200'}";
        StartCoroutine(m_GptTurboScript.GetPostData(_msgTask, CallBack, "task"));
    }

}
