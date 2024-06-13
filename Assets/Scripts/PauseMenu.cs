using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]GameObject pauseMenu;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }
    void Start()
    {
        SetPause(false);
    }
    public void TogglePause()
    {
        SetPause(!pauseMenu.activeSelf);
    }
    public void SetPause(bool paused)
    {
        pauseMenu.SetActive(paused);
        if(paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
