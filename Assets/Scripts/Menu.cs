using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
  public GameObject pauseMenu;
  public AudioMixer audioMixer;

  public void playGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public void quitGame()
  {
    Application.Quit();
  }

  public void uiEnable()
  {
    GameObject.Find("Canvas/MainMenu/UI").SetActive(true);
  }

  public void pauseGame()
  {
    pauseMenu.SetActive(true);
    Time.timeScale = 0;
  }

  public void resumeGame()
  {
    pauseMenu.SetActive(false);
    Time.timeScale = 1;
  }

  public void setVolume(float value)
  {
    audioMixer.SetFloat("MainVolume", value);
  }
}
