using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camer_C : MonoBehaviour
{


    public Camera m_Camera;//第三人称
    private GameObject mainCamera;
    public Camera p_Camera;//第一人称
    private GameObject playerCamera;
    public Transform  player;
    public GameObject m_mainPanel;
    private float mouseX, mouseY;
    public float mouseSensitivity;
    private bool isFirstPerson = false;
    public float xRotation;
    //左右移动
    public float xMove;
    //左右移动边界
    public Vector2 borderX;
    //前后移动边界
    public Vector2 borderZ;
    //前后移动
    public float zMove;
    //鼠标滚轮
    public float mouseScrollWheel;
    //移动速度
    public float moveSpeed;
    public float dashSpeed;
    //缩放尺度
    public float scale;



    void Start()
    {
        // 初始化时启用第三人称摄像机，禁用第一人称摄像机
        p_Camera.enabled = false;
        m_Camera.enabled = true;

    }

    void FixedUpdate()
    {
        // 检测键盘上的"v"键是否被按下
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
            if (isFirstPerson)
            {
                // 切换为第一人称
                player.gameObject.SetActive(true);
                p_Camera.enabled = true;
                m_Camera.enabled = false;
                m_mainPanel.SetActive(false);
            }
            else
            {
                // 切换为第三人称
                //p_Camera.enabled = false;
                m_Camera.enabled = true;
                m_mainPanel.SetActive(true);
                transform.localRotation = Quaternion.Euler(45, 0, 0);
                player.gameObject.SetActive(false);
            }
        }
        // 根据当前人称执行对应的方法
        if (isFirstPerson)
        {
            playerMove();
        }
        else
        {
            Move();
            CheckBorder();
        }
    }



    private void playerMove()
    {
        mouseX=Input.GetAxisRaw("Mouse X")*mouseSensitivity*Time.deltaTime;
        mouseY=Input.GetAxisRaw("Mouse Y")*mouseSensitivity*Time.deltaTime;

        p_Camera.transform.Rotate(-mouseY, 0, 0);
        //水平是0，往上一点就到了360，往下一点就到了1
        if(p_Camera.transform.localEulerAngles.x<300&& p_Camera.transform.localEulerAngles.x > 200)
        {
            p_Camera.transform.localEulerAngles = new Vector3(300, 0, 0);
        }
        if(p_Camera.transform.localEulerAngles.x>50&& p_Camera.transform.localEulerAngles.x < 200)
        {
            p_Camera.transform.localEulerAngles = new Vector3(50, 0, 0);
        }


        player.Rotate(Vector3.up * mouseX);
    }
    private void Move()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        zMove = Input.GetAxisRaw("Vertical");
        mouseScrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");
        //移动摄像机
        Vector3 dir = new Vector3(xMove, 0, zMove);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += dir * Time.deltaTime * dashSpeed;
        }
        else
        {
            transform.position += dir * Time.deltaTime * moveSpeed;
        }

        //缩放视角
        if (mouseScrollWheel > 0 && m_Camera.fieldOfView <= 60)
        {
            m_Camera.fieldOfView -= mouseScrollWheel * scale;
        }
        else if (mouseScrollWheel < 0 && m_Camera.fieldOfView >= 10)
        {
            m_Camera.fieldOfView -= mouseScrollWheel * scale;
        }
        if (m_Camera.fieldOfView > 60)
        {
            m_Camera.fieldOfView = 60;
        }
        if (m_Camera.fieldOfView < 10)
        {
            m_Camera.fieldOfView = 10;
        }
    }
    private void CheckBorder()
    {
        if (transform.position.x < borderX.x)
        {
            transform.position = new Vector3(borderX.x, transform.position.y, transform.position.z);
        }
        if (transform.position.x > borderX.y)
        {
            transform.position = new Vector3(borderX.y, transform.position.y, transform.position.z);

        }
        if (transform.position.z < borderZ.x)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, borderZ.x);

        }
        if (transform.position.z > borderZ.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, borderZ.y);

        }
    }
}
