using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int playerCoins = 100;
    public TextMeshProUGUI coinText;
    public GameState currentState;

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
        UpdateCoinUI();
    }

    private void Update()
    {
        Debug.Log(currentState.ToString());
    }

    public void AddCoins(int amount)
    {
        playerCoins += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        coinText.text = playerCoins.ToString();
    }

    public bool HasEnoughCoins(int towerCost)
    {
        return playerCoins >= towerCost;
    }

    public void SpendCoins(int amount)
    {
        playerCoins -= amount;
        UpdateCoinUI();
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