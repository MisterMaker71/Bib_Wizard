using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool cursorLocked;
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
        cursorLocked = Cursor.lockState == CursorLockMode.Locked;
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
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        }
        else
        {
            if (cursorLocked)
                Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

}
