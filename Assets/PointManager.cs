using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PointManager : MonoBehaviour
{
    public int points;
    [SerializeField] TMP_Text pointText;
    void Update()
    {
        pointText.text = "Score: " + points;
    }
    public void AddPoints(int Pints)
    {
        points += Pints;
    }
}
