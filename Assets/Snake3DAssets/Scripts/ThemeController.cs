using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThemeName
{
    Day = 0,
    Night = 1
}

public class ThemeController : MonoBehaviour
{
    public static event Action<ThemeName> OnThemeChange;

    public static ThemeName Theme
    {
        get => _theme;
        set
        {
            if(_theme != value)
            {
                _theme = value;
                OnThemeChange?.Invoke(_theme);
            }
        }
    }

    private static ThemeName _theme = ThemeName.Day;

    [SerializeField] private Light _sun;
    [SerializeField] private Vector3 _sunRotateNight;
    [SerializeField] private float _sunNightIntensity;

    private Vector3 _sunRotateDay = new Vector3(66, 48, 0);
    private float _sunIntensityDay = 1;

    private void Start()
    {
        OnThemeChange += SetSunRotate;
        SetSunRotate(Theme);
    }

    private void SetSunRotate(ThemeName theme)
    {
        if (_sun == null)
            return;
        _sun.transform.rotation = Quaternion.Euler(theme == ThemeName.Night ? _sunRotateNight : _sunRotateDay);
        _sun.intensity = theme == ThemeName.Night ? _sunNightIntensity : _sunIntensityDay;

    }

    private void OnDestroy()
    {
        OnThemeChange -= SetSunRotate;
    }

}
