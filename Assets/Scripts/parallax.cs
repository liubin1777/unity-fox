using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
  public Transform cam; // 摄像机
  public float moveRate; // 移动速率
  public bool lockY; // 是否锁定Y轴
  private float startPointX, startPointY; // 开始位置

  // Start is called before the first frame update
  void Start()
  {
    startPointX = transform.position.x;
    startPointY = transform.position.y;
  }

  // Update is called once per frame
  void Update()
  {
    if (lockY)
    {
      transform.position = new Vector2(startPointX + cam.position.x * moveRate, startPointY);
    }
    else
    {
      transform.position = new Vector2(startPointX + cam.position.x * moveRate, startPointY + cam.position.y * moveRate);
    }
  }
}
