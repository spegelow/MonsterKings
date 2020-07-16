using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elements;
using static Stats;

[CreateAssetMenu(fileName = "New Monster Class", menuName = "Scriptable Objects/Monster Class", order = 1)]
public class MonsterClassData : ScriptableObject
{
    //Core Stats
    public int[] stats;

    //Elements
    public Element primaryElement;
    public Element secondaryElement;

    public float GetStat(Stat stat)
    {
        return stats[(int)stat];
    }
}
