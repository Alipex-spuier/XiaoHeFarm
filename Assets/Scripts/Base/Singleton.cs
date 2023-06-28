using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 单例基类
public class Singleton<T> : MonoBehaviour where T:Singleton<T>
{
    public static T Instance;
    private void Awake()
    {
        Instance = this as T;

    }
}
