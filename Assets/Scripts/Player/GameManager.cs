using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameManager instance;
    [SerializeField] public int playerCoins = 100;

    [SerializeField] public TextMeshProUGUI coinText1;
    [SerializeField] public TextMeshProUGUI coinText2;

    [SerializeField] public GameState currentState;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject sword;

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

    public void GameStateIsPlaying()
    {
        ChangeState(GameState.Playing);
    }

    public void GameStateIsMenu()
    {
        ChangeState(GameState.MainMenu);
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
                Time.timeScale = 1;
                break;
            case GameState.Playing:
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                character.GetComponent<AreaAttack>().enabled = true;
                sword.GetComponent<MeleeAttack>().enabled = true;
                character.GetComponent<CharacterAttack>().enabled = true;
                break;
            case GameState.Paused:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            case GameState.PlacingTower:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                character.GetComponent<AreaAttack>().enabled = false;
                sword.GetComponent<MeleeAttack>().enabled = false;
                character.GetComponent<CharacterAttack>().enabled = false;
                break;
            case GameState.GameOver:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            case GameState.Victory:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }

        Debug.Log($"Estado cambiado a: {currentState}");
    }
}

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    PlacingTower,
    GameOver,
    Victory
}
