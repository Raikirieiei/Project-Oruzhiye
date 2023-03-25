using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatReward : ScriptableObject
{
    public new string name;
    public string desc;
    public int baseValue;
    public Sprite img;

    public virtual void Selected(CharacterStats characterStat){}
}
