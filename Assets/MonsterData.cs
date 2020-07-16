using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Stats;


[CreateAssetMenu(fileName = "New Monster Data", menuName = "Scriptable Objects/Monster Data", order = 1)]
public class MonsterData : ScriptableObject
{
    public MonsterClassData monsterClass;
    public TraitData[] traits;

    public float GetStat(Stat stat)
    {
        return monsterClass.GetStat(stat);
    }
}
