using UnityEngine;

public class EnemyEagleController : BaseEnemy
{
  private Rigidbody2D rb; // 刚体
  private Collider2D myCollider; // 碰撞器
  // private Animator animator; // 动画控制器
  public Transform topPoint, bottomPoint;
  public float speed;
  private bool isUp = true;
  private float topY, bottomY;

  // 重写父类的方法
  protected override void Start()
  {
    // 调用父类
    base.Start();

    // 初始化的时候获取组件
    rb = GetComponent<Rigidbody2D>();
    // animator = GetComponent<Animator>();
    myCollider = GetComponent<Collider2D>();

    // 解除子对象关系
    // transform.DetachChildren();

    topY = topPoint.position.y;
    bottomY = bottomPoint.position.y;
    Destroy(topPoint.gameObject);
    Destroy(bottomPoint.gameObject);
  }

  // Update is called once per frame
  void Update()
  {
    Movement();
  }

  void Movement()
  {
    // 向上运动
    if (isUp)
    {
      rb.velocity = new Vector2(rb.velocity.x, speed);
      // 超过 top 锚点
      if (transform.position.y > topY)
      {
        isUp = false;
      }
    }
    else // 向下运动
    {
      rb.velocity = new Vector2(rb.velocity.x, -speed);
      if (transform.position.y < bottomY)
      {
        isUp = true;
      }
    }
  }
}
