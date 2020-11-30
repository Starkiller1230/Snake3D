using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIScope : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _tmp.text = $"x{Scope.GetScope}";
        Scope.OnScopeChanged += UpdateScopeText;
    }

    private void UpdateScopeText(int scope)
    {
        if(_tmp != null)
            _tmp.text = $"x{scope}";
    }

    private void OnDestroy()
    {
        Scope.OnScopeChanged -= UpdateScopeText;
    }
}
