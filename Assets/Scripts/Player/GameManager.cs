using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentState;
    public int playerPoints = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    private void Update()
    {
        Debug.Log(currentState.ToString());
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

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.MainMenu:
                // Handle MainMenu logic
                break;
            case GameState.Playing:
                // Handle Playing logic
                break;
            case GameState.Paused:
                // Handle Paused logic
                break;
            case GameState.GameOver:
                // Handle GameOver logic
                break;
        }
    }
}

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}