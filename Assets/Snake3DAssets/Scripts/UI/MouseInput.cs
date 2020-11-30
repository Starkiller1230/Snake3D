using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MouseInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnMouseDown;
    public UnityEvent OnMouseUp;

    [SerializeField] private Sprite _dayThemeSprite;
    [SerializeField] private Sprite _nightThemeSprite;
    [SerializeField] private Color _dayThemeColor;
    [SerializeField] private Color _nightThemeColor;
    [SerializeField] private bool _changeColor;

    private Image _image;

    private void Start()
    {
        ThemeController.OnThemeChange += ThemeChanged;
        _image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData) => OnMouseDown?.Invoke();

    public void OnPointerUp(PointerEventData eventData) => OnMouseUp?.Invoke();

    private void ThemeChanged(ThemeName theme)
    {
        if (_dayThemeSprite != null && _nightThemeSprite != null)
            _image.sprite = theme == ThemeName.Day ? _dayThemeSprite : _nightThemeSprite;

        if (_changeColor == true)
            _image.color = theme == ThemeName.Day ? _dayThemeColor : _nightThemeColor;
    }

    private void OnDestroy()
    {
        ThemeController.OnThemeChange -= ThemeChanged;
    }
}
