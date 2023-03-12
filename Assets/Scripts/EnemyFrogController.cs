using UnityEngine;

public class EnemyFrogController : BaseEnemy
{
  private Rigidbody2D rb; // 刚体
  private Collider2D myCollider; // 碰撞器
  // private Animator animator; // 动画控制器
  public Transform leftPoint, rightPoint;
  public float speed, jumpForce;
  private bool faceLeft = true;
  private float leftx, rightx;
  public LayerMask ground; // 地面图层

  // Start is called before the first frame update
  protected override void Start()
  {
    base.Start();

    // 初始化的时候获取组件
    rb = GetComponent<Rigidbody2D>();
    // animator = GetComponent<Animator>();
    myCollider = GetComponent<Collider2D>();

    // 解除子对象关系
    // transform.DetachChildren();

    leftx = leftPoint.position.x;
    rightx = rightPoint.position.x;
    Destroy(leftPoint.gameObject);
    Destroy(rightPoint.gameObject);
  }

  // Update is called once per frame
  void Update()
  {
    // Movement();
    SwitchAnim();
  }

  // 移动
  void Movement()
  {

    // 地面上跳跃
    if (myCollider.IsTouchingLayers(ground))
    {
      animator.SetBool("jumping", true);
      animator.SetBool("falling", false);

      rb.velocity = new Vector2(speed * (faceLeft ? -1 : 1), jumpForce);
    }
  }

  // 切换动画
  void SwitchAnim()
  {
    // 最高点后切换下落状态
    if (rb.velocity.y < 0.1f)
    {
      animator.SetBool("falling", true);
      animator.SetBool("jumping", false);
    }

    if (myCollider.IsTouchingLayers(ground) && animator.GetBool("falling"))
    {
      animator.SetBool("falling", false);
    }

    if (myCollider.IsTouchingLayers(ground))
    {
      // 面向左侧运动
      if (faceLeft)
      {
        // 超过左侧锚点镜像掉头
        if (transform.position.x < leftx)
        {
          transform.localScale = new Vector3(-1, 1, 1);
          faceLeft = false;
        }
      }
      else // 面向右侧运动
      {
        //   rb.velocity = new Vector2(speed, jumpForce);
        if (transform.position.x > rightx)
        {
          transform.localScale = new Vector3(1, 1, 1);
          faceLeft = true;
        }
      }
    }

  }

  // public void Death()
  // {
  //   animator.SetTrigger("death");
  // }

  // void DestoryMe()
  // {
  //   Destroy(gameObject);
  // }
}
