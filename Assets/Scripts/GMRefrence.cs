using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMRefrence : MonoBehaviour
{
    [HideInInspector]
    public GameManager gm;
    void Start()
    {
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
}
