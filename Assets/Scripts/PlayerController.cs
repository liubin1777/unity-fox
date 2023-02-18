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
  public Collider2D collider; // 碰撞体

  public float speed = 400f; // 水平速度
  public float jumpForce; // 跳跃的力

  public int cherryCount; // 吃掉的樱桃个数


  private bool isJump; // 是否跳跃状态

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
    SwitchAnim();
  }

  void Update()
  {
    checkInputStatus();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Collection")
    {
      Destroy(other.gameObject);
      cherryCount++;
    }
  }

  // 检测输入系统
  void checkInputStatus()
  {
    // 检测跳跃
    if (Input.GetButtonDown("Jump") && collider.IsTouchingLayers(ground))
    {
      this.isJump = true;
    }
  }

  // 移动逻辑
  void Movement()
  {

    // 获取水平输入
    float horizontalMove = Input.GetAxis("Horizontal"); // 获取 -1。。。0.。。1 之间的小数
    float faceDirection = Input.GetAxisRaw("Horizontal"); // 获取 -1 0 1 三个整数

    // 水平速度x < 0: 向左，> 0：向右
    if (horizontalMove != 0)
    {
      rb.velocity = new Vector2(horizontalMove * speed * Time.deltaTime, rb.velocity.y);
      animator.SetFloat("running", MathF.Abs(horizontalMove));
    }

    // 水平方向
    if (faceDirection != 0)
    {
      // Debug.Log("Input.GetButtonDown|水平方向");
      transform.localScale = new Vector3(faceDirection, 1, 1);
    }

    // 跳跃
    // if (Input.GetButtonDown("Jump"))
    // if (Input.GetKeyDown(KeyCode.Space))
    if (this.isJump)
    {
      // Debug.Log("Input.Jump = " + Input.GetAxis("Vertical"));
      rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
      animator.SetBool("jumping", true);
      this.isJump = false;
    }
  }

  // 变换动画
  void SwitchAnim()
  {

    animator.SetBool("idle", false);

    if (animator.GetBool("jumping"))
    {
      if (rb.velocity.y < 0)
      {
        animator.SetBool("jumping", false);
        animator.SetBool("falling", true);
      }
    }
    else if (collider.IsTouchingLayers(ground))
    {
      animator.SetBool("falling", false);
      animator.SetBool("idle", true);
    }
  }

}
