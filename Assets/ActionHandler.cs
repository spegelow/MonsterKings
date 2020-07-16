using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    public BattleAction action;
    public MapTile tile;
    
    public float effectTime;
    public bool effectActivated;

    public float currentTime;

    public BattleUnit user;
    
    public enum ActionState {Charging, Activating}
    public ActionState actionState;


    public void ActivateAction(BattleAction action, MapTile tile)
    {
        this.action = action;
        this.tile = tile;
        effectTime = action.effectTime;
        effectActivated = false;

        currentTime = 0;
    }

    public void BattleUpdate()
    {
        if(effectActivated == false)
        { 
            currentTime += Time.deltaTime;
            if (currentTime >= effectTime)
            {
                StartEffect(action, user, tile);

                effectActivated = true;
            }  
        }
        else
        {
            if (action.actionType == BattleAction.ActionType.Movement)
            {
                //Movement
                user.transform.position = Vector3.MoveTowards(user.transform.position, tile.transform.position, Time.deltaTime * Stats.GetSpeedCommandGaugeMultiplier(user.monsterData.GetStat(Stats.Stat.Speed))); 
                if (user.transform.position.Equals(tile.transform.position))
                {
                    user.currentTile.occupant = null;
                    user.currentTile = tile;
                    FinishAction();
                }
            }
        }
    }

    public void StartEffect(BattleAction action, BattleUnit unit, MapTile tile)
    {
        this.action = action;
        this.user = unit;
        this.tile = tile;

        if (action.actionType == BattleAction.ActionType.BasicAttack)
        {
            //Basic Attack
            //Setup the attack object. This object will handle all the attack logic
            GameObject go = GameObject.Instantiate(action.actionPrefab, tile.transform.position, Quaternion.identity);
            BasicAttack ba = go.GetComponent<BasicAttack>();
            ba.Initialize(this);
            ba.onAttackFinish.AddListener(FinishAction);
        }
        else if (action.actionType == BattleAction.ActionType.Movement)
        {
            //Movemen
            //Check if the destination is open, if not we are 'blocked'
            if (tile.occupant == null)
            {
                tile.occupant = user;
            }
            else
            {
                Debug.Log(user.name + "'s movement was blocked by " + tile.occupant.name);
                FinishAction();
            }
        }
        else if (action.actionType == BattleAction.ActionType.ProjectileAttack)
        {
            GameObject go = GameObject.Instantiate(action.actionPrefab, user.currentTile.transform.position + new Vector3(0, .75f), Quaternion.identity);
            Projectile p = go.GetComponent<Projectile>();
            p.Initialize(this);
            FinishAction();
        }
        else if (action.actionType == BattleAction.ActionType.BeamAttack)
        {
            GameObject go = GameObject.Instantiate(action.actionPrefab, user.currentTile.transform.position + new Vector3(0, .75f), Quaternion.identity);
            BeamAttack b = go.GetComponent<BeamAttack>();
            b.Initialize(this);
        }
    }

    public void FinishAction()
    {
        //Apply the current effect
        Debug.Log("Used " + action.actionName);
        user.FinishAction();
    }
}
