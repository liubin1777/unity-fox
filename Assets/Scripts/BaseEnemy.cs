using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人基类
public class BaseEnemy : MonoBehaviour
{
  protected Animator animator;

  // Start is called before the first frame update
  protected virtual void Start()
  {
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void death()
  {
    animator.SetTrigger("death");
  }

  public void destoryMe()
  {
    Destroy(gameObject);
  }
}
