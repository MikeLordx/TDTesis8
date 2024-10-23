using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameManager instance;
    [SerializeField] public int playerCoins = 100;
    [SerializeField] public TextMeshProUGUI coinText;
    [SerializeField] public GameState currentState;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        UpdateCoinUI();
        Time.timeScale = 1.0f;
    }

    public void AddCoins(int amount)
    {
        playerCoins += amount;
        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        coinText.text = playerCoins.ToString();
    }


    private void Update()
    {
        Debug.Log(currentState.ToString());
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

    public void MoveNextScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.MainMenu:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            case GameState.Playing:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case GameState.Paused:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            case GameState.GameOver:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
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