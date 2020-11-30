using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStamina : MonoBehaviour
{
    [SerializeField] private Snake _snake;
    [SerializeField] private Slider _slider;
    
    private Vector3 _zeroSizeImage;

    private void Start()
    {
        if (_snake != null)
            _snake.OnChangeStamina += StaminaChanged;
    }

    private void StaminaChanged(float currentStamina, float maxStamina)
    {
        if (_slider != null)
        {
            _slider.value = currentStamina / maxStamina;
        }
    }

    private void OnDestroy()
    {
        if (_snake != null)
            _snake.OnChangeStamina -= StaminaChanged;
    }

}
