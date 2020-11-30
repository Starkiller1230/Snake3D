using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class Food : MonoBehaviour
{
    public static int FoodCount { get; private set; }
    public static event Action<int> OnFoodCountChanged;

    [SerializeField] private Light _light;
    [SerializeField] private float _lifeTime;

    private Animation _animation;
    private float _timeToDIE;

    private void Start()
    {
        ++FoodCount;
        OnFoodCountChanged?.Invoke(FoodCount);
        ThemeController.OnThemeChange += ChangeTheme;
        _timeToDIE = _lifeTime + Time.time - 1;
        _animation = GetComponent<Animation>();
    }

    private void ChangeTheme(ThemeName theme)
    {
        if (_light != null)
            _light.gameObject.SetActive(theme == ThemeName.Night ? true : false);
    }

    private void OnDestroy()
    {
        --FoodCount;
        OnFoodCountChanged?.Invoke(FoodCount);
        ThemeController.OnThemeChange -= ChangeTheme;
    }

    private void FixedUpdate()
    {
        if(Time.time > _timeToDIE)
        {
            _animation.Play("AppleDie");
            Destroy(gameObject, 1);

        }
    }

}
