using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAction", menuName = "Scriptable Objects/Battle Action", order = 1)]
public class BattleAction: ScriptableObject
{
    public string actionName;
    //AtackPattern???

    public float chargeTime;
    public float actionDuration;
    public float recoveryTime;
    
    public float effectTime;

    public float damage;
    public float health;

    public enum ActionType { Movement, BasicAttack, ProjectileAttack, BeamAttack };
    public ActionType actionType;

    public GameObject actionPrefab;
}
