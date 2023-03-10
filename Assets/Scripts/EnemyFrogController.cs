using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrogController : MonoBehaviour
{
  private Rigidbody2D rb;
  public Transform leftPoint, rightPoint;
  public float speed;
  private bool faceLeft = true;
  private float leftx, rightx;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();

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
    Movement();
  }

  void Movement()
  {
    if (faceLeft)
    {
      rb.velocity = new Vector2(-speed, rb.velocity.y);
      if (transform.position.x < leftx)
      {
        transform.localScale = new Vector3(-1, 1, 1);
        faceLeft = false;
      }
    }
    else
    {
      rb.velocity = new Vector2(speed, rb.velocity.y);
      if (transform.position.x > rightx)
      {
        transform.localScale = new Vector3(1, 1, 1);
        faceLeft = true;
      }
    }
  }
}
