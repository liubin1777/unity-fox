using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
  public AudioSource cherryAudio; // 吃樱桃的音效
  public BoxCollider2D myCollider;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void disableTrigger()
  {
    cherryAudio.Play();
    myCollider.enabled = false;
  }

  void destoryMe()
  {
    FindObjectOfType<PlayerController>().updateCherryCount();
    Destroy(gameObject);
  }
}
