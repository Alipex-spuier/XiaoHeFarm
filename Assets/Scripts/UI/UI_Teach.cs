using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Teach : MonoBehaviour
{
    public GraphicRaycaster m_Raycaster;
    public PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    public Camera mainCamera;
    private int currentState = 0;
    private List<GameObject> teachs = new List<GameObject>();
    private void Awake()
    {
        //获取所有子物体
        for(int i = 0; i < transform.childCount; i++)
        {
            teachs.Add(transform.GetChild(i).gameObject);
        }
        //将第一步设为true
        foreach(GameObject go in teachs)
        {
            go.SetActive(false);
        }
        teachs[0].SetActive(true);

    }
    private void Update()
    {
        switch (currentState)
        {
            //此时等待点击建造按钮
            case 4:
                ClickBuildButton();
                break;
            //点击建造按钮后进入建造面板，点击建造商店的按钮
            case 5:
                ClickSelectBuildButton();
                break;
            //取消放置
            case 7:
                if(Input.GetMouseButtonDown(1))
                {
                    NextStep();
                }
                break;
            //其他只需显示，无需点击按钮的操作
            default:
                if (Input.GetMouseButtonDown(0))
                {
                    NextStep();
                }
                break;
        }

        
    }
    private void NextStep()
    {
        if (currentState < teachs.Count - 1)
        {
            teachs[currentState].SetActive(false);
            currentState = currentState + 1;
            teachs[currentState].SetActive(true);
        }
        //这里退出教程
        else
        {
            MyTimer.Instance.BeginClock();
            Destroy(this.gameObject);
        }
    }
    //点击建造按钮
    private void ClickBuildButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null && button.gameObject.name == "BuildButton")
                {
                    ExecuteEvents.Execute(button.gameObject, m_PointerEventData, ExecuteEvents.pointerClickHandler);
                    //进入下一步
                    NextStep();
                    break;
                }
            }
        }
    }
    private void ClickSelectBuildButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null && button.gameObject.transform.parent.Find("Name").GetComponent<Text>().text=="商店")
                {
                    ExecuteEvents.Execute(button.gameObject, m_PointerEventData, ExecuteEvents.pointerClickHandler);
                    //进入下一步
                    NextStep();
                    break;
                }
            }
        }
    }
}
