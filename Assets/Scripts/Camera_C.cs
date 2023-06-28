using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camer_C : MonoBehaviour
{
    private Camera m_Camera;
    private GameObject CM_C;
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
        m_Camera = GetComponentInChildren<Camera>();
        CM_C = m_Camera.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        CheckBorder();
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
