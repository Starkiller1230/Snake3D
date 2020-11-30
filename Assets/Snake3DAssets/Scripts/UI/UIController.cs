using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UIController : MonoBehaviour
{
    [SerializeField] private CanvasRenderer _pausePanel;
    [SerializeField] private CanvasRenderer _settingPanel;
    [SerializeField] private Button _start;
    [SerializeField] private Button _resume;
    [SerializeField] private Button _restart;
    [SerializeField] private Button _exit;
    [SerializeField] private Button _setTheme;

    public void StartGame() => GameController.GameStatus = GameStatus.Game;
    public void PauseGame() => GameController.GameStatus = GameStatus.Pause;
    public void ResumeGame() => GameController.GameStatus = GameStatus.Game;
    public void RestartGame() => GameController.GameStatus = GameStatus.Restart;
    public void ExitGame() => GameController.GameStatus = GameStatus.Exit;

    public void PauseOrResumeGame()
    {
        if (GameController.GameStatus == GameStatus.Game || GameController.GameStatus == GameStatus.Pause)
            GameController.GameStatus = GameController.GameStatus == GameStatus.Game ? GameStatus.Pause : GameStatus.Game;
    }

    public void ShowOrHideSettingPanel()
    {
        if (_settingPanel == null)
            return;

        _settingPanel.gameObject.SetActive(!_settingPanel.gameObject.activeSelf);

        if (_settingPanel.gameObject.activeSelf == true && GameController.GameStatus == GameStatus.Game)
            GameController.GameStatus = GameStatus.Pause;
    }

    public void SetTheme()
    {
        ThemeController.Theme = ThemeController.Theme == ThemeName.Day ? ThemeName.Night : ThemeName.Day;
    }

    private void Awake()
    {
        if (_settingPanel != null)
            _settingPanel.gameObject.SetActive(false);

        GameController.OnChangeGameStatus += GameStatusChanged;
        GameController.GameStatus = GameStatus.Start;
        GameStatusChanged(GameController.GameStatus);
    }

    private void GameStatusChanged(GameStatus gameStatus)
    {
        if (_pausePanel != null)
        {
            _pausePanel.gameObject.SetActive(gameStatus != GameStatus.Game ? true : false);
            if (_pausePanel.gameObject.activeSelf == true)
            {
                if (_start != null)
                    _start.gameObject.SetActive(gameStatus == GameStatus.Start ? true : false);
                if (_resume != null)
                    _resume.gameObject.SetActive((gameStatus == GameStatus.Pause) ? true : false);
                if (_restart != null)
                    _restart.gameObject.SetActive((gameStatus != GameStatus.Game && gameStatus != GameStatus.Start) ? true : false);
                if (_exit != null)
                    _exit.gameObject.SetActive(gameStatus != GameStatus.Game ? true : false);
            }
        }

        if (gameStatus == GameStatus.Game)
            _settingPanel.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameController.OnChangeGameStatus -= GameStatusChanged;
    }

}
