using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameManager instance;
    [SerializeField] public int playerCoins = 100;

    [SerializeField] public TextMeshProUGUI coinText1;
    [SerializeField] public TextMeshProUGUI coinText2;

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

    public void SpendCoins(int amount)
    {
        playerCoins -= amount;
        UpdateCoinUI();
    }

    public bool HasEnoughCoins(int towerCost)
    {
        return playerCoins >= towerCost;
    }

    void UpdateCoinUI()
    {
        if (coinText1 != null)
            coinText1.text = playerCoins.ToString();

        if (coinText2 != null)
            coinText2.text = playerCoins.ToString();
    }

    public void MoveNextScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
        ChangeState(GameState.Playing);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
