using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Generator : MonoBehaviour
{
    public NPC_Generator Instance;
    public Transform transform;
    private void Awake()
    {
        Instance = this;
    }
    //切换到第一人称
    public void SwitchTo1(Transform transform)
    {

    }
    //切换到第三人称
    public void SwitchTo3(Transform transform)
    {

    }
}
