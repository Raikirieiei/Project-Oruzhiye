using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StatReward : ScriptableObject
{
    public new string name;
    public int baseValue;

    public static event Action<int> OnSelected;

    public void Selected(){
        OnSelected?.Invoke(baseValue);
    }
}
