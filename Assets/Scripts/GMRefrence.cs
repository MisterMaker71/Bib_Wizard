using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMRefrence : MonoBehaviour
{
    [HideInInspector]
    public GameManager gm;
    void Start()
    {
        if (GameManager.gameManager != null)
            gm = GameManager.gameManager;
        else
            gm = FindFirstObjectByType<GameManager>();
    }
    public void GoToGame(bool newGame)
    {
        if (gm != null)
            gm.GoToGame(newGame);
    }
    public void GoToMenu()
    {
        if (gm != null)
            gm.GoToMenu();
    }
    public void Quit()
    {
        if (gm != null)
            gm.Quit();
    }
    public void GoTo3D()
    {
        if (gm != null)
            gm.GoTo3D();
    }
}
