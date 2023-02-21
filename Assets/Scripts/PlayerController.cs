using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField]
  private Rigidbody2D rb; // 刚体
  private Animator animator; // 动画控制器

  public LayerMask ground; // 地面图层
  public Collider2D circleCollider2D; // 碰撞体
  public Collider2D boxCollider2D; // 碰撞体

  public float speed = 400f; // 水平速度
  private float speedFactor; // 水平奔跑速度因子
  private float faceDirection; // 水平朝向

  public float jumpForce; // 跳跃的力

  public int cherryCount; // 吃掉的樱桃个数

  private bool isJump; // 是否跳跃状态
  private bool isCrouch; // 是否下蹲状态

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
  }

  // FixedUpdate 一般处理物理相关的
  void FixedUpdate()
  {
    Movement();
  }

  void Update()
  {
    checkInputStatus();
    SwitchAnim();
  }

  // 碰撞到触发器回调函数
  void OnTriggerEnter2D(Collider2D other)
  {
    // Debug.Log(other.tag);
    if (other.tag == "Collection")
    {
      Destroy(other.gameObject);
      cherryCount++;
    }
  }

  // 检测输入系统
  void checkInputStatus()
  {
    // 获取水平输入
    this.speedFactor = Input.GetAxis("Horizontal"); // 获取 -1。。。0.。。1 之间的小数
    this.faceDirection = Input.GetAxisRaw("Horizontal"); // 获取 -1 0 1 三个整数

    // 检测跳跃
    if (Input.GetButtonDown("Jump") && circleCollider2D.IsTouchingLayers(ground))
    {
      this.isJump = true;
    }
    else if (!circleCollider2D.IsTouchingLayers(ground))
    {
      this.isJump = false;
    }

    // 检测下蹲
    if (Input.GetKey(KeyCode.S))
    {
      // Debug.Log("蹲下");
      this.isCrouch = true;
    }
    else if (!boxCollider2D.IsTouchingLayers(ground)) // 头顶没有遮挡物才能站立起来
    {
      // Debug.Log("头顶没有接触障碍物");
      this.isCrouch = false;
    }
  }

  // 移动逻辑
  void Movement()
  {

    // 水平速度x < 0: 向左，> 0：向右
    if (this.speedFactor != 0)
    {
      rb.velocity = new Vector2(this.speedFactor * speed * Time.deltaTime, rb.velocity.y);
    }

    // 水平方向
    if (this.faceDirection != 0)
    {
      // Debug.Log("Input.GetButtonDown|水平方向");
      transform.localScale = new Vector3(this.faceDirection, 1, 1);
    }

    // 跳跃
    if (this.isJump)
    {
      // Debug.Log("Input.Jump = " + Input.GetAxis("Vertical"));
      rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
    }

  }

  // 变换动画
  void SwitchAnim()
  {

    // 默认 idle false
    animator.SetBool("idle", false);

    // 水平速度
    animator.SetFloat("running", this.isCrouch ? 0 : MathF.Abs(this.speedFactor));

    // 跳跃
    if (this.isJump)
    {
      animator.SetBool("jumping", true);
    }

    // 跳跃下落 or idle 切换
    if (animator.GetBool("jumping"))
    {
      if (rb.velocity.y < 0)
      {
        animator.SetBool("jumping", false);
        animator.SetBool("falling", true);
      }
    }
    else if (circleCollider2D.IsTouchingLayers(ground))
    {
      animator.SetBool("falling", false);
      animator.SetBool("idle", true);
    }

    // 下蹲
    animator.SetBool("crouch", this.isCrouch);
    this.boxCollider2D.isTrigger = this.isCrouch;
  }

}
