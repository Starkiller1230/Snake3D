using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioSource _menuMusic;
    [SerializeField] private List<AudioSource> _sfx = new List<AudioSource>();

    public void SetGameMusic(Scrollbar volume) => SetMusicVolume(volume.value, _gameMusic);

    public void SetMenuMusic(Scrollbar volume) => SetMusicVolume(volume.value, _menuMusic);

    public void SetSFXMusic(Scrollbar volume)
    {
        foreach (var sfx in _sfx)
            if (sfx != null)
                sfx.volume = volume.value;

    }

    private void SetMusicVolume(float volume, AudioSource audioSource)
    {
        if (audioSource != null)
            audioSource.volume = volume;
    }

    private void Start()
    {
        GameController.OnChangeGameStatus += ChangeGameStatus;
        ChangeGameStatus(GameController.GameStatus);
    }

    private void ChangeGameStatus(GameStatus gameStatus)
    {
        if (_menuMusic == null || _gameMusic == null)
            return;

        if (gameStatus == GameStatus.Game)
        {
            _gameMusic.Play();
            _menuMusic.Stop();
        }
        else if(gameStatus == GameStatus.Start || gameStatus == GameStatus.Pause || gameStatus == GameStatus.GameOver)
        {
            _menuMusic.Play();
            _gameMusic.Stop();
        }

    }

    private void OnDestroy()
    {
        GameController.OnChangeGameStatus -= ChangeGameStatus;
    }
}
