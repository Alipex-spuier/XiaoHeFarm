using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Controller : MonoBehaviour
{

    private delegate void FindWay();
    private event FindWay findWay;
    

    //设置游走半径
    public float wanderRadius = 10f;  
    private NavMeshAgent agent;
    //计时器
    private float timer;
    //寻路间隔
    private float wanderTimer;
    //随机器
    System.Random random = new System.Random();
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = 0;
        wanderTimer = random.Next(1, 10);
    }
    private void Start()
    {
        Begin();
    }
    private void Update()
    {
        if(findWay!= null)
        {
            findWay();
        }

    }
    public void Begin()
    {
        //开始处要调用一次，否则停止后没有目标位置，会卡在Work的第一行
        SetRandomDestination();
        findWay += Work;
    }
    public void Stop()
    {
        findWay -= Work;
    }
    public void Work()
    {
        //如果走到了目标位置，休息一段时间再运动
        if (agent.remainingDistance<=agent.stoppingDistance)
        {
            Stop();
            Invoke("Begin", (float)random.Next(5,10));
            Debug.Log(1);
            return;
        } else
        {
            //规定时间内没有到达位置，重新移动
            if (MyTimer.Instance.GetCurrentTime() - timer > wanderTimer)
            {

                SetRandomDestination();
                timer = MyTimer.Instance.GetCurrentTime();
                wanderTimer = random.Next(7, 13);
            }
        }
    }
    void SetRandomDestination()
    {
        do
        {
            //生成一个以NPC当前位置为中心，wanderRadius为半径的球内的随机方向向量randomDirection
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);
        }
        //如果目标位置不可达，就重新计算路径
        while (agent.pathStatus == NavMeshPathStatus.PathInvalid||agent.pathStatus == NavMeshPathStatus.PathPartial);

    }
}
