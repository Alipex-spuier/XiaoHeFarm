using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_C : MonoBehaviour
{
    public static Player_C Instance;
    private CharacterController characterController;
    public float movespeed;
    public float jumpSpeed;
    private float horizontalMove,verticalMove;
    private Vector3 dir;
    public float gravity;
    private Vector3 velocity;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    public bool isGround ;
    // 全部建筑物
    private List<BaseBuild> buildList = new List<BaseBuild>();

    // 临时持有的建筑物模型
    private BaseBuild tempBuild;
    private GameObject tempBuildPrefab;

    public IPanel currPanel=null;

    // 金币
    private int gold;
    public int Gold { get => gold;
        set {
            gold = value;
            // 更新UI
            UIManager.Instance.UpdateGoldUI(gold);
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Gold = 500;
    }
    void Update()
    {   
        isGround=Physics.CheckSphere(groundCheck.position,checkRadius,groundLayer);
        if (isGround &&velocity.y<0)
        {
            velocity.y = 0f;
        }
        horizontalMove = Input.GetAxisRaw("Horizontal")*movespeed;
        verticalMove = Input.GetAxisRaw("Vertical")*movespeed;
        dir=transform.forward*verticalMove+transform.right*horizontalMove;
        characterController.Move(dir * Time.deltaTime);
        if (Input.GetButtonDown("Jump")&&isGround)
        {
            velocity.y = jumpSpeed;
        }
        velocity.y-=gravity*Time.deltaTime;
        characterController.Move(velocity*Time.deltaTime);
        if (tempBuild!=null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(tempBuild.gameObject);
                tempBuild = null;
                return;
            }

            // 当前的建筑物还有剩余份额 && 金币也足够
            if (curr_BuildItem.CanBuild)
            {
                BuildForUpdate();
            }
            else
            {
                Destroy(tempBuild.gameObject);
                tempBuild = null;
                return;
            }


        }

    }

    private UI_ShopItem curr_BuildItem;


    // 建造
    public void Build(UI_ShopItem UI_BuildItem)
    {
        curr_BuildItem = UI_BuildItem;

        tempBuildPrefab = curr_BuildItem.Prefab;
        if (tempBuild!=null)
        {
            Destroy(tempBuild.gameObject);
        }
        tempBuild = GameObject.Instantiate<GameObject>(curr_BuildItem.Prefab).GetComponent<BaseBuild>();
    }

    // 建造在Update中的调用
    private void BuildForUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, int.MaxValue, 1 << LayerMask.NameToLayer("Ground")))
        {
            // 碰撞到地面
            if (hit.collider != null && hit.collider.gameObject.tag == "Ground")
            {
                // 可以吸附的植物
                BaseBuild build = null;
                // 查找有没有很近的植物
                for (int i = 0; i < buildList.Count; i++)
                {
                    // 如果距离小于一定距离，跳出循环，选择这个植物作为吸附对象
                    if (Vector3.Distance(hit.point, buildList[i].transform.position) < (buildList[i].Size/2)+(tempBuild.Size/2) +2)
                    {
                        build = buildList[i];
                        // 跳出
                        break;
                    }
                }
                // 判断是否有可以吸附的植物
                if (build != null)
                {
                    float offset = build.Size / 2 + tempBuild.Size / 2;
                    // 确定四个点
                    Vector3 top = build.transform.position + new Vector3(0, 0, offset);
                    Vector3 bottom = build.transform.position + new Vector3(0, 0, -offset);
                    Vector3 left = build.transform.position + new Vector3(-offset, 0, 0);
                    Vector3 right = build.transform.position + new Vector3(offset, 0, 0);
                    Vector3[] points = new Vector3[] { top, bottom, left, right };

                    float dis = 10000;
                    // 吸附的位置
                    Vector3 tempPoint = Vector3.zero;
                    // 找到最近的点
                    for (int i = 0; i < points.Length; i++)
                    {
                        if (Vector3.Distance(hit.point, points[i]) < dis)
                        {
                            dis = Vector3.Distance(hit.point, points[i]);
                            tempPoint = points[i];
                        }
                    }
                    // 已经可以确定吸附的坐标了
                    tempBuild.transform.position = tempPoint;
                }
                else
                {
                    // 让鼠标处有一个空地跟着跑
                    
                    tempBuild.transform.position = hit.point;
                }

            }
            // 鼠标左键 建造
            if (Input.GetMouseButtonDown(0))
            {
                if (tempBuild.CanCreate)
                {
                    BaseBuild temp = GameObject.Instantiate<GameObject>(tempBuildPrefab, tempBuild.transform.position, Quaternion.identity, null).GetComponent<BaseBuild>();
                    buildList.Add(temp);
                    // 初始化建筑物
                    temp.InitOnPlace(curr_BuildItem.confItem);
                    // 要告诉面板，又创建了一个
                    curr_BuildItem.UpdateBuidlData();
                    // 消耗金币
                    Gold -= curr_BuildItem.Gold;
                }
                else
                {
                    Debug.Log("重叠，不能创建");
                }

            }
        }

    }


}
