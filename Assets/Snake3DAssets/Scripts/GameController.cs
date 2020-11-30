using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStatus
{
    Null = 0,
    Start = 1,
    Game = 2,
    Pause = 3,
    GameOver = 4,
    Restart = 5,
    Exit = 6
}

public class GameController
{
    public static event Action<GameStatus> OnChangeGameStatus = GameStatusChanged;

    public static GameStatus GameStatus
    {
        get => _gameStatus;
        set
        {
            if (_gameStatus != value)
            {
                _gameStatus = value;
                OnChangeGameStatus?.Invoke(_gameStatus);
            }
        }
    }

    private static GameStatus _gameStatus;

    private static void StartGame()
    {
        Time.timeScale = 0;
    }

    private static void PauseGame()
    {
        Time.timeScale = 0;
    }

    private static void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private static void RestartGame()
    {
        Scope.ResetScope();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private static void GameOver()
    {
        Time.timeScale = 0;
    }

    private static void ExitGame()
    {
        Application.Quit();

    }

    private static void GameStatusChanged(GameStatus gameStatus)
    {
        switch (gameStatus)
        {
            case GameStatus.Start:
                StartGame();
                break;
            case GameStatus.Game:
                ResumeGame();
                break;
            case GameStatus.Pause:
                PauseGame();
                break;
            case GameStatus.GameOver:
                GameOver();
                break;
            case GameStatus.Restart:
                RestartGame();
                break;
            case GameStatus.Exit:
                ExitGame();
                break;
            default:
                break;
        }

        Cursor.lockState = gameStatus == GameStatus.Game ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = gameStatus == GameStatus.Game ? false : true;

        //Debug.Log(gameStatus.ToString());
    }

}
