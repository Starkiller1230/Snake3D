using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scope
{
    public static event Action<int> OnScopeChanged;
    public static int GetScope => _scope;

    private static int _scope = 0;

    public static void AddScope(int value) => OnScopeChanged?.Invoke(_scope += value);

    public static void ResetScope() => _scope = 0;

}
