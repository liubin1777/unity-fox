using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{


  [SerializeField]
  private Rigidbody2D rb; // 刚体
  private Animator animator; // 动画控制器

  public Transform groundCheck; // 地面检测

  public LayerMask ground; // 地面图层
  public Collider2D circleCollider2D; // 碰撞体
  public Collider2D boxCollider2D; // 碰撞体

  public float speed = 400f; // 水平速度
  private float speedFactor; // 水平奔跑速度因子
  private float faceDirection; // 水平朝向

  public float jumpForce; // 跳跃的力
  private int jumpCount; // 可跳跃次数

  public int cherryCount; // 吃掉的樱桃个数
  public TextMeshProUGUI cherryText;

  private bool isJump; // 是否跳跃状态
  private bool isCrouch; // 是否下蹲状态
  private bool isHurt; // 是否受到伤害
  private bool isGround; // 是否在地面上

  public AudioSource jumpAudio; // 跳跃音效
  public AudioSource hurtAudio; // 受伤害的音效

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
  }

  // FixedUpdate 一般处理物理相关的
  void FixedUpdate()
  {
    // Debug.Log("在 fixedUpdate 中执行");
    // Debug.Log("time:" + Time.time);
    // Debug.Log("deltatime" + Time.deltaTime);
    // Debug.Log("fixedtime:" + Time.fixedTime);
    // Debug.Log("fixedDeltatimetime:" + Time.fixedDeltaTime);

    if (!this.isHurt)
    {
      Movement();

      checkJump();
    }
  }

  void Update()
  {
    // Debug.Log("在 update 中执行");

    checkInputStatus();
    SwitchAnim();
  }

  // 碰撞到触发器回调函数
  void OnTriggerEnter2D(Collider2D other)
  {
    // Debug.Log(other.tag);
    if (other.tag == "Collection")
    {
      // 直接播放 cherry 名字为 collection 的动画
      other.gameObject.GetComponent<Animator>().Play("collection");
      // other.gameObject.GetComponent<Animator>().SetTrigger("collection");
      // Destroy(other.gameObject);
      // this.cherryCount++;
    }

    // 碰撞到死亡线的时候，出发死亡逻辑
    if (other.tag == "DeathLine")
    {
      // 关闭所有音乐
      GetComponent<AudioSource>().enabled = false;
      // 2s 后调用 restart 方法
      Invoke("restart", 2f);
    }
  }

  public void updateCherryCount()
  {
    this.cherryCount++;
    this.cherryText.text = this.cherryCount.ToString();
  }

  // 消灭敌人
  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Enemy")
    {
      if (animator.GetBool("falling"))
      {
        rb.velocity = new Vector2(rb.velocity.x, this.jumpForce * Time.deltaTime);

        // 消灭敌人
        BaseEnemy enemy = other.gameObject.GetComponent<BaseEnemy>();
        enemy.death();
      }
      else if (transform.position.x < other.gameObject.transform.position.x)
      {
        Debug.Log("[player] 左侧受伤");
        this.rb.velocity = new Vector2(-4, rb.velocity.y);
        // hurtAudio.Play();
        SoundManager.instance.playHurt();
        this.isHurt = true;

      }
      else if (transform.position.x > other.gameObject.transform.position.x)
      {
        Debug.Log("[player] 右侧受伤");
        this.rb.velocity = new Vector2(4, rb.velocity.y);
        // hurtAudio.Play();
        SoundManager.instance.playHurt();
        this.isHurt = true;
      }
    }
  }

  // 检测跳跃
  void checkJump()
  {
    // 二段跳
    isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);
    // isGround = circleCollider2D.IsTouchingLayers(ground);

    if (isGround && rb.velocity.y == 0)
    {
      // Debug.Log("地面上");
      this.jumpCount = 2;
    }

    if (this.isJump && jumpCount > 0)
    {
      // Debug.Log("跳跃-before" + this.isJump + " | " + jumpCount);

      --this.jumpCount;
      rb.velocity = Vector2.up * jumpForce * Time.fixedDeltaTime;
      animator.SetBool("jumping", true);
      this.isJump = false;

      SoundManager.instance.playJump();

      // rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);

      Debug.Log("跳跃-after" + this.isJump + " | " + jumpCount);

    }

    // 检测跳跃 --- 一段跳
    // if (Input.GetButtonDown("Jump") && circleCollider2D.IsTouchingLayers(ground))
    // {
    //   jumpAudio.Play();
    //   this.isJump = true;
    // }
    // else if (!circleCollider2D.IsTouchingLayers(ground))
    // {
    //   this.isJump = false;
    // }
  }

  // 检测输入系统
  void checkInputStatus()
  {

    if (Input.GetButtonDown("Jump") && this.jumpCount > 0)
    {
      Debug.Log("触发跳跃");
      this.isJump = true;
    }

    // 获取水平输入
    this.speedFactor = Input.GetAxis("Horizontal"); // 获取 -1。。。0.。。1 之间的小数
    this.faceDirection = Input.GetAxisRaw("Horizontal"); // 获取 -1 0 1 三个整数

    // 检测下蹲
    if (Input.GetKey(KeyCode.S))
    {
      // Debug.Log("蹲下");
      this.isCrouch = true;
    }
    else if (!boxCollider2D.IsTouchingLayers(ground)) // 头顶没有遮挡物才能站立起来 , 也可以使用 Physics2D.OverlapCircle 方式来检测
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
      rb.velocity = new Vector2(this.speedFactor * speed * Time.fixedDeltaTime, rb.velocity.y);
    }

    // 水平方向
    if (this.faceDirection != 0)
    {
      // Debug.Log("Input.GetButtonDown|水平方向");
      transform.localScale = new Vector3(this.faceDirection, 1, 1);
    }

  }

  // 变换动画
  void SwitchAnim()
  {

    // 默认 idle false
    // animator.SetBool("idle", false);

    // 水平速度
    animator.SetFloat("running", this.isCrouch ? 0 : MathF.Abs(this.speedFactor));

    // 跳跃下落 or idle 切换
    if (animator.GetBool("jumping"))
    {
      if (rb.velocity.y < 0)
      {
        Debug.Log("触发下落动画");
        animator.SetBool("jumping", false);
        animator.SetBool("falling", true);
      }
    }
    else if (circleCollider2D.IsTouchingLayers(ground))
    {
      animator.SetBool("falling", false);
    }

    // 受伤动画
    animator.SetBool("hurt", this.isHurt);
    // 受伤动画执行后恢复状态标识
    if (this.isHurt && Mathf.Abs(this.rb.velocity.x) < 0.1f)
    {
      this.isHurt = false;
    }

    // 下蹲
    animator.SetBool("crouch", this.isCrouch);
    this.boxCollider2D.isTrigger = this.isCrouch;
  }

  void restart()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
