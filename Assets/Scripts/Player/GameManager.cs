using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int playerPoints = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddPoints(int points)
    {
        playerPoints += points;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Para puntos o que? Dea
    }
}