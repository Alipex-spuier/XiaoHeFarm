using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_InventoryItem : MonoBehaviour
{
    private Text nameText;
    private Text countText;

    public void Init(string cropName,int count)
    {
        nameText = transform.Find("Name").GetComponent<Text>();
        countText = transform.Find("Count").GetComponent<Text>();
        nameText.text = cropName;
        UpdateCount(count);
    }

    public void UpdateCount(int count)
    {
        countText.text = count.ToString();
    }
}
