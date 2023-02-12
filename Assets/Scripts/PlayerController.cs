using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public Rigidbody2D rb; // 刚体
  public float speed = 10f; // 速度

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

    Movement();
  }

  // 移动逻辑
  void Movement()
  {
    // 获取水平输入
    float horizontalMove = Input.GetAxis("Horizontal");

    // -1: 向左，1：向右
    if (horizontalMove != 0)
    {
      rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
    }
  }
}
