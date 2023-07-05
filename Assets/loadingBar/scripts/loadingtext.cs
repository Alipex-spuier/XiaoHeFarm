using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingtext : MonoBehaviour {

    public static loadingtext Instance;
    public delegate void Callback( );
    public Callback callback;

    private RectTransform rectComponent;
    private Image imageComp;

    public float speed = 20f;
    public Text text;
    public Text textNormal;
    public bool isLoading = false;
    public GameObject parent;

    private void Awake()
    {
        Instance = this;
        parent.SetActive(false);
    }

    void Start () {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
    }
	
	void Update () {
        if (callback != null)
        {
            callback();
        }
    }

    private void OnEnable()
    {
        BeginLoad();
    }
    public void BeginLoad()
    {
        isLoading = true;
        callback += Loading;
    }
    public void EndLoad()
    {
        isLoading = false;
        callback -= Loading;
        parent.SetActive(false);
    }

    void Loading()
    {
        int a = 0;
        //进度条没满就继续加载
        if (imageComp.fillAmount != 1f)
        {
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
            a = (int)(imageComp.fillAmount * 100);
            if (a > 0 && a <= 33)
            {
                textNormal.text = "正在加载天气数据...";
            }
            else if (a > 33 && a <= 67)
            {
                textNormal.text = "正在保存存档...";
            }
            else if (a > 67 && a <= 100)
            {
                textNormal.text = "正在加载新场景...";
            }
            else
            {

            }
            text.text = a + "%";
        }
        //进度条满了就停止然后重置
        else
        {
            imageComp.fillAmount = 0.0f;
            text.text = "0%";
            //重新计时
            MyTimer.Instance.BeginClock();
            EndLoad();
        }
    }
}
