using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RecordPanel : MonoBehaviour
{
    public Transform grid;//档位父对象
    public GameObject recordPrefab;//档位预制体
    public GameObject recordPanel;//存档面板（控制显示隐藏）
    [Header("按钮")]
    public Button load;
    public Button save;
    public Button delete;
    [ColorUsage(true)]
    public Color oriColor;
    [Header("存档详情")]
    public GameObject detail;//存档详情
    public Text gameTime;//时长
    public Text sceneName;//所在场景
    public Text gold;//金币
    //Key:存档文件名 Value: 存档序号
    Dictionary<string, int> RecordInGrid = new Dictionary<string, int>();
    bool isSave = false;//正在存档
    bool isLoad = false;//正在读档
    private void Start()
    {
        for(int i = 0; i < RecordData.recordNum; i++)
        {
            GameObject obj = Instantiate(recordPrefab, grid);//改序号
            obj.name = (i + 1).ToString();
            obj.GetComponent<RecordUI>().SetID(i + 1);
            if (RecordData.Instance.recordName[i] != "")
            {
                obj.GetComponent<RecordUI>().SetName(i);
                RecordInGrid.Add(RecordData.Instance.recordName[i], i);
            }
        }
        #region 监听
        //RecordUI.OnLeftClick += LeftClickGrid;
        //RecordUI.OnRightClick += RightClickGrid;
        //RecordUI.OnEnter += ShowDetails;
        //RecordUI.OnExit += HideDetails;
        //open.onClick.AddListener(() => CloseOrOpen);
        //save.onClick.AddListener(() => SaveOrLoad);
        //exit.onClick.AddListener(QuitGame);
        #endregion
        //TimeMgr.SetOriTime();
    }
    private void OnDestroy()
    {
        //RecordUI.OnLeftClick -= LeftClickGrid;
        //RecordUI.OnRightClick -= RightClickGrid;
        //RecordUI.OnEnter -= ShowDetials;
        //RecordUI.OnExit -= HideDetials;
    }
    private void Update()
    {
        //TimeMgr.SetCurTime();
    }
    void ShowDetials(int i)
    {
        //var data = player.Instance.ReadForShow(i);
        //gameTime.text=  $"游戏时长{TimeMgr.GetFormatTime((int)data.gameTime)";
        //sceneName.text = $"所在场景{data.scensName}";
        //level.text = $"玩家等级{data.level}";
        //screenShot.sprite = SAVE.LoadShot(i);
        //detial.SetActive(true);
    }
    void HideDetials()
    {
        //detial.SetActive(false);
    }
    void CloseOrOpen()
    {
        recordPanel.SetActive(!recordPanel.activeSelf);
      
        save.interactable = (recordPanel.activeSelf) ? true : false;
        load.interactable = (recordPanel.activeSelf) ? true : false;
    }
    //void SaveOrLoad(bool OnSave=true)

}
