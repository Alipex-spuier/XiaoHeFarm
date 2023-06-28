using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 建筑物的基类
public abstract class BaseBuild : MonoBehaviour
{
    // 当前是否在放置中
    protected bool isPlacing = true;

    public abstract float Size { get; }
    public bool CanCreate { get; set; } = false;
    protected ShopConfItem confItem;
    // 在放置的时候初始化
    public virtual void InitOnPlace(ShopConfItem confItem)
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        // 播放建造动画，结束事件为PlaceOver
        transform.DOScale(1, 2).onComplete = PlaceOver;
        this.confItem = confItem;
    }

    // 建造完毕
    private void PlaceOver()
    {
        isPlacing = false;
        OnPlaceOver();
    }

    protected virtual void OnPlaceOver() { }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlacing)
        {
            // 如果触碰到植物，不能创建
            if (other.tag != "Ground")
            {
                CanCreate = false;
            }
            else
            {
                CanCreate = true;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (isPlacing)
        {
            CanCreate = true;
        }
            
    }

}
