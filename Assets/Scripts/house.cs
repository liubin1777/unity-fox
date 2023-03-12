using UnityEngine;

public class house : MonoBehaviour
{
  public GameObject dialogUI; // 对话框
  public Animator dialogAnimator; // 对话框动画组件

  private void Awake()
  {
    dialogAnimator = dialogUI.GetComponent<Animator>();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Player")
    {
      dialogAnimator.SetBool("enter", true);
      dialogAnimator.SetBool("exit", false);
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (other.tag == "Player")
    {
      dialogAnimator.SetBool("exit", true);
      dialogAnimator.SetBool("enter", false);
    }
  }
}