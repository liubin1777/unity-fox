using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人基类
public class BaseEnemy : MonoBehaviour
{
  protected Animator animator;
  protected AudioSource audioSource;

  // Start is called before the first frame update
  protected virtual void Start()
  {
    animator = GetComponent<Animator>();
    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void death()
  {
    audioSource.Play();
    animator.SetTrigger("death");
  }

  public void destoryMe()
  {
    Destroy(gameObject);
  }
}
