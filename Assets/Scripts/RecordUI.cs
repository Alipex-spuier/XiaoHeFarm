using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RecordUI : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Text indexText;
    public Text recordName;
    public GameObject auto;
    public Image rect;
    [ColorUsage(true)]
    public Color enterColor;

    public static System.Action<int> OnLeftClick;
    public static System.Action<int> OnRightClick;
    public static System.Action<int> OnEnter;
    public static System.Action OnExit;

    int id;
    private void Start()
    {
        id = transform.GetSiblingIndex();
            }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (OnLeftClick != null)
            {
                OnLeftClick(id);
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (OnRightClick != null)
            {
                OnRightClick(id);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.color = Color.black;
        if (recordName.text != "¿Õµµ") ;
        {
            if (OnEnter != null)
            {
                OnEnter(id);
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        rect.color = Color.white;
        if (OnExit != null)
        {
            OnExit();
        }
    }
    public void SetID(int i)
    {
        indexText.text = i.ToString();
    }
    public void SetName(int i)
    {
        if (RecordData.Instance.recordName[i] == "") 
        {
            recordName.text = "¿Õµµ";
            auto.SetActive(false);
        }
        else
        {
            string full = RecordData.Instance.recordName[i];
            string date = full.Substring(0, 8);
            string time = full.Substring(9, 6);
            //TimeMgr.SetDate(ref date);
            //TimeMgr.SetTime(ref date);
            recordName.text = date + "" + time;
            if (full.Substring(full.Length - 4) == "auto")
            {
                auto.SetActive(true);
            }
            else
            {
                auto.SetActive(false);
            }
        }
    }
}
