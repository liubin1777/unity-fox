using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  public static SoundManager instance;
  public AudioSource audioSource;
  public AudioClip jump, hurt, cherry;

  private void Awake()
  {
    instance = this;
  }

  public void playJump()
  {
    audioSource.clip = jump;
    audioSource.Play();
  }

  public void playHurt()
  {
    audioSource.clip = hurt;
    audioSource.Play();
  }

  public void playCherry()
  {
    audioSource.clip = cherry;
    audioSource.Play();
  }
}
