using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme : MonoBehaviour
{
    [SerializeField] private ThemeName _activeTheme;
    [SerializeField] private bool _allThemes = false;

    private void Awake()
    {
        ThemeController.OnThemeChange += ThemeChanged;
        ThemeChanged(ThemeController.Theme);
    }

    private void ThemeChanged(ThemeName theme)
    {
        if(_allThemes == true)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(_activeTheme == theme ? true : false);
    }

    private void OnDestroy()
    {
        ThemeController.OnThemeChange -= ThemeChanged;
    }
}
