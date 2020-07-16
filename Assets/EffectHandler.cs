using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectHandler
{
    //public UnityEvent onEffectDone;

    //public BattleAction action;
    //public BattleUnit user;
    //public MapTile tile;

    //public EffectHandler()
    //{
    //    onEffectDone = new UnityEvent();
    //}

    //public void StartEffect(BattleAction action, BattleUnit unit, MapTile tile)
    //{
    //    this.action = action;
    //    this.user = unit;
    //    this.tile = tile;

    //    if (action.actionType == BattleAction.ActionType.BasicAttack)
    //    {
    //        //Basic Attack
    //        //Setup the attack object. This object will handle all the attack logic
    //        GameObject go = GameObject.Instantiate(action.actionPrefab, tile.transform.position, Quaternion.identity);
    //        BasicAttack ba = go.GetComponent<BasicAttack>();
    //        ba.Initialize(this);
    //        ba.onAttackFinish.AddListener(EndEffect);
    //    }
    //    else if (action.actionType == BattleAction.ActionType.Movement)
    //    {
    //        //Movemen
    //        //Check if the destination is open, if not we are 'blocked'
    //        if(tile.occupant == null)
    //        {
    //            tile.occupant = user;
    //        }
    //        else
    //        {
    //            Debug.Log(user.name + "'s movement was blocked by " + tile.occupant.name);
    //            EndEffect();
    //        }
    //    }
    //    else if (action.actionType == BattleAction.ActionType.ProjectileAttack)
    //    {
    //        GameObject go = GameObject.Instantiate(action.actionPrefab, user.currentTile.transform.position + new Vector3(0,.75f), Quaternion.identity);
    //        Projectile p = go.GetComponent<Projectile>();
    //        p.Initialize(this);
    //        EndEffect();
    //    }
    //    else if (action.actionType == BattleAction.ActionType.BeamAttack)
    //    {
    //        GameObject go = GameObject.Instantiate(action.actionPrefab, user.currentTile.transform.position + new Vector3(0, .75f), Quaternion.identity);
    //        BeamAttack b = go.GetComponent<BeamAttack>();
    //        b.Initialize(this);
    //    }
    //}

    //public void BattleUpdate()
    //{
    //    if (action.actionType == BattleAction.ActionType.Movement)
    //    {
    //        //Movement
    //        user.transform.position = Vector3.MoveTowards(user.transform.position, tile.transform.position, Time.deltaTime * 1); //REPLACE 1 with unit move speed
    //        if(user.transform.position.Equals(tile.transform.position))
    //        {
    //            user.currentTile.occupant = null;
    //            user.currentTile = tile;
    //            EndEffect();
    //        }
    //    }
    //}

    //public void EndEffect()
    //{
    //    onEffectDone.Invoke();
    //}
}
