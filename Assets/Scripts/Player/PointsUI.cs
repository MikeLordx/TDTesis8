using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointsUI : MonoBehaviour
{
    private TextMeshProUGUI pointsText;
    private int points = 0;

    public void UpdatePoints(int pointsToAdd)
    {
        points += pointsToAdd;
        pointsText.text = points.ToString();
    }
}