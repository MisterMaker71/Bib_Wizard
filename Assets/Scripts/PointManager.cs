using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PointManager : MonoBehaviour
{
    public static int points;
    public TMP_Text pointText;
    public Slider mana;
    public TMP_Text manaText;
    public Slider helth;
    public TMP_Text helthText;
    void Update()
    {
        pointText.text = "Score: " + points;
        helthText.text = "Health: " + Mathf.Round(helth.value);
        manaText.text = "Mana: " + Mathf.Round(mana.value);
    }
    public static void AddPoints(int Pints)
    {
        points += Pints;
    }
}
